using System.Security.Claims;
using System.Threading.Tasks;
using SaeedRezayi.Common;
using SaeedRezayi.DataLayer.Context;
using SaeedRezayi.Api.Areas.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SaeedRezayi.ViewModels.Account;
using SaeedRezayi.Services.Account;
using SaeedRezayi.Services.Contracts.Account;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.ViewModels.Account.Login;
using SaeedRezayi.ViewModels.Account.User;
using System.Collections.Generic;
using SaeedRezayi.ViewModels.Types;
using DNTCommon.Web.Core;

namespace SaeedRezayi.Api.Account.Controllers
{
    [Area(AreaConstants.AccountArea)]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IUsersService _usersService;
        private readonly ITokenStoreService _tokenStoreService;
        private readonly IAntiForgeryCookieService _antiforgery;
        private readonly ITokenFactoryService _tokenFactoryService;

        public AccountController(
            IUsersService usersService,
            ITokenStoreService tokenStoreService,
            ITokenFactoryService tokenFactoryService,
            IUnitOfWork uow,
            IAntiForgeryCookieService antiforgery)
        {
            _usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(usersService));

            _tokenStoreService = tokenStoreService;
            _tokenStoreService.CheckArgumentIsNull(nameof(tokenStoreService));

            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _antiforgery = antiforgery;
            _antiforgery.CheckArgumentIsNull(nameof(antiforgery));

            _tokenFactoryService = tokenFactoryService;
            _tokenFactoryService.CheckArgumentIsNull(nameof(tokenFactoryService));
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseViewModel>> Login([FromBody] LoginRequestViewModel loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest("user is not set.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserInfo user = await _usersService
                .FindUserAsync(loginUser);

            if (user?.IsActive != true)
            {
                return Unauthorized();
            }

            var result = await _tokenFactoryService.CreateJwtTokensAsync(user);
            await _tokenStoreService.AddUserTokenAsync(user, result.RefreshTokenSerial, result.AccessToken, null);
            await _uow.SaveChangesAsync();

            _antiforgery.RegenerateAntiForgeryCookies(result.Claims);

            return Ok(new LoginResponseViewModel
            {
                Access_token = result.AccessToken,
                Refresh_token = result.RefreshToken
            });
        }

        [AllowAnonymous]
        [IgnoreAntiforgeryToken]
        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponseViewModel>> RefreshToken([FromBody] RefreshTokenRequestViewModel refreshTokenRequest)
        {
            //var refreshTokenValue = model.RefreshToken;
            if (string.IsNullOrWhiteSpace(refreshTokenRequest.RefreshToken))
            {
                return BadRequest("refreshToken is not set.");
            }

            var token = await _tokenStoreService.FindTokenAsync(refreshTokenRequest);
            if (token == null)
            {
                return Unauthorized();
            }

            JwtTokensDataViewModel result = await _tokenFactoryService
                .CreateJwtTokensAsync(token.User);

            await _tokenStoreService
                .AddUserTokenAsync(token.User, result.RefreshTokenSerial, result.AccessToken, _tokenFactoryService.GetRefreshTokenSerial(refreshTokenRequest.RefreshToken));
            await _uow.SaveChangesAsync();

            _antiforgery.RegenerateAntiForgeryCookies(result.Claims);

            return Ok(new LoginResponseViewModel
            {
                Access_token = result.AccessToken,
                Refresh_token = result.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<bool> Logout(RefreshTokenRequestViewModel refreshTokenRequest)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userIdValue = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;

            // The Jwt implementation does not support "revoke OAuth token" (logout) by design.
            // Delete the user's tokens from the database (revoke its bearer token)
            await _tokenStoreService
                .RevokeUserBearerTokensAsync(userIdValue, refreshTokenRequest.RefreshToken);
            await _uow.SaveChangesAsync();

            _antiforgery.DeleteAntiForgeryCookies();

            return true;
        }

        [NoBrowserCache]
        [HttpGet("[action]"), HttpPost("[action]")]
        public bool IsAuthenticated()
        {
            return User.Identity.IsAuthenticated;
        }

        [NoBrowserCache]
        [HttpGet("[action]"), HttpPost("[action]")]
        public ActionResult<GetUserInfoResponseViewModel> GetUserInfo()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            return Ok(new GetUserInfoResponseViewModel
            {
                Username = claimsIdentity.Name
            });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async ValueTask<UserViewModel> Index(int Id)
        {
            Id.CheckArgumentIsNull(nameof(Id));
            return await _usersService.FindUserAsync(Id, true);
        }

        [ResponseCache(Duration = 10)]
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IEnumerable<UserViewModel>> Users(string field = "", int maxRecords = 10, SortingOrderTypes sortOrder = SortingOrderTypes.Descending)
        {
            maxRecords = maxRecords > 200 ? 10 : maxRecords;
            return await _usersService.GetPagedUsersListAsync(field, maxRecords, sortOrder);
        }
    }
}
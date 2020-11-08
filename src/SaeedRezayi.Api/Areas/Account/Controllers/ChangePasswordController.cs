using System.Threading.Tasks;
using SaeedRezayi.Common;
using SaeedRezayi.Services.Account;
using SaeedRezayi.Api.Areas.Account;
using SaeedRezayi.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SaeedRezayi.Services.Contracts.Account;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.ViewModels.Account.ChangePassword;

namespace SaeedRezayi.Api.Account.Controllers
{
    [Area(AreaConstants.AccountArea)]
    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class ChangePasswordController : ControllerBase
    {
        private readonly IUsersService _usersService;
        public ChangePasswordController(IUsersService usersService)
        {
            _usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(usersService));
        }

        // Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult<ChangePasswordResponseViewModel>> ChangePassword([FromBody] ChangePasswordRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserInfo user = await _usersService.GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest("NotFound");
            }

            var result = await _usersService.ChangePasswordAsync(user, model);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Error);
        }
    }
}
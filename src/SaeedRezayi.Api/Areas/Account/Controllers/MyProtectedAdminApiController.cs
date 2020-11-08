using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaeedRezayi.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using SaeedRezayi.Api.Areas.Account;
using SaeedRezayi.Services.Account;
using SaeedRezayi.ViewModels.Account;
using SaeedRezayi.Services.Contracts.Account;

namespace SaeedRezayi.Api.Account.Controllers
{
    [Area(AreaConstants.AccountArea)]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Authorize(Policy = CustomRolesViewModel.Admin)]
    public class MyProtectedAdminApiController : Controller
    {
        private readonly IUsersService _usersService;

        public MyProtectedAdminApiController(IUsersService usersService)
        {
            _usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(usersService));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity.FindFirst(ClaimTypes.UserData);
            var userId = userDataClaim.Value;

            return Ok(new
            {
                Id = 1,
                Title = "Hello from My Protected Admin Api Controller! [Authorize(Policy = CustomRoles.Admin)]",
                Username = this.User.Identity.Name,
                UserData = userId,
                TokenSerialNumber = await _usersService.GetSerialNumberAsync(int.Parse(userId)),
                Roles = claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList()
            });
        }
    }
}
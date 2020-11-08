using SaeedRezayi.Api.Areas.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SaeedRezayi.ViewModels.Account;

namespace SaeedRezayi.Api.Account.Controllers
{
    [Area(AreaConstants.AccountArea)]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Authorize(Policy = CustomRolesViewModel.Editor)]
    public class MyProtectedEditorsApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Id = 1,
                Title = "Hello from My Protected Editors Controller! [Authorize(Policy = CustomRoles.Editor)]",
                Username = this.User.Identity.Name
            });
        }
    }
}
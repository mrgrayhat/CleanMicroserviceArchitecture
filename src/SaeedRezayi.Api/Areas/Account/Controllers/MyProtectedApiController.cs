using SaeedRezayi.Api.Areas.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace SaeedRezayi.Api.Account.Controllers
{
    [Area(AreaConstants.AccountArea)]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class MyProtectedApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                Id = 1,
                Title = "Hello from My Protected Controller! [Authorize]",
                Username = this.User.Identity.Name
            });
        }
    }
}
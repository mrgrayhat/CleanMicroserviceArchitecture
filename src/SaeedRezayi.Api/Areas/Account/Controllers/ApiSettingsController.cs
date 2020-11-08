using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SaeedRezayi.Common;
using SaeedRezayi.Api.Areas.Account;
using SaeedRezayi.ViewModels.Api;

namespace SaeedRezayi.Api.Account.Controllers
{
    [Area(AreaConstants.AccountArea)]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [Produces("application/json")]
    public class ApiSettingsController : Controller
    {
        private readonly IOptionsSnapshot<ApiSettingsViewModel> _apiSettingsConfig;

        public ApiSettingsController(IOptionsSnapshot<ApiSettingsViewModel> apiSettingsConfig)
        {
            _apiSettingsConfig = apiSettingsConfig;
            _apiSettingsConfig.CheckArgumentIsNull(nameof(apiSettingsConfig));
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<ApiSettingsViewModel> Get()
        {
            return Ok(_apiSettingsConfig.Value); // For the Angular Client
        }
    }
}
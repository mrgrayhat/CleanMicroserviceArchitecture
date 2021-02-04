using System;
using Gateway.Web.Api.Services.Contracts;
using Gateway.Web.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Web.Api.Controllers
{
    [AllowAnonymous]
    public class AppController : BaseController
    {
        private readonly IApplicationService _applicationService;

        public AppController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        //public AppController()
        //{

        //}

        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect("~/");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ApplicationDataViewModel> GetApplicationData()
        {
            var appData = _applicationService.GetApplicationData();

            return Ok(appData);
        }
    }
}

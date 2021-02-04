using Microsoft.AspNetCore.Mvc;
using STS.Application.Abstractions;

namespace STS.Web.ViewComponents
{
    public class ReturnLinkViewComponent : ViewComponent
    {
        private readonly IClientInfoService _clientInfoService;

        public ReturnLinkViewComponent(IClientInfoService clientInfoService)
        {
            _clientInfoService = clientInfoService;
        }

        public IViewComponentResult Invoke()
        {
            return View<string>(_clientInfoService.GetClient().ClientUri);
        }
    }
}

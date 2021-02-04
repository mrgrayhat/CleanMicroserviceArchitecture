using System.Collections.Generic;

namespace Gateway.Web.Api.ViewModels
{
    public class ApplicationDataViewModel
    {
        public Dictionary<string, string> Content { get; set; }
        public object CookieConsent { get; set; }
        public IEnumerable<CulturesDisplayViewModel> Cultures { get; set; }
        public EnvironmentInformation EnvironmentInfo { get; set; }
    }
}

using Gateway.Web.Api.ViewModels;

namespace Gateway.Web.Api.Services.Contracts
{
    public interface IApplicationService
    {
        ApplicationDataViewModel GetApplicationData();
    }
}
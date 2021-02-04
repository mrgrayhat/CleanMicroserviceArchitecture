using STS.Application.Models;

namespace STS.Application.Abstractions
{
    public interface IApplicationService
    {
        ApplicationDataViewModel GetApplicationData();
    }
}
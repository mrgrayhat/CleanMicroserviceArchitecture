using BlogModule.Application.Models;

namespace BlogModule.Application.Abstractions
{
    public interface IApplicationService
    {
        ApplicationDataViewModel GetApplicationData();
    }
}
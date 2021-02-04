using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace BlogModule.Web.Api
{
    public static class ServiceExtensions
    {
        public static IMvcBuilder AddBLogModuleControllers(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPart(Assembly.GetAssembly(typeof(BlogModule.Web.Api.Controllers.v1.BlogController)));

            return mvcBuilder;
        }
    }
}

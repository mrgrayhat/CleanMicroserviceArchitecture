using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(STS.Web.Areas.Identity.IdentityHostingStartup))]
namespace STS.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}
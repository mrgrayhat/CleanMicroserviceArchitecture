using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

//[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Gateway.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            IWebHost host = CreateWebHostBuilder(args).Build();

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration((host, config) =>
               {
                   config.AddJsonFile("ocelot.json", false, true)
                   .AddEnvironmentVariables();
               })
               .UseSerilog((hostingContext, loggerConfiguration) =>
               {
                   loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
               })
               .UseStartup<Startup>();
        }
    }
}

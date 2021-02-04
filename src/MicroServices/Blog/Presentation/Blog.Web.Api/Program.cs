using System;
using System.Threading.Tasks;
using BlogModule.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

//[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace Blog.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Seeding Web And Localization Databases");

                    var mediator = services.GetRequiredService<IMediator>();

                    var localizationDbContext = services
                        .GetRequiredService<ILocalizationDbContext>();
                    localizationDbContext.Database.Migrate();

                    await Task.Delay(10); //TODO: Remove this

                    //var env = services.GetRequiredService<IWebHostEnvironment>();
                    //await mediator.Send(new LocalizationDataSeederCommand { ContentRoot = env.ContentRootPath }, CancellationToken.None);

                    //var applicationDbContext = services.GetRequiredService<IBlogDbContext>();
                    // applicationDbContext.Database.Migrate();
                    //await mediator.Send(new WebDataSeederCommand(), CancellationToken.None);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error creating/seeding API database - " + ex);
                }
            }

            host.Run();
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
            })
            .UseStartup<Startup>();

    }
}

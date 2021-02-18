using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using StorageManagement.Application.Interfaces;

namespace StorageManagement.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            IHost host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                IServiceProvider services = scope.ServiceProvider;
                ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Press x to cancel StorageAgent task");
                    var tokenSource = new CancellationTokenSource();
                    var cancellationToken = tokenSource.Token;
                    IStorageAgent storageAgent = services.GetRequiredService<IStorageAgent>();
                    //await storageAgent.CleanupDatabase(cancellationToken);
                    await Task.Run(() =>
                    {
                        Task<Task> task = storageAgent.CleanupDatabase(cancellationToken);
                        while (!task.IsCompleted)
                        {
                            if (Console.ReadKey().Key == ConsoleKey.X)
                            {
                                tokenSource.Cancel();
                                logger.LogWarning("x key pressed, Operation Cancelled.");
                                break;
                            }
                        }
                        return Task.CompletedTask;
                    }, cancellationToken);


                }
                catch (Exception ex)
                {
                    logger.LogCritical("Error executing/completeing Storage Agent Service" + ex);
                    //Log.CloseAndFlush();
                    //await host.StopAsync();
                }
            }


            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                });
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace SaeedRezayi.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    //webBuilder.UseSerilog((builder, logger) =>
                    //{
                    //string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logs");
                    //string logsPath = builder.Configuration
                    //    .GetConnectionString("LogDatabase")
                    //    .Replace("|DataDirectory|", basePath);

                    //if (builder.HostingEnvironment.IsDevelopment())
                    //{
                    //    logger.WriteTo.Console().MinimumLevel.Verbose();
                    //}
                    //else if (builder.HostingEnvironment.IsProduction())
                    //{
                    //    logger.WriteTo.MSSqlServer(logsPath,
                    //            new SinkOptions
                    //            {
                    //                TableName = "AppLogs",
                    //                AutoCreateSqlTable = true
                    //            })
                    //        .MinimumLevel.Error();
                    //}
                    // end serilog builder
                    //});

                })
                .UseSerilog((hostingContext, loggerConfig) =>
                {
                    loggerConfig.ReadFrom
                    .Configuration(hostingContext.Configuration);
                    if (hostingContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerConfig.WriteTo.Console().MinimumLevel.Information();
                    }
                    else
                    {
                        loggerConfig.WriteTo.Console().MinimumLevel.Information();
                    }
                });
        }
    }
}
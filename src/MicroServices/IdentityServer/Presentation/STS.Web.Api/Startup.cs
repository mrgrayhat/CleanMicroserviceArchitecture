using System;
using HealthChecks.UI.Client;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using STS.Application;
using STS.Application.Abstractions;
using STS.Application.Services;
using STS.Common;
using STS.Infrastructure;
using STS.Infrastructure.Identity;
using STS.Web.Seed;

namespace STS.Web
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            HostingEnvironment = environment;
            Serilog.Debugging.SelfLog.Enable((log) =>
            {
                Log.Fatal(log);
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IApplicationService, ApplicationService>();
            services.AddTransient<IProfileService, CustomProfileService>();
            services.AddTransient<IIdentitySeedData, IdentitySeedData>();

            services
                .AddApplication()
                .AddInfrastructure(Configuration, HostingEnvironment);

            services.AddStsServer(Configuration, HostingEnvironment);

            #region HealthCheck Modules
            bool.TryParse(Configuration["HealthCheck:DbCheck"], out bool dbCheck);
            bool.TryParse(Configuration["HealthCheck:StorageCheck"], out bool storageCheck);

            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
            if (dbCheck)
            {
                Console.WriteLine($"Database Health Check is {dbCheck}");
                services.AddHealthChecks()
                        .AddDbContextCheck<IdentityServerDbContext>("Database", HealthStatus.Unhealthy, new[] { "identity db" });
            }
            if (storageCheck)
            {
                long.TryParse(Configuration["HealthCheck:MinFreeDisk"], out long minFreeSize);
                string storagePath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Windows).Split('\\')[0], "\\");
                Console.WriteLine($"Storage Health Check is {storageCheck} with minimum {minFreeSize} MB.");
                //services.AddHealthChecks()
                //        .AddDiskStorageHealthCheck(opt => opt.AddDrive(storagePath,
                //                minFreeSize),
                //                "Storage Disk", failureStatus: HealthStatus.Degraded);
            }
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseInfrastructure(HostingEnvironment);

            app.UseRouting();
            app.UseCors(Constants.DefaultCorsPolicy);

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc",
                    new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                        //ResultStatusCodes =
                        //{
                        //    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        //    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        //    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                        //}
                    });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}

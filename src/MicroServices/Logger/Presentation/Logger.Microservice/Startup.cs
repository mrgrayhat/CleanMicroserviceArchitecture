using System;
using HealthChecks.UI.Client;
using LogModule.Application;
using LogModule.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace Logger.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        //public IWebHostEnvironment WebHostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddLogModuleApplicationLayer()
                .AddLogModulePersistenceInfrastructure(Configuration);

            services.AddHttpContextAccessor()
                .AddResponseCompression()
                .AddMemoryCache();

            #region HealthCheck Modules
            string dbCon = Configuration["LogModuleSettings:ConnectionString"];
            bool.TryParse(Configuration["HealthCheck:DbCheck"], out bool dbCheck);
            bool.TryParse(Configuration["HealthCheck:StorageCheck"], out bool storageCheck);
            long.TryParse(Configuration["HealthCheck:MinFreeDisk"], out long minFreeSize);

            Console.WriteLine($"Database Health Check is {dbCheck}");
            Console.WriteLine($"Storage Health Check is {storageCheck} with minimum {minFreeSize} MB.");

            string storagePath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Windows).Split('\\')[0], "\\");

            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
            if (dbCheck)
                services.AddHealthChecks()
                        .AddMongoDb(dbCon,
                                    "Database",
                                    HealthStatus.Unhealthy);
            if (storageCheck)
                services.AddHealthChecks()
                        .AddDiskStorageHealthCheck(opt => opt.AddDrive(storagePath,
                                minFreeSize),
                                "Storage Disk", failureStatus: HealthStatus.Degraded);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRequestLogging();

            app.UseRouting();

            app.UseAuthorization();

            app.UseApiVersioning();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc",
                    new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                endpoints.MapControllers();
            });
        }
    }
}

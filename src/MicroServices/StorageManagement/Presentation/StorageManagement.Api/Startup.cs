using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using StorageManagement.Application;
using StorageManagement.Infrastructure;
using StorageManagement.Infrastructure.Shared;

namespace StorageManagement.Api
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly IWebHostEnvironment HostingEnvironment;

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            Serilog.Debugging.SelfLog.Enable((log) =>
            {
                Log.Fatal(log);
            });
        }


        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Layers Extensions
            services.AddApplication()
                    .AddInfrastructures(Configuration)
                    .AddStoragePersistenceInfrastructure(Configuration, HostingEnvironment)
                    .AddSharedInfrastructures();
            #endregion

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "StorageManagement.Api",
                    Version = "v1"
                });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StorageManagement.Api v1"));
            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/wwwroot/api";
                settings.DocumentPath = "/wwwroot/api/specification.json";
            });
            app.UseRouting();

            app.UseAuthorization();
            //app.UseAuthentication();
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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StorageManagement.Application;
using StorageManagement.Infrastructure;
using StorageManagement.Infrastructure.Shared;

namespace StorageManagement.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Layers Extensions
            services.AddApplication()
                    .AddInfrastructures(Configuration/*, HostingEnvironment*/)
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
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StorageManagement.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();
            //app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

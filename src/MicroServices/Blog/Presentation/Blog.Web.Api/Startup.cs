using System;
using System.Collections.Generic;
using Blog.Localization.Infrastructure;
using BlogModule.Application;
using BlogModule.Infrastructure;
using BlogModule.Infrastructure.Contexts;
using BlogModule.Infrastructure.Shared;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Blog.Api
{
    public class Startup
    {
        private IWebHostEnvironment HostingEnvironment { get; }
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            HostingEnvironment = env;
            Configuration = configuration;
            Serilog.Debugging.SelfLog.Enable((log) =>
            {
                Serilog.Log.Fatal(log);
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddBlogModuleApplicationLayer()
                    .AddInfrastructures(Configuration, HostingEnvironment)
                    .AddCustomLocalization(Configuration, HostingEnvironment)
                    .AddBlogModulePersistenceInfrastructure(Configuration, HostingEnvironment)
                    .AddBlogModuleSharedInfrastructure(Configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // base-address of your identity server
                    options.Authority = Configuration["Auth:Authority"];
                    // name of the API resource
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidAudiences = Configuration
                        .GetSection("Auth:Audiences").Get<List<string>>(),
                    };
                });

            #region HealthCheck Modules
            bool.TryParse(Configuration["HealthCheck:DbCheck"], out bool dbCheck);
            bool.TryParse(Configuration["HealthCheck:StorageCheck"], out bool storageCheck);

            if (storageCheck)
            {
                long.TryParse(Configuration["HealthCheck:MinFreeDisk"], out long minFreeSize);
                string storagePath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Windows).Split('\\')[0], "\\");
                Console.WriteLine($"Storage Health Check is {storageCheck} with minimum {minFreeSize} MB.");
            }
            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());
            if (dbCheck)
            {
                Console.WriteLine($"Database Health Check is {dbCheck}");
                services.AddHealthChecks()
                        .AddDbContextCheck<BlogDbContext>("Blog Database", HealthStatus.Unhealthy, new[] { "blog-database" });
            }
            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddBlogModuleDbInitializer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHealthChecks("/hc");

            app.UseHttpsRedirection();


            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });

            app.UseRouting();

            app.UseAuthentication();
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
    }
}

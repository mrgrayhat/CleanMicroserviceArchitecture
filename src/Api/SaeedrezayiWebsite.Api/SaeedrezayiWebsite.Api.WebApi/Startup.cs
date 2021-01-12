using BlogModule.Application;
using BlogModule.Infrastructure;
using LogModule.Application;
using LogModule.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SaeedrezayiWebsite.Api.Application;
using SaeedrezayiWebsite.Api.Application.Interfaces;
using SaeedrezayiWebsite.Api.Infrastructure.Identity;
using SaeedrezayiWebsite.Api.Infrastructure.Persistence;
using SaeedrezayiWebsite.Api.Infrastructure.Shared;
using SaeedrezayiWebsite.Api.WebApi.Extensions;
using SaeedrezayiWebsite.Api.WebApi.Services;


namespace SaeedrezayiWebsite.Api.WebApi
{
    public class Startup
    {
        public IConfiguration _config { get; }
        public Startup(IConfiguration configuration)
        {
            _config = configuration;

        }
        public void ConfigureServices(IServiceCollection services)
        {

            #region log module
            services.AddLogModuleApplicationLayer();
            services.AddLogModulePersistenceInfrastructure(_config);
            #endregion

            services.AddApplicationLayer();
            services.AddIdentityInfrastructure(_config);
            services.AddPersistenceInfrastructure(_config);
            services.AddSharedInfrastructure(_config);

            #region BlogModule
            services.AddBlogModuleApplicationLayer();
            services.AddBlogModulePersistenceInfrastructure(_config);
            services.AddBlogModuleSharedInfrastructure(_config);
            //services.AddBLogModuleControllers();
            #endregion

            services.AddSwaggerExtension();
            services.AddControllers()
                //.AddLogModuleController()
                //.AddApplicationPart(Assembly.GetAssembly(typeof(LogModule.Application.Controllers.LogController)))
                .AddNewtonsoftJson(opt =>
                   {
                       opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                       opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                       opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                       //opt.SerializerSettings.Formatting = Formatting.Indented;
                       opt.UseMemberCasing();
                   });
            services.AddApiVersioningExtension();

            services.AddHealthChecks();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddBlogModuleDbInitializer();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            //app.UseRequestLogging();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwaggerExtension();
            app.UseErrorHandlingMiddleware();
            app.UseHealthChecks("/health");

            app.UseEndpoints(endpoints =>
             {
                 endpoints.MapControllers();
             });
        }
    }
}

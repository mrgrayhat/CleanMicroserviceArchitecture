using System.Linq;
using LogModule.Application.Interfaces;
using LogModule.Application.Interfaces.Repositories;
using LogModule.Domain.Settings;
using LogModule.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace LogModule.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddLoggerInfrastructures(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCustomUi()
                .AddLogModulePersistenceInfrastructure(configuration);

            return services;
        }

        /// <summary>
        /// Configure Log Database. <inheritdoc cref="IConfiguration"/>
        /// AddLogModule Service. <inheritdoc cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration">configuration instannce</param>
        public static IServiceCollection AddLogModulePersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LogDatabaseSettings>(options =>
                       configuration.GetSection("LogModuleSettings").Bind(options));
            services.TryAddSingleton<ILogDatabaseSettings>(sp =>
               sp.GetRequiredService<IOptions<LogDatabaseSettings>>().Value); // singleton cuz no need to get setting's always, just once when module initialized.

            #region Repositories
            services.AddSingleton(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddSingleton<ILogRepositoryAsync, LogRepositoryAsync>();
            #endregion

            return services;
        }

        private static IServiceCollection AddCustomUi(this IServiceCollection services)
        {
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Blog API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(),
                    new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}."
                    });

                configure.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //var controllerWithViews = services.AddControllersWithViews();
            services.AddControllers().AddJsonOptions((opt) =>
            {
                opt.JsonSerializerOptions.WriteIndented = true;
            });

            return services;
        }
    }
}

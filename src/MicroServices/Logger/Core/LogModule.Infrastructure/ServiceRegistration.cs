using LogModule.Application.Interfaces;
using LogModule.Application.Interfaces.Repositories;
using LogModule.Domain.Settings;
using LogModule.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LogModule.Infrastructure
{
    public static class ServiceRegistration
    {
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
    }
}

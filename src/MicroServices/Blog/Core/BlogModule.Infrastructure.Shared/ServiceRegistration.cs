using BlogModule.Application.Interfaces;
using BlogModule.Domain.Settings;
using BlogModule.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogModule.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBlogModuleSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}

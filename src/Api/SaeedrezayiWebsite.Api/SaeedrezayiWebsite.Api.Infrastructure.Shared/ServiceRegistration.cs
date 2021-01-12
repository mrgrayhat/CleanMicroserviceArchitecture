using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaeedrezayiWebsite.Api.Application.Interfaces;
using SaeedrezayiWebsite.Api.Domain.Settings;
using SaeedrezayiWebsite.Api.Infrastructure.Shared.Services;

namespace SaeedrezayiWebsite.Api.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}

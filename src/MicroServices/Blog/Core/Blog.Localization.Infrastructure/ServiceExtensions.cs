using System.Collections.Generic;
using System.Globalization;
using Blog.Localization.Infrastructure.Localization;
using Blog.Localization.Infrastructure.Localization.EFLocalizer;
using BlogModule.Application.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

namespace Blog.Localization.Infrastructure
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddCustomLocalization(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-GB"),
                        new CultureInfo("en-US"),
                        new CultureInfo("fa-IR"),
                        new CultureInfo("ar-SA"),
                        new CultureInfo("fr-FR")
                    };

                    options.DefaultRequestCulture = new RequestCulture(
                        culture: "en-US", uiCulture: "en-US");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });

            services.AddDbContext<LocalizationDbContext>(options =>
            {
                options.UseSqlite(configuration["Data:Localization"],
                    b => b.MigrationsAssembly(typeof(LocalizationDbContext).Assembly.FullName));

                if (environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                }
            },
                ServiceLifetime.Singleton,
                ServiceLifetime.Singleton);

            services.AddSingleton<IStringLocalizerFactory, EFStringLocalizerFactory>();
            services.AddSingleton<ILocalizationDbContext>(provider => provider.GetService<LocalizationDbContext>());

            return services;
        }
    }
}

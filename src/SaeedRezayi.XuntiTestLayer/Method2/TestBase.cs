using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SaeedRezayi.IoCConfig;
using SaeedRezayi.Services.Core;

namespace SaeedRezayi.XuntiTestLayer
{
    public class TestsBase
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public TestsBase()
        {
            ServiceProvider = getServiceProvider();
            DbInit();
        }

        private static IServiceProvider getServiceProvider()
        {
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", reloadOnChange: true, optional: false)
                .AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string,string>("UseInMemoryDatabase", "true"),
                })
                .Build();
            var localDBTestConnection = configuration.GetSection("LocalDBTest");
            var csdfd = configuration.GetConnectionString("LocalDBTest");
            services.AddSingleton<IConfigurationRoot>(provider => configuration);
            services.AddCustomOptions(configuration);
            services.AddCustomServices();
            services.AddBlogServices();
            services.AddCustomDbContext(configuration, true);
            services.AddCustomJwtBearer(configuration);
            services.AddCustomCors();
            services.AddCustomAntiforgery();
            //services.AddCustomMvcSetup();
            //services.AddCustomSwagger();

            // services.Configure<SmtpConfig>(options => configuration.GetSection("SmtpConfig").Bind(options));
            // services.Configure<AntiDosConfig>(options => configuration.GetSection("AntiDosConfig").Bind(options));
            // services.Configure<AntiXssConfig>(options => configuration.GetSection("AntiXssConfig").Bind(options));

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            // services.AddDNTCommonWeb();

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
        private void DbInit()
        {
            var scopeFactory = ServiceProvider.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializerService>();
                dbInitializer.Initialize(true);
                Console.WriteLine("db Test Initializer invoked successfully.");
                dbInitializer.SeedData();
                Console.WriteLine("db Test Seed invoked successfully.");
            }

            //var dbInitializer = serviceProvider.GetService<IDbInitializerService>();
            //dbInitializer.Initialize(true);
            //dbInitializer.SeedData();

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NSwag;
using NSwag.Generation.Processors.Security;
using StorageManagement.Application.Abstractions;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Common;
using StorageManagement.Infrastraucture.Common.HealthChecks;
using StorageManagement.Infrastructure.Contexts;
using StorageManagement.Infrastructure.Repositories;

namespace StorageManagement.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration/*, IWebHostEnvironment environment*/)
        {

            services.AddHttpContextAccessor()
                    .AddResponseCompression()
                    .AddMemoryCache();

            services.AddCustomConfiguration(configuration)
                //.AddCustomSignalR()
                .AddCustomCors(configuration)
                .AddCustomUi();
            services.AddHealthChecking(configuration);
            return services;
        }
        private static IServiceCollection AddHealthChecking(this IServiceCollection services, IConfiguration configuration)
        {
            #region HealthCheck Modules
            var healthChecks = services.AddHealthChecks();

            healthChecks.AddCheck("self", () => HealthCheckResult.Healthy());
            if (configuration.GetValue<bool>("HealthCheck:DbCheck"))
            {
                Console.WriteLine($"Database Health Check is Enabled for {nameof(StorageDbContext)}.");
                healthChecks.AddDbContextCheck<StorageDbContext>(name: "Storage DbContext Check",
                                    failureStatus: HealthStatus.Unhealthy,
                                    tags: new[] { "StorageDbContext" });
            }
            if (configuration.GetValue<bool>("HealthCheck:StorageCheck"))
            {
                long.TryParse(configuration["HealthCheck:MinFreeDisk"], out long minFreeSize);
                string storagePath = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Windows).Split('\\')[0], "\\");
                Console.WriteLine($"Storage Health Check is Enabled with minimum {minFreeSize} MB, for Volume: {storagePath}");

                healthChecks.AddDiskStorageHealthCheck(opt => opt.AddDrive(
                    driveName: storagePath,
                    minimumFreeMegabytes: minFreeSize),
                    name: "Storage Management Disk",
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "StorageDisk" });
            }
            if (configuration.GetValue<bool>("HealthCheck:MemoryCheck"))
            {
                long.TryParse(configuration["HealthCheck:MinFreeMemory"], out long minFreeMemory);
                Console.WriteLine($"System Memory Health Check is Enabled with minimum {minFreeMemory} MB.");
                healthChecks.AddCheck<SystemMemoryHealthcheck>("Memory");
            }
            #endregion

            return services;
        }
        private static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Custom configuration
            //services.ConfigurePoco<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.Configure<FormOptions>(x =>
            {
                x.MultipartBodyLengthLimit = configuration.GetValue<long>("Storage:UploadLimitSize");
            });
            return services;
        }
        private static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Constants.DefaultCorsPolicy,
                    builder =>
                    {
                        var corsList = configuration.GetSection("CorsOrigins").Get<List<string>>()?.ToArray() ?? new string[] { };
                        builder
                        //.WithOrigins(corsList)
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            return services;
        }
        public static IServiceCollection AddStoragePersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {

            services.AddDbContext<StorageDbContext>(options =>
            {
                bool.TryParse(configuration["Data:useSqLite"], out var useSqlite);
                bool.TryParse(configuration["Data:useSqlServer"], out var useSqlServer);
                bool.TryParse(configuration["Data:useInMemory"], out var useInMemory);
                string connectionString = configuration.GetConnectionString("Default");

                if (useInMemory)
                {
                    connectionString = configuration.GetConnectionString("InMemory");
                    options.UseInMemoryDatabase(nameof(StorageManagement)); // Takes database name
                }
                else if (useSqlServer)
                {
                    connectionString = configuration["ConnectionStrings:SqlServer"];
                    options.UseSqlServer(connectionString, b =>
                    {
                        b.MigrationsAssembly(typeof(StorageDbContext).Assembly.FullName);
                        //b.UseNetTopologySuite();
                    });
                }
                else if (useSqlite)
                {
                    connectionString = configuration.GetConnectionString("SqLite");
                    options.UseSqlite(connectionString, b =>
                    {
                        b.MigrationsAssembly(typeof(StorageDbContext).Assembly.FullName);
                        //b.UseNetTopologySuite();
                    });
                    options.ConfigureWarnings(x => x.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.AmbientTransactionWarning));
                }
                else
                {
                    options.UseSqlServer(configuration["ConnectionStrings:Default"]);
                }

                if (environment.EnvironmentName == "Development")
                {
                    options.EnableSensitiveDataLogging();
                }
            });

            #region old
            //services.AddDbContext<BlogDbContext>(options =>
            //{
            //    options.UseSqlite(configuration.GetConnectionString("Identity"),
            //        b => b.MigrationsAssembly(typeof(BlogDbContext).Assembly.FullName));

            //    if (environment.IsDevelopment())
            //    {
            //        options.EnableSensitiveDataLogging();
            //    }
            //});

            //var output = Directory.CreateDirectory(Path.Combine(Directory
            //.GetCurrentDirectory(), "wwwroot", "app_data")).FullName;

            //string con = configuration.GetConnectionString("BlogConnection")
            //    .Replace("|DataDirectory|", output);
            //services.AddDbContext<BlogDbContext>(options =>
            //{
            //    options.UseSqlServer(con,
            //    b => b.MigrationsAssembly(typeof(BlogDbContext).Assembly.FullName));

            //if (environment.IsDevelopment())
            //{
            //    options.EnableSensitiveDataLogging();
            //}
            //});
            #endregion

            services.AddScoped<IStorageDbContext>(provider =>
            provider.GetService<StorageDbContext>());

            //services.AddScoped<IDbInitializerService, DbInitializerService>();
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IStorageRepositoryAsync, StorageRepositoryAsync>();
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

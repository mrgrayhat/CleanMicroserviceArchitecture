using System.Collections.Generic;
using System.Linq;
using Blog.Common;
using BlogModule.Application.Abstractions;
using BlogModule.Application.Interfaces;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Infrastructure.Contexts;
using BlogModule.Infrastructure.Repositories;
using BlogModule.Infrastructure.Seeds;
using LogModule.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace BlogModule.Infrastructure
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddInfrastructures(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {

            services.AddHttpContextAccessor()
                    .AddResponseCompression()
                    .AddMemoryCache();

            services.AddCustomConfiguration(configuration)
                //.AddCustomSignalR()
                .AddCustomCors(configuration)
                .AddCustomUi(environment);

            return services;
        }
        private static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Custom configuration
            //services.ConfigurePoco<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));

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
        public static IServiceCollection AddBlogModulePersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {

            services.AddDbContext<BlogDbContext>(options =>
            {
                bool.TryParse(configuration["Data:useSqLite"], out var useSqlite);
                bool.TryParse(configuration["Data:useInMemory"], out var useInMemory);
                var connectionString = configuration["Data:Blog"];

                if (useInMemory)
                {
                    options.UseInMemoryDatabase(nameof(BlogModule)); // Takes database name
                }
                else if (useSqlite)
                {
                    options.UseSqlite(connectionString, b =>
                    {
                        b.MigrationsAssembly(typeof(BlogDbContext).Assembly.FullName);
                        //b.UseNetTopologySuite();
                    });
                }
                else
                {
                    options.UseSqlServer(connectionString, b =>
                    {
                        b.MigrationsAssembly(typeof(BlogDbContext).Assembly.FullName);
                        // Add following package to enable net topology suite for sql server:
                        // Microsoft.EntityFrameworkCore.SqlServer.NetTopologySuite
                        //b.UseNetTopologySuite();
                    });

                }
                if (environment.IsDevelopment())
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

            //    if (environment.IsDevelopment())
            //    {
            //        options.EnableSensitiveDataLogging();
            //    }
            //});
            #endregion

            services.AddScoped<IBlogDbContext>(provider =>
            provider.GetService<BlogDbContext>());

            services.AddScoped<IDbInitializerService, DbInitializerService>();
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IPostRepositoryAsync, PostRepositoryAsync>();
            #endregion

            return services;
        }
        public static async void AddBlogModuleDbInitializer(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                IDbInitializerService dbInitializer = scope.ServiceProvider.GetService<IDbInitializerService>();
                dbInitializer.Initialize();
                await dbInitializer.SeedData();
            }
        }

        private static IServiceCollection AddCustomUi(this IServiceCollection services, IWebHostEnvironment environment)
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
            services.AddControllers()

            //var razorPages = services.AddRazorPages()
            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

            //if (environment.IsDevelopment())
            //{
            //    controllerWithViews.AddRazorRuntimeCompilation();
            //    razorPages.AddRazorRuntimeCompilation();
            //}

            return services;
        }

        //private static IServiceCollection AddCustomSignalR(this IServiceCollection services)
        //{
        //    services.AddSignalR()
        //        .AddMessagePackProtocol();

        //    return services;
        //}


    }
}

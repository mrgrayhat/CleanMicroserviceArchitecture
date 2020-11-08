using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using SaeedRezayi.DataLayer.Context;
using SaeedRezayi.Services.Blog;
using SaeedRezayi.Services.Contracts.Blog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SaeedRezayi.ViewModels.Api;
using SaeedRezayi.ViewModels.Account;
using SaeedRezayi.Services.Account;
using SaeedRezayi.Services.Contracts.Account;
using SaeedRezayi.Services.Core;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using DNTCommon.Web.Core;
using Microsoft.Extensions.Options;
using SaeedRezayi.LogModule.Models;
using SaeedRezayi.LogModule.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SaeedRezayi.IoCConfig.Models;
using System.Linq;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.ResponseCaching;
using Newtonsoft.Json.Converters;

namespace SaeedRezayi.IoCConfig
{
    public static class ConfigureServicesExtensions
    {
        public static void AddDbInitializer(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializerService>();
                dbInitializer.Initialize();
                dbInitializer.SeedData();
            }

            Serilog.Log.Logger.Information("Database Initializer Configuration Succeeded.");
        }
        public static void UseExceptionHandler(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    if (error?.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response
                        .WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                        {
                            State = 401,
                            Msg = "token expired"
                        }));
                    }
                    else if (error?.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response
                        .WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                        {
                            State = 500,
                            Msg = error.Error.Message
                        }));
                    }
                    else
                    {
                        await next();
                    }
                });
            });
            Serilog.Log.Logger.Information("Custom Exception Configuration Succeeded.");
        }
        public static void AddCustomEndpoints(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Blog}/{action=Index}/{id?}");
            });

            Serilog.Log.Logger.Information("Api Endpoint setting's Configured.");
        }
        public static void AddCustomStaticFiles(this IApplicationBuilder applicationBuilder)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            applicationBuilder.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });
            Serilog.Log.Logger.Information("Static Files Configured.");
        }

        public static void UseCustomSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/SaeedRezayiApi/swagger.json", "Blog V1.0");
                setupAction.RoutePrefix = "";
                setupAction.DefaultModelExpandDepth(2);
                setupAction.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                setupAction.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                //setupAction.EnableDeepLinking();
                setupAction.DisplayOperationId();
            });
            Serilog.Log.Logger.Information("Swagger UI Configured.");
        }

        ///<summary>
        /// configure swagger service.
        ///
        /// setup swagger document generator and Open Api.
        /// configure api security requirements.
        /// add xml doc's written in project's code.
        /// (for project's which enabled xml output)
        ///</summary>
        public static void AddCustomSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(setupAction =>
                       {
                           setupAction.SwaggerDoc(
                                  name: "SaeedRezayiApi",
                                  info: new OpenApiInfo()
                                  {
                                      Title = "SaeedRezayi Blog V1.0 API",
                                      Version = "1.0",
                                      Description = "Through this API you can access blog and all it's content's.",
                                      Contact = new OpenApiContact()
                                      {
                                          Email = "saeedrezayi@saeedrezayi.ir",
                                          Name = "Saeed Rezayi",
                                          Url = new Uri("http://www.saeedrezayi.ir")
                                      },
                                      License = new OpenApiLicense()
                                      {
                                          Name = "MIT License",
                                          Url = new Uri("https://opensource.org/licenses/MIT")
                                      }
                                  });
                           setupAction.AddSecurityDefinition("Bearer",
                               new OpenApiSecurityScheme
                               {
                                   Description = Constants.SWAGGER_AUTH_MESSAGE,
                                   Name = "Authorization",
                                   In = ParameterLocation.Header,
                                   Type = SecuritySchemeType.ApiKey,
                                   Scheme = "Bearer",
                                   BearerFormat = "Bearer "
                               });
                           setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement{
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        //The name of the previously defined security scheme.
                                        Id = "Bearer",
                                        Type = ReferenceType.SecurityScheme
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                                    BearerFormat = "Bearer "
                                },
                                new List<string>()
                            }});
                           var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory,
                               "*.xml", SearchOption.TopDirectoryOnly).ToList();
                           xmlFiles.ForEach(xmlFile => setupAction.IncludeXmlComments(xmlFile));

                       });
            Serilog.Log.Logger.Information("Swagger Generator Configured.");
        }
        ///<summary>
        /// enumerate and return project's xml document
        ///</summary>
        private static IEnumerable<string> GetXmlDocumentFiles()
        {
            return Directory.EnumerateFiles(AppContext.BaseDirectory,
               "*.xml", SearchOption.TopDirectoryOnly);
        }
        ///<summary>
        /// configure mvc service.
        /// Add Usually Response Code's to Controller Actions
        /// (to avoid defination per every action).
        ///</summary>
        public static void AddCustomMvcSetup(this IServiceCollection services)
        {
            // used this because we are api (service base)
            services.AddControllers(setupAction =>
            {
                setupAction.Filters
                   .Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters
                 .Add(new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
                // config csrf validation for every [HTTP POST] Request's
                setupAction.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                setupAction.ReturnHttpNotAcceptable = true;
            })
                .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
                opt.SerializerSettings.Converters.Add(new StringEnumConverter());
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //opt.SerializerSettings.Formatting = Formatting.Indented;
                opt.UseMemberCasing();
            });
            Serilog.Log.Logger.Information("Asp.net Core Controller's and Json Serializer Setting's Configured.");
        }
        ///<summary>
        /// configure Csrf header and filter
        ///</summary>
        public static void AddCustomAntiforgery(this IServiceCollection services)
        {
            services.AddAntiforgery(x => x.HeaderName = "X-XSRF-TOKEN");
            // we configured this filter in AddCustomMvcSetup Before
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            //});
            Serilog.Log.Logger.Information("Anti Forgery Configured.");
        }
        ///<summary>
        /// configure Cross origin policy and allowed host setting's.
        ///</summary>
        public static void AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        //Note:  The URL must be specified without a trailing slash (/).
                        .WithOrigins(
                        "http://localhost:4200", // angular/client app
                        "http://127.0.0.1:4200", // test porpose
                        "http://localhost:5000") // test porpose
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials());
            });
            Serilog.Log.Logger.Information("Cors Configured.");
        }

        ///<summary>
        /// configure Json Web Token Auth Service.
        /// add default auth policies
        /// configure security validation's & handler's
        ///</summary>
        public static void AddCustomJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            // Only needed for custom roles.
            services.AddAuthorization(options =>
            {
                options.AddPolicy(CustomRolesViewModel.Admin,
                policy => policy.RequireRole(CustomRolesViewModel.Admin));
                options.AddPolicy(CustomRolesViewModel.User,
                policy => policy.RequireRole(CustomRolesViewModel.User));
                options.AddPolicy(CustomRolesViewModel.Editor,
                policy => policy.RequireRole(CustomRolesViewModel.Editor));
                options.AddPolicy(CustomRolesViewModel.Writer,
                policy => policy.RequireRole(CustomRolesViewModel.Writer));
            });

            // Needed for jwt auth.
            services
                .AddAuthentication(options =>
                {
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        // site that makes the token
                        ValidIssuer = configuration["BearerTokens:Issuer"],
                        // TODO: change this to avoid forwarding attacks
                        ValidateIssuer = false,
                        // site that consumes the token
                        ValidAudience = configuration["BearerTokens:Audience"],
                        // TODO: change this to avoid forwarding attacks
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(configuration["BearerTokens:Key"])),
                        // verify signature to avoid tampering
                        ValidateIssuerSigningKey = true,
                        // validate the expiration
                        ValidateLifetime = true,
                        // tolerance for the expiration date
                        ClockSkew = TimeSpan.Zero
                    };
                    cfg.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            logger.LogError(Constants.AUTH_FAILED, context.Exception);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var tokenValidatorService = context.HttpContext.RequestServices.GetRequiredService<ITokenValidatorService>();
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            logger.LogInformation(Constants.TOKEN_VALIDATED);
                            return tokenValidatorService.ValidateAsync(context);
                        },
                        OnMessageReceived = context =>
                        {
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                            logger.LogError(Constants.CHALLENGE_ERROR, context.Error, context.ErrorDescription);
                            return Task.CompletedTask;
                        }
                    };
                });
            Serilog.Log.Logger.Information("JWT System Configured.");
        }

        // database provider detector
        private static (string, DatabaseProvidersTypes) GetDatabaseProvider(this IConfiguration configuration)
        {
            switch (configuration.GetSection("DatabaseProvider").Value)
            {
                case "0":
                    return (configuration
                        .GetConnectionString("LocalDB")
                        .Replace("|DataDirectory|",
                        Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "app_data")), DatabaseProvidersTypes.LocalDB);
                case "1":
                    return (configuration.GetConnectionString("SqlServer"),
                    DatabaseProvidersTypes.SqlServer);
                case "2":
                    return (configuration.GetConnectionString("MySqlServer"),
                    DatabaseProvidersTypes.MySqlServer);
                case "3":
                    return (configuration.GetConnectionString("InMemory"),
                    DatabaseProvidersTypes.InMemory);
                case "4":
                    return (configuration.GetConnectionString("SqlLite"),
                    DatabaseProvidersTypes.SqlLite);
                case "5":
                    return (configuration.GetConnectionString("MongoDB"),
                    DatabaseProvidersTypes.MongoDB);
                default:
                    return (configuration.GetConnectionString("DefaultConnection"), DatabaseProvidersTypes.LocalDB);
            }
        }

        /// <summary>
        /// configure database context provider and connection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="isTest">if requested through test', config test db</param>
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration, bool isTest = false)
        {
            var dbProvider = GetDatabaseProvider(configuration);

            services.AddDbContext<ApplicationDbContext>(options =>
           {
               try
               {
                   // fot test layer 
                   if (isTest)
                   {
                       options.UseSqlServer(
                           configuration.GetConnectionString("SqlServerTest")
                           //configuration.GetConnectionString("LocalDBTest").Replace("|DataDirectory|", Path.Combine(Directory.GetCurrentDirectory()))
                           );
                       Serilog.Log.Logger.Information("Test Mode Database Configured.");
                       return;
                   }
                   // else
                   if (dbProvider.Item2 == DatabaseProvidersTypes.MySqlServer)
                   {
                       options.UseMySql(dbProvider.Item1, options =>
                       {
                           options.EnableRetryOnFailure();
                       });
                       Serilog.Log.Logger.Information("MySQL Database Configured.");
                   }
                   else if (dbProvider.Item2 == DatabaseProvidersTypes.InMemory)
                   {
                       options.UseInMemoryDatabase(dbProvider.Item1);
                       Serilog.Log.Logger.Information("In Memory Database Configured.");
                   }
                   else if (dbProvider.Item2 == DatabaseProvidersTypes.SqlLite)
                   {
                       options.UseSqlite(dbProvider.Item1);
                       //options.UseSqlite("Data Source=SaeedRezayiDatabase.db;");
                       Serilog.Log.Logger.Information("Sqlite Database Configured.");
                   }
                   else if (dbProvider.Item2 == DatabaseProvidersTypes.LocalDB)
                   {
                       options.UseSqlServer(dbProvider.Item1,
                           serverDbContextOptionsBuilder =>
                           {
                               var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                               serverDbContextOptionsBuilder.CommandTimeout(minutes);
                               serverDbContextOptionsBuilder.EnableRetryOnFailure();
                           });
                       //Serilog.Log.Logger.Information("LocalDB Database Configured.");
                   }
                   else if (dbProvider.Item2 == DatabaseProvidersTypes.SqlServer)
                   {
                       options.UseSqlServer(dbProvider.Item1,
                           serverDbContextOptionsBuilder =>
                           {
                               var minutes = (int)TimeSpan.FromMinutes(3).TotalSeconds;
                               serverDbContextOptionsBuilder.CommandTimeout(minutes);
                               serverDbContextOptionsBuilder.EnableRetryOnFailure();
                           });
                       Serilog.Log.Logger.Information("Sql Server Database Configured.");
                   }
                   else
                   {
                       Serilog.Log.Logger.Error(Constants.NO_DATABASE_PROVIDER);
                   }
               }
               catch (Exception ex)
               {
                   Serilog.Log.Logger.Error(ex, "Error In Database Provider Configuration");
               }
           });
            Serilog.Log.Logger.Information($"{dbProvider.Item2} Database Provider Configured.");
        }
        public static void AddLogModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LogDatabaseSettings>(configuration
                .GetSection(nameof(LogDatabaseSettings))); // getting Log database setting's from configuration(appsettings.json for example)

            services.AddSingleton<ILogDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<LogDatabaseSettings>>().Value); // singleton bcz no need to get setting's always, just when app initialized.
            
            services.AddSingleton<ILogService, LogService>(); // or scoped (singleton bcz mongodb singleton pattern)

            Console.WriteLine($"Log Module Service's Configured. using {configuration.GetSection(nameof(LogDatabaseSettings))["Provider"]} with {configuration.GetSection(nameof(LogDatabaseSettings))["ConnectionString"]} Connection For LogModule.");
        }
        public static void AddResponseOptimizations(this IServiceCollection services)
        {
            services.AddResponseCaching();
            services.Configure<ResponseCachingOptions>(options =>
            {
                // cache response up to 8kb (to reduce LOH Heap Size we set max to less than 8kb)
                options.MaximumBodySize = 7 * 1024;
            });

            //services.AddResponseCompression();
            // services.Configure<GzipCompressionProviderOptions>(options =>
            // {
            //     options.Level = CompressionLevel.Fastest;
            // });
            Serilog.Log.Logger.Information("Response Caching Configured.");
        }
        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAntiForgeryCookieService, AntiForgeryCookieService>();
            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IRolesService, RolesService>();
            services.AddSingleton<ISecurityService, SecurityService>();
            services.AddScoped<IDbInitializerService, DbInitializerService>();
            services.AddScoped<ITokenStoreService, TokenStoreService>();
            services.AddScoped<ITokenValidatorService, TokenValidatorService>();
            services.AddScoped<ITokenFactoryService, TokenFactoryService>();

            Serilog.Log.Logger.Information("Custom Service's Configured.");
        }
        public static void AddBlogServices(this IServiceCollection services)
        {
            services.AddScoped<IBlogService, BlogService>();

            Serilog.Log.Logger.Information("Blog Service's Configured.");
        }
        public static void AddDNTCommonOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpConfig>(options => configuration.GetSection("SmtpConfig").Bind(options));
            services.Configure<AntiDosConfig>(options => configuration.GetSection("AntiDosConfig").Bind(options));
            services.Configure<AntiXssConfig>(options => configuration.GetSection("AntiXssConfig").Bind(options));
            services.Configure<ContentSecurityPolicyConfig>(options => configuration.GetSection("ContentSecurityPolicyConfig").Bind(options));
            services.AddDNTCommonWeb();

            Serilog.Log.Logger.Information("Security middleware option's Configured.");
        }
        /// <summary>
        /// configure api setting and bearer token option's
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<BearerTokensOptionsViewModel>()
                .Bind(configuration.GetSection("BearerTokens"))
                .Validate(validation: bearerTokens =>
                {
                    return (bearerTokens.AccessTokenExpirationMinutes
                            < bearerTokens.RefreshTokenExpirationMinutes);
                }, failureMessage: Constants.TOKEN_FAILURE_MESSAGE);
            services.AddOptions<ApiSettingsViewModel>()
                .Bind(configuration.GetSection("ApiSettings"));

            Serilog.Log.Logger.Information("ApiSetting setting's Configured.");
        }

    }
}

using System;
using System.Linq;
using Gateway.Web.Api.Services;
using Gateway.Web.Api.Services.Contracts;
using Gateway.Web.Api.SignalR;
using HealthChecks.UI.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using NSwag;
using NSwag.Generation.Processors.Security;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Serilog;

namespace Gateway.Web.Api
{
    public class Startup
    {
        private IWebHostEnvironment HostingEnvironment { get; }
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            HostingEnvironment = env;
            Configuration = configuration;
            Serilog.Debugging.SelfLog.Enable((log) =>
            {
                Log.Fatal(log);
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            #region Local Culture
            //services.AddLocalization(options => options.ResourcesPath = "Resources");
            //services.Configure<RequestLocalizationOptions>(
            //    options =>
            //    {
            //        var supportedCultures = new List<CultureInfo>
            //        {
            //            new CultureInfo("en-GB"),
            //            new CultureInfo("en-US"),
            //            new CultureInfo("fa-IR"),
            //            new CultureInfo("ar-SA"),
            //            new CultureInfo("fr-FR")
            //        };

            //        options.DefaultRequestCulture = new RequestCulture(
            //            culture: "en-US", uiCulture: "en-US");
            //        options.SupportedCultures = supportedCultures;
            //        options.SupportedUICultures = supportedCultures;
            //    });
            #endregion
            services.AddTransient<IApplicationService, ApplicationService>();
            services.AddSingleton<IDeploymentEnvironment, DeploymentEnvironment>();

            services.AddHttpContextAccessor()
                    .AddResponseCompression()
                    .AddMemoryCache();
            var healthChecks = services.AddHealthChecks();

            healthChecks.AddCheck("self", () => HealthCheckResult.Healthy());
            if (Configuration.GetValue<bool>("HealthChecks-UI:MicroservicesUptimeMonitor"))
            {
                healthChecks.AddUrlGroup(new Uri("http://localhost:5003/api/v1/storage"), "Storage service state", HealthStatus.Unhealthy, new[] { "Storage", "Uptime" });

                healthChecks.AddUrlGroup(new Uri("http://localhost:5007/"), "Identity server service state", HealthStatus.Unhealthy, new[] { "STS", "Uptime" });
                
                healthChecks.AddUrlGroup(new Uri("http://localhost:4949/api/v1/storage"), "Logger service state", HealthStatus.Unhealthy, new[] { "Logger", "Uptime" });

            }

            services.AddHealthChecksUI()
                .AddSqliteStorage(Configuration["Data:HealthCheckDb"]);

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Gateway API";
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

            services.AddSignalR();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication("Bearer", options =>
                {
                    options.Authority = Configuration["Auth:Authority"];
                    options.RequireHttpsMetadata = true;
                    options.ApiName = "https://localhost:5005";
                    options.SupportedTokens = SupportedTokens.Both;
                });
            //.AddJwtBearer(options =>
            //{
            //    // base-address of your identity server
            //    options.Authority = Configuration["Auth:Authority"];
            //    // name of the API resource
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidAudiences = Configuration.GetSection("Auth:Audiences")
            //        .Get<List<string>>(),
            //    };
            //});

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist/Gateway.Web.Api";
            });
            // configure ocelot gateway and QOS using polly
            services.AddOcelot(Configuration)
                    .AddPolly();
        }

        public async void Configure(IApplicationBuilder app)
        {
            if (!HostingEnvironment.IsDevelopment())
                app.UseSpaStaticFiles();
            else
                app.UseDeveloperExceptionPage();

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/wwwroot/api";
                settings.DocumentPath = "/wwwroot/api/specification.json";
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors(Constants.DefaultCorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc",
                    new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                        ResultStatusCodes =
                        {
                            [HealthStatus.Healthy] = StatusCodes.Status200OK,
                            [HealthStatus.Degraded] = StatusCodes.Status200OK,
                            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                        }
                    });
                endpoints.MapHealthChecksUI(config =>
                {
                    config.UIPath = "/hc-ui";
                    config.AddCustomStylesheet("wwwroot/custom-health-ui.css");
                });
                endpoints.MapDefaultControllerRoute();
                endpoints.MapHub<Notification>("/notificationHub");
            });
            await app.UseOcelot();

            #region proxy spa app
            //app.UseSpa(spa =>
            //{
            //    spa.Options.SourcePath = "ClientApp";

            //    /*
            //    // If you want to enable server-side rendering (SSR),
            //    // [1] In Gateway.Web.Api.csproj, change the <BuildServerSideRenderer> property
            //    //     value to 'true', so that the SSR bundle is built during publish
            //    // [2] Uncomment this code block
            //    */

            //    //   spa.UseSpaPrerendering(options =>
            //    //    {
            //    //        options.BootModulePath = $"{spa.Options.SourcePath}/dist-server/main.bundle.js";
            //    //        options.BootModuleBuilder = env.IsDevelopment() ? new AngularCliBuilder(npmScript: "build:ssr") : null;
            //    //        options.ExcludeUrls = new[] { "/sockjs-node" };
            //    //        options.SupplyData = (requestContext, obj) =>
            //    //        {
            //    //          //  var result = appService.GetApplicationData(requestContext).GetAwaiter().GetResult();
            //    //          obj.Add("Cookies", requestContext.Request.Cookies);
            //    //        };
            //    //    });

            //    if (HostingEnvironment.IsDevelopment())
            //    {
            //        //spa.UseAngularCliServer(npmScript: "start");
            //        //   OR
            //        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
            //    }
            //});
            #endregion
        }
    }
}

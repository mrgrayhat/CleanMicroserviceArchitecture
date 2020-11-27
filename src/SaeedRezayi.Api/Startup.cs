using System;
using System.Diagnostics;
using System.Runtime;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SaeedRezayi.IoCConfig;
using SaeedRezayi.IoCConfig.Middlewares;

namespace SaeedRezayi.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ResetColor();
            });

            #region Gc Configurations
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"LatencyMode: {GCSettings.LatencyMode}");
            Console.WriteLine($"IsServerGc: {GCSettings.IsServerGC}");
            Console.WriteLine($"LargeObjectHeapCompactionMode: {GCSettings.LargeObjectHeapCompactionMode}");
            Console.ResetColor();
            #endregion
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add Services From Inversion Of Control Layer
            // configure log module service
            services.AddLogModule(Configuration);
            // configure ApiSettings and custom options
            services.AddCustomOptions(Configuration);
            // config security options
            services.AddDNTCommonOptions(Configuration);
            // add core system services
            services.AddCustomServices();
            // configure response caching and compression
            services.AddResponseOptimizations();
            // configure blog system services
            services.AddBlogServices();
            // configure database provider
            services.AddCustomDbContext(Configuration);
            // configure auth system and jwt security
            services.AddCustomJwtBearer(Configuration);
            // configure Cors Policy
            services.AddCustomCors();
            // configure csrf
            services.AddCustomAntiforgery();
            // Add Mvc Action Filter's
            services.AddCustomMvcSetup();
            // configure swagger, OpenApi SecurityDefinition's & Api Doc's
            services.AddCustomSwagger();
            #endregion
        }

        // This method gets called by the runtime.
        //Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO: Remove Custom GC 
            // app.Use(async (context, next) =>
            //     {
            //         await next();
            //         GC.Collect(2, GCCollectionMode.Forced, true);
            //         GC.WaitForPendingFinalizers();
            //     });

            app.AddDbInitializer();

            //app.UseRequestTrackerMiddleware();
            //app.UseSerilogRequestLogging();

            app.UseMiddleware<SerilogRequestLoggerMiddleware>();


            if (!env.IsDevelopment())
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            // TODO: Log Http 401 errors (incorrect login)
            app.UseExceptionHandlerMiddleware();

            //app.UseAntiDos(); // ensure AddDNTCommonOptions service was called before

            app.UseContentSecurityPolicy(); // must add before use static files
            // configure swagger ui options & SwaggerEndpoint's
            app.UseCustomSwaggerUI();

            app.UseStatusCodePages();

            app.AddCustomStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            // UseCors must define in middle of UseRouting() and UseEndpoints
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.AddCustomEndpoints();

            // catch-all handler for HTML5 client routes - serve index.html
            // app.Run(async context =>
            // {
            //     context.Response.ContentType = "text/html";
            //     await context.Response.SendFileAsync(Path.Combine(env.WebRootPath, "index.html"));
            // });

        }
    }
}

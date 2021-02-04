using System;
using System.Diagnostics;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace LogModule.Application
{
    public static class ServiceExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging();

            return app;
        }
        public static IMvcBuilder AddLogModuleController(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddApplicationPart(Assembly.GetAssembly(typeof(LogModule.Application.Controllers.LogController)));

            return mvcBuilder;
        }
        public static IServiceCollection AddLogModuleApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // auto performance behavior detection
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));

            // to do atomic trnsaction's and rollback if any problem occured
            //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            Serilog.Debugging.SelfLog.Enable(msg =>
            {
                Debug.Print(msg);
                Debugger.Break();

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ResetColor();
            });

            return services;
        }
        public static IHostBuilder AddLogModuleToHostBuilder(this IHostBuilder builder, IConfiguration configuration)
        {
            //Log.Logger = new LoggerConfiguration()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();

            builder.UseSerilog((hostingContext, loggerConfig) =>
            {
                loggerConfig.ReadFrom
                .Configuration(configuration);
            });

            return builder;
        }
    }
}

using System.Reflection;
using AutoMapper;
using BlogModule.Application.Abstractions;
using BlogModule.Application.Behaviours;
using BlogModule.Application.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BlogModule.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddBlogModuleApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient<IApplicationService, ApplicationService>();

            // to validate cqrs validations
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // auto performance behavior detection
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            // to do atomic trnsaction's and rollback if any problem occured
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            return services;
        }
    }
}

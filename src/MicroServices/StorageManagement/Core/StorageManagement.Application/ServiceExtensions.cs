using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using StorageManagement.Application.Behaviours;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Services;

namespace StorageManagement.Application
{
    public static class ServiceExtensions
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // to validate cqrs validations
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // auto performance behavior detection
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehavior<,>));
            // to do atomic trnsaction's and rollback if any problem occured
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

            services.AddScoped<IStorageFileSystemProvider, StorageFileSystemProvider>();
            services.AddScoped<IStorageAgent, StorageAgent>();

            return services;
        }
    }
}

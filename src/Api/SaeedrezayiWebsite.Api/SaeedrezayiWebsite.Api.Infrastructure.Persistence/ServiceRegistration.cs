using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SaeedrezayiWebsite.Api.Application.Interfaces;
using SaeedrezayiWebsite.Api.Application.Interfaces.Repositories;
using SaeedrezayiWebsite.Api.Infrastructure.Persistence.Contexts;
using SaeedrezayiWebsite.Api.Infrastructure.Persistence.Repositories;
using SaeedrezayiWebsite.Api.Infrastructure.Persistence.Repository;

namespace SaeedrezayiWebsite.Api.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "app_data"));

                string con = configuration.GetConnectionString("DefaultConnection").Replace("|DataDirectory|", Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot", "app_data"));

                services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(con,
                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }
            #region Repositories
            services.AddTransient(typeof(IGenericRepositoryAsync<>), typeof(GenericRepositoryAsync<>));
            services.AddTransient<IProductRepositoryAsync, ProductRepositoryAsync>();
            #endregion
        }
    }
}

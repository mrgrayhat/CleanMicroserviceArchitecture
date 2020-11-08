using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SaeedRezayi.DataLayer.Context;
using SaeedRezayi.Services.Core;

namespace SaeedRezayi.XuntiTestLayer.Base
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        /// <summary>
        /// amount of extra data to seed in db
        /// </summary>

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(async services =>
            {
                var descriptor = services.SingleOrDefault(d => d
                .ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
                    var dbInitializer = scopedServices
                        .GetRequiredService<IDbInitializerService>();
                    db.Database.EnsureCreated();

                    try
                    {
                        await dbInitializer.SeedData(); // init default data
                        await dbInitializer.SeedExtraData(Constants.ExtraRecordsAmount); // append extra data
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                } // end of injected services scope
            }); // end of config host services
        }
    }
}

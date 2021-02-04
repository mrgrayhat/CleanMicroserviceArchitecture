using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogModule.Domain.Entities;
using BlogModule.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlogModule.Infrastructure.Seeds
{
    public interface IDbInitializerService
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize(bool isTest = false);

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        Task SeedData();
    }


    public class DbInitializerService : IDbInitializerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        //private readonly ISecurityService _securityService;
        private readonly ILogger<DbInitializerService> _logger;

        public DbInitializerService(
            IServiceScopeFactory scopeFactory, ILogger<DbInitializerService> logger)
        {
            _logger = logger;
            //_logger.CheckArgumentIsNull(nameof(logger));

            _scopeFactory = scopeFactory;
            //_scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

            //_securityService = securityService;
            //_securityService.CheckArgumentIsNull(nameof(_securityService));
        }

        public void Initialize(bool isTest = false)
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<BlogDbContext>();
            if (isTest)
            {
                context.Database.EnsureCreated();
            }
            else
            {
                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }
            }

        }

        public async Task SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<BlogDbContext>())
                {
                    if (!context.Cultures.Any())
                    {
                        var cultureEnglish = new Culture
                        {
                            Code = "en-us",
                            DisplayName = "English-UnitedState"
                        };
                        var cultureFarsi = new Culture
                        {
                            Code = "fa-ir",
                            DisplayName = "Farsi-Iran"
                        };
                        var cultureArabic = new Culture
                        {
                            Code = "ar-sa",
                            DisplayName = "Arabic"
                        };

                        await context.AddRangeAsync(cultureEnglish, cultureFarsi, cultureArabic);
                        await context.SaveChangesAsync();
                    }

                    if (!context.Posts.Any())
                    {
                        var post = new Post
                        {
                            Locales = new List<PostLocale>()
                            {
                                new PostLocale
                                {
                                    Title = "Default Post Title",
                                    Content = "Default Post Body",
                                    Slug = "default-post-slug",
                                    CultureId = context.Cultures
                                    //.AsNoTracking()
                                    .FirstOrDefault(x=>x.Code == "en-us").Id
                                },
                                new PostLocale
                                {
                                    Title = "عنوان مطلب پیشفرض",
                                    Content = "محتوای مطلب پیشفرض",
                                    Slug = "اسلاگ-مطلب-پیشفرض",
                                    CultureId =  context.Cultures
                                    //.AsNoTracking()
                                    .FirstOrDefault(x=>x.Code == "fa-ir").Id
                                },
                                new PostLocale
                                {
                                    Title = "العنوان",
                                    Content = "المحتوی",
                                    Slug = "المطلب-اسلاگ",
                                    CultureId = context.Cultures
                                    //.AsNoTracking()
                                    .FirstOrDefault(x=>x.Code == "ar-sa").Id
                                }
                            },
                            Tags = "tag1;tag2;تگ آزمایشی;",
                            Description = "Default Post Description",
                            Created = DateTime.UtcNow,
                            IsArchive = false,
                            IsPublic = true,
                            Visits = 1
                        };


                        await context.AddAsync(post);
                        await context.SaveChangesAsync();
                    }
                    //await context.SeedDefaultCulturesAsync();
                    //await context.SeedDefaultPostsAsync();
                }
            }
        }

    }
}

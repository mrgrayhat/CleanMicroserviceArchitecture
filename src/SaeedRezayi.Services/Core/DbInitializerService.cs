using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SaeedRezayi.Common;
using SaeedRezayi.DataLayer.Context;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.DomainClasses.Authentication.JoiningTables;
using SaeedRezayi.DomainClasses.Blog.JoiningTables;
using SaeedRezayi.DomainClasses.Blog.Posts;
using SaeedRezayi.DomainClasses.Blog.Posts.Locales;
using SaeedRezayi.DomainClasses.Common;
using SaeedRezayi.LogModule.Utilities;
using SaeedRezayi.Services.Contracts.Account;
using SaeedRezayi.ViewModels.Account;

namespace SaeedRezayi.Services.Core
{
    public class DbInitializerService : IDbInitializerService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISecurityService _securityService;
        private readonly ILogger<DbInitializerService> _logger;

        public DbInitializerService(
            IServiceScopeFactory scopeFactory,
            ISecurityService securityService, ILogger<DbInitializerService> logger)
        {
            _logger = logger;
            _logger.CheckArgumentIsNull(nameof(logger));

            _scopeFactory = scopeFactory;
            _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));
        }

        public void Initialize(bool isTest = false)
        {
            using var serviceScope = _scopeFactory.CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            if (isTest)
            {
                context.Database.EnsureDeleted();
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
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    if (!await context.Cultures.AnyAsync())
                    {
                        IEnumerable<CultureInfo> cultures = new List<CultureInfo>(3)
                        {
                            new CultureInfo
                            {
                                //Id=1,
                                Code = "fa-ir",
                                DisplayName = "Persian"
                            },
                           new CultureInfo
                           {
                                //Id=2,
                                Code = "en-us",
                                DisplayName = "English"
                           },
                           new CultureInfo
                           {
                                //Id=3,
                                Code = "ar-sa",
                                DisplayName = "Arabic"
                           }
                        };
                        await context.AddRangeAsync(cultures);
                        await context.SaveChangesAsync();
                    }
                    // AddAsync default roles
                    var adminRole = new RoleInfo { Name = CustomRolesViewModel.Admin };
                    var userRole = new RoleInfo { Name = CustomRolesViewModel.User };
                    var writerRole = new RoleInfo { Name = CustomRolesViewModel.Writer };

                    if (!await context.Roles.AnyAsync())
                    {
                        await context.AddAsync(adminRole);
                        await context.AddAsync(userRole);
                        await context.AddAsync(writerRole);

                        await context.SaveChangesAsync();
                    }

                    // AddAsync Admin user
                    if (!await context.Users.AnyAsync())
                    {
                        //IFormatProvider provider = new System.Globalization.CultureInfo("fa-ir", true);
                        UserInfo adminUser = new UserInfo
                        {
                            Username = "admin",
                            DisplayName = "ادمین",
                            IsActive = true,
                            LastLoggedIn = null,
                            Password = _securityService.GetSha256Hash("admin"),
                            SerialNumber = Guid.NewGuid().ToString("N"),
                            EmailAddress = "admin@saeedrezayi.ir",
                            Country = "Iran",
                            Culture = await context.Cultures
                            .FirstOrDefaultAsync(x => x.Code == "fa-ir"),
                            Birthday = DateTime.Parse("1375/10/10").ToShortDateString(),
                            Bio = "I'm administrator of this website.",
                            WebsiteUrl = "mywebsite.com"
                        };
                        UserInfo writerUser = new UserInfo
                        {
                            Username = "writer1",
                            DisplayName = "Writer 1",
                            IsActive = true,
                            LastLoggedIn = null,
                            EmailAddress = "writer1@saeedrezayi.ir",
                            Country = "United States",
                            CultureId = 2,
                            //Culture = await context.Cultures
                            //.FirstOrDefaultAsync(x => x.Code == "en-us"),
                            Password = _securityService.GetSha256Hash("writer"),
                            SerialNumber = Guid.NewGuid().ToString("N"),
                            Birthday = DateTime.Parse("2001/09/11").ToShortDateString(),
                            Bio = "I'm a write.",
                            WebsiteUrl = "mywebsite.com"
                        };
                        UserInfo standardUser = new UserInfo
                        {
                            Username = "user",
                            DisplayName = "مستخدم عادي",
                            EmailAddress = "user@saeedrezayi.ir",
                            IsActive = true,
                            LastLoggedIn = null,
                            Country = "Saudi Arabia",
                            Culture = await context.Cultures
                            .FirstOrDefaultAsync(x => x.Code == "ar-sa"),
                            Password = _securityService.GetSha256Hash("user"),
                            SerialNumber = Guid.NewGuid().ToString("N"),
                            Birthday = DateTime.Parse("2000/11/12").ToShortDateString(),
                            Bio = "أنا الكاتب",
                            WebsiteUrl = "mywebsite.com"
                        };

                        await context.AddAsync(adminUser);
                        await context.AddAsync(writerUser);
                        await context.AddAsync(standardUser);
                        await context.SaveChangesAsync();

                        // add admin user and assign role
                        await context.AddAsync(
                            new UserRole
                            {
                                Role = adminRole,
                                User = adminUser
                            });
                        await context.AddAsync(new UserRole
                        {
                            Role = userRole,
                            User = adminUser
                        });
                        // add normal user and assign role
                        await context
                            .AddAsync(new UserRole
                            {
                                Role = userRole,
                                User = standardUser
                            });
                        // add writer user and assign role
                        await context.AddAsync(new UserRole
                        {
                            Role = userRole,
                            User = writerUser
                        });
                        await context.AddAsync(new UserRole
                        {
                            Role = writerRole,
                            User = writerUser
                        });

                        await context.SaveChangesAsync();
                    }
                    if (!await context.UserFriends.AnyAsync())
                    {
                        await context.AddAsync(new UserFriendsInfo
                        {
                            User1 = await context.Users.FirstOrDefaultAsync(x => x.Id == 3),
                            User2 = await context.Users.FirstOrDefaultAsync(x => x.Id == 2)
                        });
                        await context.SaveChangesAsync();
                    }
                    if (!await context.Keywords.AnyAsync())
                    {
                        var defaultKeywords = new KeywordInfo
                        {
                            //Id = 1,
                            Title = "blog"
                        };

                        await context.AddAsync(defaultKeywords);
                        await context.SaveChangesAsync();
                    }
                    if (!await context.Tags.AnyAsync())
                    {
                        var defaultTag = new TagInfo
                        {
                            //Id = 1,
                            Title = "عمومی",
                            Visits = 10
                        };
                        await context.AddAsync(defaultTag);
                        await context.SaveChangesAsync();
                    }
                    if (!await context.Categories.AnyAsync())
                    {
                        var defaultCategory = new CategoryInfo
                        {
                            //Id = 1,
                            Title = "عمومی",
                            Description = "default category"
                        };
                        await context.AddAsync(defaultCategory);
                        await context.SaveChangesAsync();
                    }
                    if (!await context.Attachments.AnyAsync())
                    {
                        var defaultAttachment = new AttachmentInfo
                        {
                            Id = Guid.Parse("60bdecef-4342-4c60-aed9-e2d2ca4be752"),
                            Name = "myPhoto150150.png",
                            TotalDownloads = 1,
                            Size = 90434,
                            Url = "60bdecef-4342-4c60-aed9-e2d2ca4be752",
                            User = await context.Users
                            .FirstOrDefaultAsync(x => x.Username == "admin")
                        };

                        await context.AddAsync(defaultAttachment);
                        await context.SaveChangesAsync();
                    }
                    int post1Id = 0;
                    //int post2Id = 0;
                    if (!context.Posts.Any())
                    {
                        PostInfo defaultPost = new PostInfo
                        {
                            Author = await context.Users
                            .FirstOrDefaultAsync(x => x.Username == "admin"),
                            IsPublic = true,
                            IsArchive = false,
                            Category = await context.Categories
                            .FirstOrDefaultAsync(x => x.Title == "عمومی"),
                            Visits = 10
                        };
                        //PostInfo defaultPost2 = new PostInfo
                        //{
                        //    Author = await context.Users
                        //    .FirstOrDefaultAsync(x => x.Username == "admin"),
                        //    IsPublic = true,
                        //    IsArchive = false,
                        //    Category = await context.Categories
                        //    .FirstOrDefaultAsync(x => x.Title == "عمومی"),
                        //    Visits = 10
                        //};
                        var post1 = await context.AddAsync(defaultPost);
                        //var post2 = await context.AddAsync(defaultPost2);
                        await context.SaveChangesAsync();

                        post1Id = post1.Entity.Id;
                        //post2Id = post2.Entity.Id;

                        PostLocaleInfo post2LocalArabic = new PostLocaleInfo
                        {
                            Title = "السلام-دنیا",
                            Slug = "السلام-دنیا",
                            Content = "هذا مطلب الاول ",
                            Post = await context.Posts
                            .FirstOrDefaultAsync(x => x.Id == post1Id),
                            //PostId = post2Id,
                            LocalCulture = await context.Cultures
                            .FirstOrDefaultAsync(x => x.Code == "ar-sa")
                        };
                        PostLocaleInfo postLocalEn = new PostLocaleInfo
                        {
                            Title = "Hello World",
                            Slug = "hello-world",
                            Content = "This is my first post in this blog!",
                            Post = await context.Posts
                            .FirstOrDefaultAsync(x => x.Id == post1Id),
                            LocalCulture = await context.Cultures
                            .FirstOrDefaultAsync(x => x.Code == "en-us")
                        };
                        PostLocaleInfo postLocalFa = new PostLocaleInfo
                        {
                            Title = "سلام-دنیا",
                            Slug = "سلام-دنیا",
                            Content = "این اولین مطلب من در این وبلاگ هست!",
                            Post = await context.Posts
                            .FirstOrDefaultAsync(x => x.Id == post1Id),
                            LocalCulture = await context.Cultures
                            .FirstOrDefaultAsync(x => x.Code == "fa-ir")
                        };
                        await context.AddRangeAsync(postLocalFa, postLocalEn, post2LocalArabic);
                        await context.SaveChangesAsync();
                    }

                    if (!await context.PostTags.AnyAsync())
                    {
                        await context.AddAsync(new PostTag
                        {
                            Post = await context.Posts
                            .FirstOrDefaultAsync(x => x.Id == post1Id),
                            Tag = await context.Tags
                            .FirstOrDefaultAsync(x => x.Title == "عمومی")
                        });

                        await context.SaveChangesAsync();
                    }
                    if (!await context.PostAttachments.AnyAsync())
                    {
                        await context.AddAsync(new PostAttachment
                        {
                            Post = await context.Posts
                            .FirstOrDefaultAsync(x => x.Id == post1Id),
                            Attachment = await context.Attachments
                            .FirstOrDefaultAsync(gid => gid.Id == Guid
                            .Parse("60bdecef-4342-4c60-aed9-e2d2ca4be752"))
                        });

                        await context.SaveChangesAsync();
                    }
                    if (!await context.PostKeywords.AnyAsync())
                    {
                        await context.AddAsync(new PostKeyword
                        {
                            Keyword = await context.Keywords
                            .FirstOrDefaultAsync(x => x.Title == "blog"),
                            Post = await context.Posts
                            .FirstOrDefaultAsync(x => x.Id == post1Id)
                        });
                        await context.SaveChangesAsync();
                    }

                }
            }
        }

        /// <summary>
        /// seed extra data to blog tables, 
        /// such as extra test post's, tag's, categories, attachment's
        /// </summary>
        /// <param name="amount">amout of extra data. default is 10 item</param>
        /// <returns></returns>
        public async Task SeedExtraData(int amount = 10)
        {
            for (int i = 0; i < amount; i++)
            {
                using IServiceScope serviceScope = _scopeFactory.CreateScope();
                using ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                int rnd = new Random((int)DateTime.Now.Ticks).Next(1_000, 10_000_000);
                var guid = Guid.NewGuid();

                var keyword = new KeywordInfo
                {
                    //Id = 1,
                    Title = $"keyword {rnd}"
                };

                await context.AddAsync(keyword);
                await context.SaveChangesAsync();

                var tag = new TagInfo
                {
                    //Id = 1,
                    Title = $"tag {rnd}",
                    Visits = rnd
                };
                await context.AddAsync(tag);
                await context.SaveChangesAsync();

                var category = new CategoryInfo
                {
                    //Id = 1,
                    Title = $"category {rnd}",
                    Description = $"category {rnd}"
                };
                await context.AddAsync(category);
                await context.SaveChangesAsync();

                var attachment = new AttachmentInfo
                {
                    Id = guid,
                    Name = $"photo{rnd}",
                    TotalDownloads = rnd,
                    Size = rnd,
                    Url = $"phoho{rnd}.png",
                    User = context.Users
                    .FirstOrDefault(x => x.Username == "admin")
                };
                await context.AddAsync(attachment);
                await context.SaveChangesAsync();

                var post = new PostInfo
                {
                    Id = rnd,
                    //PostLocale = new PostInfoLocale
                    //{
                    //    Title = $"Post {rnd}",
                    //    Slug = $"post-{rnd}",
                    //    Content = $"Test Post {rnd}"
                    //},
                    Author = context.Users
                    .FirstOrDefault(x => x.Username == "admin"),
                    IsPublic = true,
                    IsArchive = false,
                    Thumbnail = "",
                    Category = context.Categories
                    .FirstOrDefault(x => x.Title == $"category {rnd}"),
                    Visits = rnd
                };
                await context.AddAsync(post);
                await context.SaveChangesAsync();

                PostLocaleInfo postLocalEn = new PostLocaleInfo
                {
                    Title = $"Post {rnd}",
                    Slug = $"post-{rnd}",
                    Content = $"Test Post {rnd}",
                    Post = await context.Posts
                    .FirstOrDefaultAsync(x => x.Id == rnd),
                    LocalCulture = await context.Cultures
                    .FirstOrDefaultAsync(x => x.Code == "en-us")
                };
                PostLocaleInfo postLocalFa = new PostLocaleInfo
                {
                    Title = $"مطلب {rnd}",
                    Slug = $"مطلب-{rnd}",
                    Content = $"مطلب آزمایشی {rnd}",
                    Post = await context.Posts
                    .FirstOrDefaultAsync(x => x.Id == rnd),
                    LocalCulture = await context.Cultures
                    .FirstOrDefaultAsync(x => x.Code == "fa-ir")
                };

                await context.AddAsync(postLocalEn);
                await context.AddAsync(postLocalFa);
                await context.SaveChangesAsync();

                await context.AddAsync(new PostTag
                {
                    Post = context.Posts
                    //.FirstOrDefault(x => x.Locales.Title == $"Post {rnd}"),
                    .FirstOrDefault(x => x.Id == rnd),
                    Tag = context.Tags
                    .FirstOrDefault(x => x.Title == $"tag {rnd}")
                });
                await context.SaveChangesAsync();

                await context.AddAsync(new PostAttachment
                {
                    Post = context.Posts
                    //.FirstOrDefault(x => x.Locales.Title == $"Post {rnd}"),
                    .FirstOrDefault(x => x.Id == rnd),
                    Attachment = context.Attachments
                    .FirstOrDefault(gid => gid.Id == guid)
                });
                await context.SaveChangesAsync();

                await context.AddAsync(new PostKeyword
                {
                    Keyword = context.Keywords
                    .FirstOrDefault(x => x.Title == $"keyword {rnd}"),
                    Post = context.Posts
                    //.FirstOrDefault(x => x.Locales.Title == $"Post {rnd}")
                    .FirstOrDefault(x => x.Id == rnd)
                });
                await context.SaveChangesAsync();
            } //end of for(loop)
        }
        public void Dispose()
        {
            //RuntimeExtensions.GetGCInfo(_logger);
            _logger.GetGCInfo();

            GC.Collect();
            GC.WaitForFullGCComplete();
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced, true, true);

            _logger.GetGCInfo();
            _logger.LogInformation($"Db Initializer Service Disposed");
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogModule.Domain.Entities;
using BlogModule.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BlogModule.Infrastructure.Seeds
{
    public static class DefaultPosts
    {
        public static async Task SeedDefaultPostsAsync(this BlogDbContext blogDbContext)
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
                        LocalCulture = blogDbContext.Cultures
                        .AsNoTracking()
                        .FirstOrDefault(x=>x.Code == "en-us")
                    },
                    new PostLocale
                    {
                        Title = "عنوان مطلب پیشفرض",
                        Content = "محتوای مطلب پیشفرض",
                        Slug = "اسلاگ-مطلب-پیشفرض",
                        LocalCulture =  blogDbContext.Cultures
                        .AsNoTracking()
                        .FirstOrDefault(x=>x.Code == "fa-ir")
                    },
                    new PostLocale
                    {
                        Title = "العنوان",
                        Content = "المحتوی",
                        Slug = "المطلب-اسلاگ",
                        LocalCulture = blogDbContext.Cultures
                        .AsNoTracking()
                        .FirstOrDefault(x=>x.Code == "ar-sa")
                    }
                },
                Tags = "tag1;tag2;تگ آزمایشی;",
                Description = "Default Post Description",
                Created = DateTime.UtcNow,
                IsArchive = false,
                IsPublic = true,
                Visits = 1
            };

            if (!blogDbContext.Posts.Any())
            {
                await blogDbContext.AddAsync(post);
                await blogDbContext.SaveChangesAsync();
            }
        }
    }
}

using BlogModule.Application.Abstractions;
using BlogModule.Domain.Localization;
using Microsoft.EntityFrameworkCore;

namespace Blog.Localization.Infrastructure.Localization
{
    public class LocalizationDbContext : DbContext, ILocalizationDbContext
    {
        public LocalizationDbContext(DbContextOptions<LocalizationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Resource> Resources { get; set; }
    }
}

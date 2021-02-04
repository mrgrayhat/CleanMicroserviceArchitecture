using STS.Application.Abstractions;
using STS.Domain.Entities.Localization;
using Microsoft.EntityFrameworkCore;

namespace STS.Infrastructure.Localization
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

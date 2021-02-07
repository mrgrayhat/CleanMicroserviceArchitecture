using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageManagement.Application.Abstractions;
using StorageManagement.Domain.Entities;
using StorageManagement.Infrastructure.Configurations;

namespace StorageManagement.Infrastructure.Contexts
{
    public class StorageDbContext : DbContext, IStorageDbContext
    {
        //private readonly ICurrentUserService _currentUserService;
        //private readonly IDateTime _dateTime;

        //private readonly IDateTimeService _dateTime;
        //private readonly IAuthenticatedUserService _authenticatedUser;

        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
        {
        }
        //public BlogDbContext(DbContextOptions<BlogDbContext> options, ICurrentUserService currentUserService, IDateTime dateTime)
        //   : base(options)
        //{
        //    ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        //    //_currentUserService = currentUserService;
        //    //_dateTime = dateTime;
        //}


        public DbSet<Item> Items { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            /* 
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
             {
                 switch (entry.State)
                 {
                     case EntityState.Added:
                         entry.Entity.CreatedBy = _currentUserService.UserId.ToString();
                         entry.Entity.Created = _dateTime.Now;
                         break;
                     case EntityState.Modified:
                         entry.Entity.LastModifiedBy = _currentUserService.UserId.ToString();
                         entry.Entity.LastModified = _dateTime.Now;
                         break;
                 }
             }
             */
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddCustomMappings();
        }
    }
}

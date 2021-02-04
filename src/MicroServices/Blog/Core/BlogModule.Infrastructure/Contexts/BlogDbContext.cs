using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blog.Common;
using BlogModule.Application.Abstractions;
using BlogModule.Application.Interfaces;
using BlogModule.Domain.Common;
using BlogModule.Domain.Entities;
using BlogModule.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BlogModule.Infrastructure.Contexts
{
    public class BlogDbContext : DbContext, IBlogDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;

        //private readonly IDateTimeService _dateTime;
        //private readonly IAuthenticatedUserService _authenticatedUser;

        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }
        //public BlogDbContext(DbContextOptions<BlogDbContext> options, ICurrentUserService currentUserService, IDateTime dateTime)
        //   : base(options)
        //{
        //    ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        //    //_currentUserService = currentUserService;
        //    //_dateTime = dateTime;
        //}


        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLocale> PostLocales { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<Attachment> Attachments { get; set; }

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
            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }

            base.OnModelCreating(builder);

            builder.AddCustomMappings();
        }
    }
}

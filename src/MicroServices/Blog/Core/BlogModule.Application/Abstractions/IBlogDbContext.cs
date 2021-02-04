using System.Threading;
using System.Threading.Tasks;
using BlogModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BlogModule.Application.Abstractions
{
    public interface IBlogDbContext
    {

        DbSet<Culture> Cultures { get; set; }
        DbSet<Post> Posts { get; set; }
        DbSet<PostLocale> PostLocales { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Comment> Comments { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

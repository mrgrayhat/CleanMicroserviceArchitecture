using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Abstractions
{
    public interface IStorageDbContext
    {

        public DbSet<Item> Items { get; set; }
        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}

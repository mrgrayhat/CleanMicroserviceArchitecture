using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Domain.Entities;
using StorageManagement.Infrastructure.Contexts;

namespace StorageManagement.Infrastructure.Repositories
{
    public class StorageRepositoryAsync : GenericRepositoryAsync<Item>, IStorageRepositoryAsync
    {
        private readonly DbSet<Item> _items;

        public StorageRepositoryAsync(StorageDbContext dbContext) : base(dbContext)
        {
            _items = dbContext.Set<Item>();
        }
        public override async ValueTask<Item> GetByIdAsync(int id)
        {
            return await _items
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async ValueTask<IReadOnlyList<Item>> SearchAsync(int pageNumber, int pageSize, string text, string sortOrder = "Desc")
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new System.ArgumentException($"'{nameof(text)}' cannot be null or empty", nameof(text));
            }

            return await _items
                .Where(x => x.Name.Contains(text, System.StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task HitDownload(int contentId)
        {
            var post = await base.GetByIdAsync(contentId);
            post.Downloaded++;
            await UpdateAsync(post);
        }
        public async Task<bool> IsUniqueTitleAsync(string name)
        {
            return await _items
            .AllAsync(p => p.Name != name);
        }
    }
}

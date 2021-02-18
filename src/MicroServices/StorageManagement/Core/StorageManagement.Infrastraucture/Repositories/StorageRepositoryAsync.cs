using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageManagement.Application.Exceptions;
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
        ///<summary>
        /// get by id using FirstOrDefault
        ///</summary>
        public override async ValueTask<Item> GetByIdAsync(int id)
        {
            return await _items
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async ValueTask<IReadOnlyList<Item>> SearchAsync(string text, int pageNumber = 1, int pageSize = 10, string sortOrder = "Desc")
        {
            if (string.IsNullOrEmpty(text))
                throw new ApiException($"'{nameof(text)}' cannot be null or empty", nameof(text));

            return await _items
                .Where(x => x.Name.Contains(text, System.StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
        public async Task<IReadOnlyList<Item>> GetPagedReponseAsync(int pageNumber, int pageSize, string sortOrder = "Desc")
        {
            var query = _items.AsQueryable();
            query = sortOrder == "Desc" ? 
                query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id);

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task HitDownload(int contentId)
        {
            var post = await base.GetByIdAsync(contentId);
            post.Downloaded++;
            await UpdateAsync(post);
        }
        public async Task<bool> IsUniqueFileHashAsync(string hash, CancellationToken cancellationtoken)
        {
            if (hash is null)
                throw new ArgumentNullException("{hash} is null", nameof(hash));
            return !await _items.AnyAsync(p => p.VerifiedHash.Equals(hash, StringComparison.Ordinal), cancellationtoken);
        }
    }
}

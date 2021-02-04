using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Domain.Entities;
using BlogModule.Infrastructure.Contexts;
using BlogModule.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogModule.Infrastructure.Repositories
{
    public class PostRepositoryAsync : GenericRepositoryAsync<Post>, IPostRepositoryAsync
    {
        private readonly DbSet<Post> _posts;
        private readonly DbSet<PostLocale> _postLocales;
        //private readonly BlogDbContext _dbContext;

        public PostRepositoryAsync(BlogDbContext dbContext) : base(dbContext)
        {
            //_dbContext = dbContext;
            _posts = dbContext.Set<Post>();
            _postLocales = dbContext.Set<PostLocale>();
        }
        /// <summary>
        /// get a post + include related data
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a value task</returns>
        public override async ValueTask<Post> GetByIdAsync(int id)
        {
            return await _posts
                .Include(x => x.Locales).ThenInclude(l => l.LocalCulture)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async ValueTask<IReadOnlyList<Post>> SearchAsync(int pageNumber, int pageSize, string text, string sortOrder = "Desc")
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new System.ArgumentException($"'{nameof(text)}' cannot be null or empty", nameof(text));
            }

            return await _posts
                .Include(x => x.Locales)
                .ThenInclude(l => l.LocalCulture)
                .TakeWhile(t => t.Locales.Any(x => x.Title.Contains(text)))
                .ToListAsync();
        }

        public async Task Visit(int postId)
        {
            var post = await base.GetByIdAsync(postId);
            post.Visits++;
            await UpdateAsync(post);
        }
        public async Task<bool> IsUniqueTitleAsync(string title)
        {
            return await _postLocales
            .AllAsync(p => p.Title != title);
        }
    }
}

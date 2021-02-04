using System.Collections.Generic;
using System.Threading.Tasks;
using BlogModule.Domain.Entities;

namespace BlogModule.Application.Interfaces.Repositories
{
    public interface IPostRepositoryAsync : IGenericRepositoryAsync<Post>
    {
        ValueTask<IReadOnlyList<Post>> SearchAsync(int pageNumber, int pageSize, string text, string sortOrder = "Desc");
        Task<bool> IsUniqueTitleAsync(string title);
        Task Visit(int postId);
    }
}

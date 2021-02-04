using System.Collections.Generic;
using System.Threading.Tasks;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Interfaces.Repositories
{
    public interface IStorageRepositoryAsync : IGenericRepositoryAsync<Item>
    {
        ValueTask<IReadOnlyList<Item>> SearchAsync(int pageNumber, int pageSize, string text, string sortOrder = "Desc");
        //Task<bool> IsUniqueTitleAsync(string title);
        Task HitDownload(int contentId);
    }
}

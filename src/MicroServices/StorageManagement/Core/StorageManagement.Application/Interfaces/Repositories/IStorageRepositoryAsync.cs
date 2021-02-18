using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Interfaces.Repositories
{
    public interface IStorageRepositoryAsync : IGenericRepositoryAsync<Item>
    {
        Task<IReadOnlyList<Item>> GetPagedReponseAsync(int pageNumber, int pageSize, string sortOrder = "Desc");
        ValueTask<IReadOnlyList<Item>> SearchAsync(string text, int pageNumber = 1, int pageSize = 10, string sortOrder = "Desc");
        Task HitDownload(int contentId);
        Task<bool> IsUniqueFileHashAsync(string hash, CancellationToken cancellationToken);
    }
}

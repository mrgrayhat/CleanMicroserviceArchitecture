using System.Threading;
using System.Threading.Tasks;

namespace StorageManagement.Application.Interfaces
{
    public interface IStorageAgent
    {
        Task<Task> CleanupDatabase(CancellationToken cancellationToken = default);

    }
}

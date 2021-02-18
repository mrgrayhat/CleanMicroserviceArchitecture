using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Interfaces.Repositories;

namespace StorageManagement.Application.Services
{
    public class StorageAgent : IStorageAgent
    {

        private readonly IConfiguration _configuration;
        private readonly ILogger<StorageAgent> _logger;
        private readonly IStorageRepositoryAsync _storageRepository;

        public StorageAgent(ILogger<StorageAgent> logger, IConfiguration configuration, IStorageRepositoryAsync storageRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _storageRepository = storageRepository;
        }


        public async Task<Task> CleanupDatabase(CancellationToken cancellationToken = default)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            var items = await _storageRepository.GetAllAsync();
            _logger.LogInformation("Storage Agent: Database Cleanup operation running...");
            _logger.LogInformation(" found {total} items in storage database", items.Count);
            foreach (var item in items)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Storage Agent: Database Cleanup operation Aborted due to trigger cancellation.");
                    //break;
                    return Task.FromCanceled(cancellationToken);
                }
                if (File.Exists(item.Url) is false)
                {
                    _logger.LogWarning("Storage Agent: Database Cleanup found an item {item} in database, but related file path {fileUrl} is not exist in Storage FileSystem, Trying to delete record.", item.Name, item.Url);
                    await _storageRepository.DeleteAsync(item);
                    _logger.LogWarning("Record ID {Id} with name {name} Deleted Successfully from database.", item.Id, item.Name);
                }
            }
            watch.Stop();
            _logger.LogInformation("Storage Agent: Dtabase Cleanup operation finished after {elapsed} ms.", watch.ElapsedMilliseconds);
            //cancellationToken.ThrowIfCancellationRequested();
            return Task.CompletedTask;
        }

    }
}

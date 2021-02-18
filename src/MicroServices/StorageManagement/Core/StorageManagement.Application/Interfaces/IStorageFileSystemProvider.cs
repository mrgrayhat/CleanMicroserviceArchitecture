using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StorageManagement.Application.Wrappers;

namespace StorageManagement.Application.Interfaces
{
    public interface IStorageFileSystemProvider
    {
        Task<string> StoreAsync(IFormFile file, CancellationToken cancellationToken);
        Task<Stream> RetriveAsync(string filePath, CancellationToken cancellationToken);
        Response<int> DeleteAsync(string filePath, CancellationToken cancellationToken);
        Task<string> ArchiveAsync(string filePath);
        Task<bool> LockFile(string filePath, TimeSpan? durration);
    }
 }

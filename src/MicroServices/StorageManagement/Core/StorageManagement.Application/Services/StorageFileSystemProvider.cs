using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Wrappers;

namespace StorageManagement.Application.Services
{
    /// <summary>
    /// This service is for manage physical files (<see cref="File"/>) in the FileSystem.
    /// Store (Save into the specified storage path with unique name),Retrive(open for read or download or etc),Delete files and Search for them, are main functions.
    /// </summary>
    public class StorageFileSystemProvider : IStorageFileSystemProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<StorageFileSystemProvider> _logger;

        public StorageFileSystemProvider(ILogger<StorageFileSystemProvider> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// save the input file to the storage, based on configured appsetting
        /// </summary>
        /// <param name="file"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> StoreAsync(IFormFile file, CancellationToken cancellationToken)
        {
            string fileName = Guid.NewGuid().ToString("N"); // Give file name
            string extension = Path.GetExtension(file.FileName);

            string filePath = Path.Combine(_configuration["Storage:StoragePath"],
                string.Concat(fileName, extension));

            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await file.CopyToAsync(fs, cancellationToken).ConfigureAwait(false);

            return filePath;
        }
        /// <summary>
        /// open file stream to read
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Stream> RetriveAsync(string filePath, CancellationToken cancellationToken)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            return await Task.FromResult(fs);
        }
        public Response<int> DeleteAsync(string filePath, CancellationToken cancellationToken)
        {
            Response<int> response = new Response<int>();

            if (File.Exists(filePath) is false)
                response.Message = _configuration["Storage:Messages:NotFound"];

            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.Message);
                response.Message = _configuration["Storage:Messages:CouldnotComplete"];
                response.Succeeded = false;
                return response;
            }
            return response;
        }
        /// <summary>
        /// move the file to the Specified archive path(in the settings)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Task<string> ArchiveAsync(string filePath)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// lock file for others access, until specified durration (default:30s)
        /// </summary>
        /// <param name="filePath">path of the file</param>
        /// <param name="durration">lock durration</param>
        /// <returns></returns>
        public async Task<bool> LockFile(string filePath, TimeSpan? durration)
        {
            using var f = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.None);
            await Task.Delay(durration.GetValueOrDefault(TimeSpan.FromSeconds(30)));

            return true;
        }

    }
}

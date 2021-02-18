using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StorageManagement.Application.Exceptions;
using StorageManagement.Application.Extensions;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Commands.UpdateContent
{
    public class UpdateContentCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        /// <summary>
        /// a nickname name
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "No More than 50 character are allowed")]
        public string Name { get; set; }
        public string ContentType { get; private set; }
        [MaxLength(500, ErrorMessage = "No More that 500 character are allowed")]
        public string Description { get; set; }
        public DateTime LastModified { get; private set; }
        public string LastModifiedBy { get; private set; }
        public IFormFile File { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// total bytes of data
        /// </summary>
        public long Size { get; private set; }
        /// <summary>
        /// Unique Hash to protect against invalid/untracked changes
        /// </summary>
        public string VerifiedHash { get; private set; }
    }

    public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, Response<int>>
    {
        private readonly IStorageRepositoryAsync _storageRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IStorageFileSystemProvider _storageFileSystemProvider;
        private readonly ILogger<UpdateContentCommandHandler> _logger;

        public UpdateContentCommandHandler(ILogger<UpdateContentCommandHandler> logger, IStorageRepositoryAsync storageRepository, IStorageFileSystemProvider storageFileSystemProvider, IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException($"{nameof(httpContext)} is null");
            _logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} is null");
            _storageFileSystemProvider = storageFileSystemProvider ?? throw new ArgumentNullException($"{nameof(storageFileSystemProvider)} is null");
            _storageRepository = storageRepository ?? throw new ArgumentNullException($"{nameof(storageRepository)} is null"); ;
        }
        public async Task<Response<int>> Handle(UpdateContentCommand command, CancellationToken cancellationToken)
        {
            Item item = await _storageRepository.GetByIdAsync(command.Id);

            if (item == null)
            {
                throw new ApiException($"Content Not Found.");
            }
            else
            {
                item.Name = command.Name ?? Path.GetFileNameWithoutExtension(command.File.FileName);
                item.ContentType = command.File.GetContentType();
                item.LastModifiedBy = _httpContext.HttpContext.User.Identity.Name;
                item.Description = command.Description;
                item.Size = command.File.Length;
                item.VerifiedHash = await command.File
                    .CalculateMD5FileHashAsync(cancellationToken);

                item.Url = await _storageFileSystemProvider
                    .StoreAsync(command.File, cancellationToken, overwrite: true).ConfigureAwait(false);

                if (!string.IsNullOrWhiteSpace(item.Url))
                    await _storageRepository.UpdateAsync(item);

                _logger.LogInformation("storage item {Name} successfully updated in {Url}", item.Name, item.Url);
                return new Response<int>
                {
                    Data = item.Id,
                    Succeeded = true,
                    Message = $"storage item {item.Name} successfully updated."
                };
            }
        }
    }
}
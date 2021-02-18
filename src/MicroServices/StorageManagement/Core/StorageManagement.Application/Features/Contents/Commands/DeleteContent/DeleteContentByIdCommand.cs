using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using StorageManagement.Application.Exceptions;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Commands.DeleteContent
{
    public class DeleteContentByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
    }
    public class DeleteContentByIdCommandHandler : IRequestHandler<DeleteContentByIdCommand, Response<int>>
    {
        private readonly IConfiguration _configuration;
        private readonly IStorageRepositoryAsync _storageRepository;
        private readonly IStorageFileSystemProvider _storageFileSystemProvider;
        public DeleteContentByIdCommandHandler(IConfiguration configuration, IStorageRepositoryAsync postRepository, IStorageFileSystemProvider storageFileSystemProvider)
        {
            _configuration = configuration;
            _storageFileSystemProvider = storageFileSystemProvider;
            _storageRepository = postRepository;
        }
        public async Task<Response<int>> Handle(DeleteContentByIdCommand command, CancellationToken cancellationToken)
        {
            Item content = await _storageRepository.GetByIdAsync(command.Id);
            if (content is null)
            {
                throw new ApiException(_configuration["Storage:Messages:NotFound"], command.Id);
                //return null;
            }
            Response<int> storageResult = _storageFileSystemProvider.DeleteAsync(content.Url, cancellationToken);
            if (storageResult.Succeeded is true)
                await _storageRepository.DeleteAsync(content);

            storageResult.Data = command.Id;
            return storageResult;

        }
    }
}

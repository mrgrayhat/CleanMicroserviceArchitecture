using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StorageManagement.Application.Exceptions;
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
        private readonly IStorageRepositoryAsync _storageRepository;
        public DeleteContentByIdCommandHandler(IStorageRepositoryAsync postRepository)
        {
            _storageRepository = postRepository;
        }
        public async Task<Response<int>> Handle(DeleteContentByIdCommand command, CancellationToken cancellationToken)
        {
            Item content = await _storageRepository.GetByIdAsync(command.Id);
            if (content == null) throw new ApiException($"Content Not Found.");
            await _storageRepository.DeleteAsync(content);
            return new Response<int>(content.Id);
        }
    }
}

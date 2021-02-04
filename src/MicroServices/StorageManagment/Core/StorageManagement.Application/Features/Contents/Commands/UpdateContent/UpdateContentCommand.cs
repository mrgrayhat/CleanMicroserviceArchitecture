using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using StorageManagement.Application.Exceptions;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Commands.UpdateContent
{
    public class UpdateContentCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContentType ContentType { get; set; }
        public int Downloads { get; set; }
        public string Description { get; set; }
        public IFormFile File { get; set; }


        public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, Response<int>>
        {
            private readonly IStorageRepositoryAsync _storageRepository;
            public UpdateContentCommandHandler(IStorageRepositoryAsync storageRepository)
            {
                _storageRepository = storageRepository;
            }
            public async Task<Response<int>> Handle(UpdateContentCommand command, CancellationToken cancellationToken)
            {
                Item content = await _storageRepository.GetByIdAsync(command.Id);

                if (content == null)
                {
                    throw new ApiException($"Content Not Found.");
                }
                else
                {
                    content.Name = command.Name;
                    content.Downloaded = command.Downloads;
                    content.Description = command.Description;
                    content.ContentType = command.ContentType;

                    await _storageRepository.UpdateAsync(content);
                    return new Response<int>(content.Id);
                }
            }
        }
    }
}

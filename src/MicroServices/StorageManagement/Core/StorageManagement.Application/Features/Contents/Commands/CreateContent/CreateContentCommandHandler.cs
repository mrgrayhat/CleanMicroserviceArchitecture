using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StorageManagement.Application.DTOs;
using StorageManagement.Application.Events;
using StorageManagement.Application.Exceptions;
using StorageManagement.Application.Extensions;
using StorageManagement.Application.Interfaces;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Commands.CreateContent
{
    public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, Response<ItemDto>>
    {
        private readonly IStorageRepositoryAsync _storageRepository;
        private readonly IStorageFileSystemProvider _storageFileSystemProvider;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMediator _mediator;
        private readonly ILogger<CreateContentCommandHandler> _logger;

        public CreateContentCommandHandler(IStorageRepositoryAsync storageRepository, IMapper mapper, IMediator mediator, IStorageFileSystemProvider storageFileSystemProvider, ILogger<CreateContentCommandHandler> logger, IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException($"{nameof(httpContext)} is null");
            _logger = logger ?? throw new ArgumentNullException($"{nameof(logger)} is null");
            _storageFileSystemProvider = storageFileSystemProvider ?? throw new ArgumentNullException($"{nameof(storageFileSystemProvider)} is null"); ;
            _storageRepository = storageRepository ?? throw new ArgumentNullException($"{nameof(storageRepository)} is null"); ;
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mapper)} is null"); ;
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)} is null");
        }

        public async Task<Response<ItemDto>> Handle(CreateContentCommand request, CancellationToken cancellationToken)
        {
            Item item = _mapper.Map<Item>(request);
            ItemDto result;
            try
            {
                item.Name = request.Name ?? Path.GetFileNameWithoutExtension(request.File.FileName);
                item.ContentType = request.File.GetContentType();
                item.CreatedBy = _httpContext.HttpContext.User.Identity.Name;
                item.Size = request.File.Length;
                item.VerifiedHash = await request.File
                    .CalculateMD5FileHashAsync(cancellationToken);
                item.Url = await _storageFileSystemProvider
                    .StoreAsync(request.File, cancellationToken).ConfigureAwait(false);
                _logger.LogInformation("storage item {Name} successfully stored in {Url}", item.Name, item.Url);

                result = _mapper.Map<ItemDto>(await _storageRepository.AddAsync(item));
                // Raising new content created Event ...
                await _mediator.Publish(new ContentCreatedEvent(DateTime.Now, $"{item.CreatedBy}, {_httpContext.HttpContext.Connection.RemoteIpAddress}", item.VerifiedHash, item.Url), cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("failed to store item {Name}, {ex}", item.Name, ex);
                throw new ApiException(ex.Message);
            }
            return new Response<ItemDto>(result);
        }
    }
}

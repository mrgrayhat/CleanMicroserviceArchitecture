using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using StorageManagement.Application.DTOs;
using StorageManagement.Application.Events;
using StorageManagement.Application.Exceptions;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Queries.GetContenById
{
    public class GetContentByIdQuery : IRequest<Response<ItemDto>>
    {
        /// <summary>
        /// content id
        /// </summary>
        public int Id { get; set; }
    }
    public class GetContentByIdQueryHandler : IRequestHandler<GetContentByIdQuery, Response<ItemDto>>
    {
        private readonly IStorageRepositoryAsync _storageRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetContentByIdQueryHandler(IStorageRepositoryAsync postRepository, IMapper mapper, IMediator mediator, IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException($"{nameof(httpContext)} is null");
            _storageRepository = postRepository ?? throw new ArgumentNullException($"{nameof(mediator)} is null");
            _mapper = mapper ?? throw new ArgumentNullException($"{nameof(mediator)} is null");
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)} is null");
        }
        public async Task<Response<ItemDto>> Handle(GetContentByIdQuery query, CancellationToken cancellationToken)
        {
            Item content = await _storageRepository.GetByIdAsync(query.Id);
            if (content == null)
            {
                throw new ApiException($"Content Not Found.");
            }
            ItemDto contentDto = _mapper.Map<ItemDto>(content);
            // Raising newlly content created Event ...
            await _mediator.Publish(new ContentRequestedEvent(DateTime.Now, contentDto.Id, _httpContext.HttpContext.User.Identity.Name), cancellationToken);
            return new Response<ItemDto>(contentDto);
        }
    }
}

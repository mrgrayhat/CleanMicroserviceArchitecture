using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using StorageManagement.Application.Events;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Commands.CreateContent
{
    public class CreateContentCommand : IRequest<Response<int>>
    {
        public IFormFile File { get; set; }
        public string Title { get; set; }
        public ContentType ContentType { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string LastModifiedBy { get; set; }
        public string Description { get; set; }

    }
    public class CreateContentCommandHandler : IRequestHandler<CreateContentCommand, Response<int>>
    {
        private readonly IStorageRepositoryAsync _postRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CreateContentCommandHandler(IStorageRepositoryAsync postRepository, IMapper mapper, IMediator mediator)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)} is null");
        }

        public async Task<Response<int>> Handle(CreateContentCommand request, CancellationToken cancellationToken)
        {
            Item post = _mapper.Map<Item>(request);
            await _postRepository.AddAsync(post);
            // Raising new content created Event ...
            await _mediator.Publish(new ContentCreatedEvent(DateTime.Now, post.CreatedBy), cancellationToken);

            return new Response<int>(post.Id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlogModule.Application.DTOs.Post;
using BlogModule.Application.Events.Posts;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Application.Wrappers;
using BlogModule.Domain.Entities;
using MediatR;

namespace BlogModule.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<Response<int>>
    {
        //public DateTime Created { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTime? LastModified { get; set; }
        //public string LastModifiedBy { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// language specific content's
        /// </summary>
        public ICollection<PostLocaleDto> Locales { get; set; }

    }
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Response<int>>
    {
        private readonly IPostRepositoryAsync _postRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public CreatePostCommandHandler(IPostRepositoryAsync postRepository, IMapper mapper, IMediator mediator)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)} is null");
        }

        public async Task<Response<int>> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            Post post = _mapper.Map<Post>(request);
            await _postRepository.AddAsync(post);
            // Raising acceptance by admin Event ...
            await _mediator.Publish(new PostApprovalPendingEvent(DateTime.Now, post.CreatedBy, post.Locales.FirstOrDefault().Title), cancellationToken);
            // Raising newlly post created Event ...
            await _mediator.Publish(new PostCreatedEvent(DateTime.Now, post.CreatedBy), cancellationToken);

            return new Response<int>(post.Id);
        }
    }
}

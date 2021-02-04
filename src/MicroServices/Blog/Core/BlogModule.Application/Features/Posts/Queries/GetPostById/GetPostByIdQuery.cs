using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlogModule.Application.DTOs.Post;
using BlogModule.Application.Events.Posts;
using BlogModule.Application.Exceptions;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Application.Wrappers;
using BlogModule.Domain.Entities;
using MediatR;

namespace BlogModule.Application.Features.Posts.Queries.GetPostById
{
    public class GetPostByIdQuery : IRequest<Response<GetPostDto>>
    {
        /// <summary>
        /// post id
        /// </summary>
        public int Id { get; set; }
    }
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Response<GetPostDto>>
    {
        private readonly IPostRepositoryAsync _postRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public GetPostByIdQueryHandler(IPostRepositoryAsync postRepository, IMapper mapper, IMediator mediator)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            _mediator = mediator ?? throw new ArgumentNullException($"{nameof(mediator)} is null");
        }
        public async Task<Response<GetPostDto>> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
        {
            Post post = await _postRepository.GetByIdAsync(query.Id);
            if (post == null)
                throw new ApiException($"Post Not Found.");

            var postDto = _mapper.Map<GetPostDto>(post);
            // Raising newlly post created Event ...
            //TODO: pass Client Info(ip) for event parameter
            await _mediator.Publish(new PostRequestedEvent(DateTime.Now, postDto.Id, ""), cancellationToken);
            return new Response<GetPostDto>(postDto);
        }
    }
}

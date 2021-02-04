using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlogModule.Application.DTOs.Post;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Application.Wrappers;
using BlogModule.Domain.Entities;
using MediatR;

namespace BlogModule.Application.Features.Posts.Queries.GetAllPosts
{
    /// <summary>
    /// Get Logs Query.
    /// </summary>
    public class GetAllPostsQuery : IRequest<PagedResponse<IEnumerable<GetPostDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    /// <summary>
    /// Get Posts Query Handler. call service and do mappings to generate response.
    /// </summary>
    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, PagedResponse<IEnumerable<GetPostDto>>>
    {
        private readonly IPostRepositoryAsync _postRepository;
        private readonly IMapper _mapper;
        //private readonly IMediator _mediator;

        public GetAllPostsQueryHandler(IPostRepositoryAsync postRepository, IMapper mapper/*, IMediator mediator*/)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            //_mediator = mediator;
        }
        /// <summary>
        /// get posts query handler
        /// </summary>
        /// <param name="request">request query</param>
        /// <param name="cancellationToken">thread cancellation notif</param>
        /// <returns>paging collection of posts</returns>
        public async Task<PagedResponse<IEnumerable<GetPostDto>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            GetAllPostsParameter validParams = _mapper.Map<GetAllPostsParameter>(request);
            IReadOnlyList<Post> posts = await _postRepository
                .GetPagedReponseAsync(validParams.PageNumber, validParams.PageSize);
            var postViewModel = _mapper.Map<IEnumerable<GetPostDto>>(posts);

            // Raising Event ...
            //await _mediator.Publish(new PostRequestedEvent(DateTime.Now, validParams.IP), cancellationToken);

            return new PagedResponse<IEnumerable<GetPostDto>>(postViewModel, validParams.PageNumber, validParams.PageSize);
        }
    }
}

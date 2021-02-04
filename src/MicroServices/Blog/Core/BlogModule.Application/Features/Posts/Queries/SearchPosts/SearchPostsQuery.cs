using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlogModule.Application.DTOs.Post;
using BlogModule.Application.Interfaces.Repositories;
using BlogModule.Application.Wrappers;
using BlogModule.Domain.Entities;
using MediatR;

namespace BlogModule.Application.Features.Posts.Queries.SearchPosts
{
    /// <summary>
    /// search posts Query/Filter.
    /// </summary>
    public class SearchPostsQuery : IRequest<PagedResponse<IEnumerable<GetPostDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string Text { get; set; }
        public string SortOrder { get; set; }
    }
    /// <summary>
    /// search Posts Query Handler. call service and do mappings to generate response.
    /// </summary>
    public class SearchPostsQueryHandler : IRequestHandler<SearchPostsQuery, PagedResponse<IEnumerable<GetPostDto>>>
    {
        private readonly IPostRepositoryAsync _postRepository;
        private readonly IMapper _mapper;
        //private readonly IMediator _mediator;

        public SearchPostsQueryHandler(IPostRepositoryAsync postRepository, IMapper mapper/*, IMediator mediator*/)
        {
            _postRepository = postRepository;
            _mapper = mapper;
            //_mediator = mediator;
        }
        /// <summary>
        /// search posts query handler
        /// </summary>
        /// <param name="request">request query</param>
        /// <param name="cancellationToken">thread cancellation notif</param>
        /// <returns>paging collection of search result</returns>
        public async Task<PagedResponse<IEnumerable<GetPostDto>>> Handle(SearchPostsQuery request, CancellationToken cancellationToken)
        {
            SearchPostsParameter validParams = _mapper.Map<SearchPostsParameter>(request);
            IReadOnlyList<Post> posts = await _postRepository
                .SearchAsync(validParams.PageNumber, validParams.PageSize, validParams.Text, validParams.SortOrder);
            var postViewModel = _mapper.Map<IEnumerable<GetPostDto>>(posts);

            // Raising Event ...
            //await _mediator.Publish(new PostRequestedEvent(DateTime.Now, validParams.IP), cancellationToken);

            return new PagedResponse<IEnumerable<GetPostDto>>(postViewModel,
                validParams.PageNumber, validParams.PageSize);
        }
    }
}

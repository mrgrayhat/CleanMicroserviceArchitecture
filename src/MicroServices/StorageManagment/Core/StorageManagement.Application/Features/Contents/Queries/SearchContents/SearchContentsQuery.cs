using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using StorageManagement.Application.DTOs;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Queries.SearchContents
{
    /// <summary>
    /// search contents Query/Filter.
    /// </summary>
    public class SearchContentsQuery : IRequest<PagedResponse<IEnumerable<ItemDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public string Text { get; set; }
        public string SortOrder { get; set; }
    }
    /// <summary>
    /// search Contents Query Handler. call service and do mappings to generate response.
    /// </summary>
    public class SearchContentsQueryHandler : IRequestHandler<SearchContentsQuery, PagedResponse<IEnumerable<ItemDto>>>
    {
        private readonly IStorageRepositoryAsync _storageRepository;
        private readonly IMapper _mapper;

        public SearchContentsQueryHandler(IStorageRepositoryAsync postRepository, IMapper mapper)
        {
            _storageRepository = postRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// search Contents query handler
        /// </summary>
        /// <param name="request">request query</param>
        /// <param name="cancellationToken">thread cancellation notif</param>
        /// <returns>paging collection of search result</returns>
        public async Task<PagedResponse<IEnumerable<ItemDto>>> Handle(SearchContentsQuery request, CancellationToken cancellationToken)
        {
            SearchContentsParameter validParams = _mapper.Map<SearchContentsParameter>(request);
            IReadOnlyList<Item> contents = await _storageRepository
                .SearchAsync(validParams.PageNumber, validParams.PageSize, validParams.Text, validParams.SortOrder);
            var contentViewModel = _mapper.Map<IEnumerable<ItemDto>>(contents);

            // Raising Event ...
            //await _mediator.Publish(new PostRequestedEvent(DateTime.Now, validParams.IP), cancellationToken);

            return new PagedResponse<IEnumerable<ItemDto>>(contentViewModel,
                validParams.PageNumber, validParams.PageSize);
        }
    }
}

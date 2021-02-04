using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using StorageManagement.Application.DTOs;
using StorageManagement.Application.Interfaces.Repositories;
using StorageManagement.Application.Wrappers;
using StorageManagement.Domain.Entities;

namespace StorageManagement.Application.Features.Contents.Queries.GetAllContents
{
    /// <summary>
    /// Get Logs Query.
    /// </summary>
    public class GetAllContentsQuery : IRequest<PagedResponse<IEnumerable<ItemDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    /// <summary>
    /// Get Posts Query Handler. call service and do mappings to generate response.
    /// </summary>
    public class GetAllContentsQueryHandler : IRequestHandler<GetAllContentsQuery, PagedResponse<IEnumerable<ItemDto>>>
    {
        private readonly IStorageRepositoryAsync _postRepository;
        private readonly IMapper _mapper;
        //private readonly IMediator _mediator;

        public GetAllContentsQueryHandler(IStorageRepositoryAsync postRepository, IMapper mapper/*, IMediator mediator*/)
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
        public async Task<PagedResponse<IEnumerable<ItemDto>>> Handle(GetAllContentsQuery request, CancellationToken cancellationToken)
        {
            GetAllContentsParameter validParams = _mapper.Map<GetAllContentsParameter>(request);
            IReadOnlyList<Item> contents = await _postRepository
                .GetPagedReponseAsync(validParams.PageNumber, validParams.PageSize);
            var contenViewModel = _mapper.Map<IEnumerable<ItemDto>>(contents);

            // Raising Event ...
            //await _mediator.Publish(new PostRequestedEvent(DateTime.Now, validParams.IP), cancellationToken);

            return new PagedResponse<IEnumerable<ItemDto>>(contenViewModel, validParams.PageNumber, validParams.PageSize);
        }
    }
}

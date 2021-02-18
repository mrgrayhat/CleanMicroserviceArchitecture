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
    /// Get Storage Content Query with filters
    /// </summary>
    public class GetAllContentsQuery : IRequest<PagedResponse<IEnumerable<ItemDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortOrder { get; set; }
    }
    public class GetAllContentsQueryHandler : IRequestHandler<GetAllContentsQuery, PagedResponse<IEnumerable<ItemDto>>>
    {
        private readonly IStorageRepositoryAsync _postRepository;
        private readonly IMapper _mapper;

        public GetAllContentsQueryHandler(IStorageRepositoryAsync postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }
        public async Task<PagedResponse<IEnumerable<ItemDto>>> Handle(GetAllContentsQuery request, CancellationToken cancellationToken)
        {
            GetAllContentsParameter validParams = _mapper.Map<GetAllContentsParameter>(request);
            IReadOnlyList<Item> contents = await _postRepository
                .GetPagedReponseAsync(validParams.PageNumber, validParams.PageSize, validParams.SortOrder);
            var contenViewModel = _mapper.Map<IEnumerable<ItemDto>>(contents);

            return new PagedResponse<IEnumerable<ItemDto>>(contenViewModel, validParams.PageNumber, validParams.PageSize, contents.Count);
        }
    }
}

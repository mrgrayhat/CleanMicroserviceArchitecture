using System.Threading;
using System.Threading.Tasks;
using LogModule.Application.Exceptions;
using LogModule.Application.Interfaces.Repositories;
using LogModule.Application.Wrappers;
using MediatR;

namespace LogModule.Application.Features.Logs.Queries.GetLogById
{
    public class GetLogByIdQuery : IRequest<Response<Domain.Entities.Log>>
    {
        public string Id { get; set; }
        public class GetLogByIdQueryHandler : IRequestHandler<GetLogByIdQuery, Response<Domain.Entities.Log>>
        {
            private readonly ILogRepositoryAsync _LogRepository;
            public GetLogByIdQueryHandler(ILogRepositoryAsync LogRepository)
            {
                _LogRepository = LogRepository;
            }
            public async Task<Response<Domain.Entities.Log>> Handle(GetLogByIdQuery query, CancellationToken cancellationToken)
            {
                var Log = await _LogRepository.GetByIdAsync(query.Id);
                if (Log == null) throw new ApiException($"Log Not Found.");
                return new Response<LogModule.Domain.Entities.Log>(Log);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LogModule.Application.Enums;
using LogModule.Application.Events.Log;
using LogModule.Application.Interfaces.Repositories;
using LogModule.Application.Wrappers;
using MediatR;

namespace LogModule.Application.Features.Logs.Queries.GetAllLogs
{
    /// <summary>
    /// Get Logs Query.
    /// </summary>
    public class GetAllLogsQuery : IRequest<PagedResponse<IEnumerable<GetAllLogsViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public LogLevels LogLevel { get; set; }
        public string IP { get; set; }
    }
    /// <summary>
    /// Get Logs Query Handler. call service and do mappings to generate response.
    /// </summary>
    public class GetAllLogsQueryHandler : IRequestHandler<GetAllLogsQuery, PagedResponse<IEnumerable<GetAllLogsViewModel>>>
    {
        private readonly ILogRepositoryAsync _logRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public GetAllLogsQueryHandler(ILogRepositoryAsync logRepository, IMapper mapper, IMediator mediator)
        {
            _logRepository = logRepository;
            _mapper = mapper;
            _mediator = mediator;
        }
        /// <summary>
        /// get logs query handler
        /// </summary>
        /// <param name="request">request query</param>
        /// <param name="cancellationToken">thread cancellation notif</param>
        /// <returns>paging collection of logs</returns>
        public async Task<PagedResponse<IEnumerable<GetAllLogsViewModel>>> Handle(GetAllLogsQuery request, CancellationToken cancellationToken)
        {
            GetAllLogsParameter validParams = _mapper.Map<GetAllLogsParameter>(request);
            var log = await _logRepository.GetPagedReponseAsync(validParams.PageNumber, validParams.PageSize, validParams.LogLevel);
            var logViewModel = _mapper.Map<IEnumerable<GetAllLogsViewModel>>(log);

            // Raising Event ...
            await _mediator.Publish(new LogRequestedEvent(DateTime.Now, validParams.IP), cancellationToken);

            return new PagedResponse<IEnumerable<GetAllLogsViewModel>>(logViewModel, validParams.PageNumber, validParams.PageSize);
        }
    }
}

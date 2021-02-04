using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LogModule.Application.Enums;
using LogModule.Application.Interfaces.Repositories;
using LogModule.Application.Wrappers;
using MediatR;

namespace LogModule.Application.Features.Logs.Commands.CreateLog
{
    public class CreateLogCommand : IRequest<Response<int>>
    {
        public string Guid { get; set; } = new Guid().ToString();
        public string Message { get; set; }
        public string Description { get; set; }
        public LogLevels Level { get; set; } = LogLevels.Information;
    }
    public class CreateLogCommandHandler : IRequestHandler<CreateLogCommand, Response<int>>
    {
        private readonly ILogRepositoryAsync _LogRepository;
        private readonly IMapper _mapper;
        public CreateLogCommandHandler(ILogRepositoryAsync LogRepository, IMapper mapper)
        {
            _LogRepository = LogRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateLogCommand request, CancellationToken cancellationToken)
        {
            var Log = _mapper.Map<Domain.Entities.Log>(request);
            await _LogRepository.AddAsync(Log);
            return new Response<int>(Log.Id);
        }
    }
}

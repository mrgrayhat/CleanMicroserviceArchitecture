using System.Threading;
using System.Threading.Tasks;
using LogModule.Application.Exceptions;
using LogModule.Application.Interfaces.Repositories;
using LogModule.Application.Wrappers;
using MediatR;

namespace LogModule.Application.Features.Logs.Commands.DeleteLogById
{
    public class DeleteLogByIdCommand : IRequest<Response<int>>
    {
        public string Id { get; set; }
        public class DeleteLogByIdCommandHandler : IRequestHandler<DeleteLogByIdCommand, Response<int>>
        {
            private readonly ILogRepositoryAsync _LogRepository;
            public DeleteLogByIdCommandHandler(ILogRepositoryAsync LogRepository)
            {
                _LogRepository = LogRepository;
            }
            public async Task<Response<int>> Handle(DeleteLogByIdCommand command, CancellationToken cancellationToken)
            {
                var Log = await _LogRepository.GetByIdAsync(command.Id);
                if (Log == null) throw new ApiException($"Log Not Found.");
                await _LogRepository.DeleteAsync(Log);
                return new Response<int>(Log.Id);
            }
        }
    }
}

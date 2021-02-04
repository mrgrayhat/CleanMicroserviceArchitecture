using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BlogModule.Application.Behaviours
{
    /// <summary>
    /// measure request process performance and raise events if needed.
    /// for example: log about a query/command if it took more than 5 seconds to complete.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class RequestPerformanceBehavior<TRequest, TResponse> :
     IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<RequestPerformanceBehavior<TRequest, TResponse>> _logger;

        public RequestPerformanceBehavior(ILogger<RequestPerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            TResponse response = await next();

            stopwatch.Stop();

            if (stopwatch.ElapsedMilliseconds > TimeSpan.FromSeconds(5).TotalMilliseconds)
            {
                // This method has taken a long time, So we log that to check it later.
                _logger.LogWarning($"[{(int)System.Net.HttpStatusCode.TooManyRequests}] {request} has taken {stopwatch.ElapsedMilliseconds}ms to run completely!");
            }

            return response;
        }
    }
}

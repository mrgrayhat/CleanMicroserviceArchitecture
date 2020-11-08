using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace SaeedRezayi.IoCConfig.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            _logger = logFactory.CreateLogger("ExceptionHandlerMiddleware");
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogTrace("Exception Handler Middleware executing...");
            var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
            Guid errorId = Guid.NewGuid();
            //Serilog.Log.ForContext("Type", "Error")
            //    .ForContext("Exception", exception, destructureObjects: true)
            //    .Error(exception, exception.Message + ". {@errorId}", errorId);
            if (error?.Error is Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response
                .WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    State = 401,
                    ErrorID = errorId,
                    Msg = "token expired"
                }));
            }
            else if (error?.Error != null)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response
                .WriteAsync(System.Text.Json.JsonSerializer.Serialize(new
                {
                    State = 500,
                    ErrorID = errorId,
                    Msg = error?.Error.Message
                }));
            }
            else
            {
                await _next(context); // or don't go next!? (get error and stop next process?)
            }
        }
    }
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}

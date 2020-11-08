using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace SaeedRezayi.IoCConfig.Middlewares
{
    public class RequestTrackerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestTrackerMiddleware(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            _logger = logFactory.CreateLogger("RequestTrackerMiddleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogTrace("Request Tracker Middleware executing...");
            //Get username
            var username = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "anonymous";
            LogContext.PushProperty("User", username);
            //Get remote IP address
            var ip = httpContext.Connection.RemoteIpAddress.ToString();
            LogContext.PushProperty("IP", !string.IsNullOrWhiteSpace(ip) ? ip : "unknown");

            _logger.LogTrace($"Request from User {username} with IP: {ip}");
            await _next(httpContext); // calling next middleware
        }
    }

    public static class RequestTrackerMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTrackerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTrackerMiddleware>();
        }
    }
}

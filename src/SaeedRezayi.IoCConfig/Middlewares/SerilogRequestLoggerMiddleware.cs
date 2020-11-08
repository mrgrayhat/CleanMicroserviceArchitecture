using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace SaeedRezayi.IoCConfig.Middlewares
{
    public class SerilogRequestLoggerMiddleware
    {
        readonly RequestDelegate _next;

        public SerilogRequestLoggerMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            // Push the user name into the log context so that it is included in all log entries
                        //Get username
            var username = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "anonymous";
            LogContext.PushProperty("Username", username);

            // TODO: Hanlde null connection props
            var ip = httpContext.Connection.RemoteIpAddress?.ToString();
            LogContext.PushProperty("IP", !string.IsNullOrWhiteSpace(ip) ? ip : "unknown");
            Log.Debug($"Request from User {username} with IP: {ip}");

            // Getting the request body is a little tricky because it's a stream
            // So, we need to read the stream and then rewind it back to the beginning
            string requestBody = "";
            httpContext.Request.EnableBuffering();
            Stream body = httpContext.Request.Body;
            byte[] buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
            await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            requestBody = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            httpContext.Request.Body = body;

            Log.ForContext("RequestHeaders", httpContext.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
               .ForContext("RequestBody", requestBody)
               .Debug("Request information {RequestMethod} {RequestPath} information", httpContext.Request.Method, httpContext.Request.Path);


            // The reponse body is also a stream so we need to:
            // - hold a reference to the original response body stream
            // - re-point the response body to a new memory stream
            // - read the response body after the request is handled into our memory stream
            // - copy the response in the memory stream out to the original response stream
            using (var responseBodyMemoryStream = new MemoryStream())
            {
                var originalResponseBodyReference = httpContext.Response.Body;
                httpContext.Response.Body = responseBodyMemoryStream;

                await _next(httpContext);

                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
                httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

                Log.ForContext("RequestBody", requestBody)
                   .ForContext("ResponseBody", responseBody)
                   .Debug("Response information {RequestMethod} {RequestPath} {statusCode}", httpContext.Request.Method, httpContext.Request.Path, httpContext.Response.StatusCode);

                await responseBodyMemoryStream.CopyToAsync(originalResponseBodyReference);
            }
        }
    }
}

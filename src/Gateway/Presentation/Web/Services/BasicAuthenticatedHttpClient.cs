using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Http;

namespace Gateway.Web.Api.Services
{
    /// <summary>
    /// custom http client for log sender
    /// </summary>
    public class BasicAuthenticatedHttpClient : IHttpClient
    {
        private readonly HttpClient httpClient;

        public BasicAuthenticatedHttpClient()
        {
            httpClient = new HttpClient();
        }

        public void Configure(IConfiguration configuration)
        {
            string username = configuration["LogServer:Username"];
            string password = configuration["LogServer:Password"];
            httpClient.DefaultRequestHeaders.Add("X-Api-Key", configuration["LogServer:ApiKey"]);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}")));
        }

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) => httpClient.PostAsync(requestUri, content);

        public void Dispose() => httpClient?.Dispose();
    }
}

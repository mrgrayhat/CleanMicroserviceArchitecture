using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SaeedRezayi.Api;
using SaeedRezayi.LogModule.Models;
using SaeedRezayi.XuntiTestLayer.Base;
using Xunit;

namespace SaeedRezayi.XuntiTestLayer
{
    public class LogSystemTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public LogSystemTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }


        [Theory]
        [InlineData("/api/Logs", 10)]
        [InlineData("/api/Logs?maxRecords=50", 50)]
        public async Task Get_Logs_ReturnLatestLogsAndSuccess(string url, int maxRecords = 10)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                //AllowAutoRedirect = false,
                HandleCookies = true
            });

            // Act
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode(); // Status Code 200-299
            // Assert
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            // Deserialize response
            string json = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                DateFormatHandling = DateFormatHandling.IsoDateFormat
            };

            PagedLogsListViewModel pagedLogsList;
            pagedLogsList = JsonConvert
                .DeserializeObject<PagedLogsListViewModel>(json, settings);

            // Assert
            Assert.True(pagedLogsList.Paging
                .TotalItems > 1);
            Assert.True(pagedLogsList.Logs
                .Count == maxRecords && pagedLogsList.Paging.ItemsPerPage == maxRecords);

        }
    }
}

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using SaeedRezayi.Api;
using SaeedRezayi.ViewModels.Blog;
using SaeedRezayi.XuntiTestLayer.Base;
using Xunit;

namespace SaeedRezayi.XuntiTestLayer
{
    /// <summary>
    /// All Unit's For Testing Blog System
    /// </summary>
    public class BlogModuleTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public BlogModuleTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        [Theory]
        [InlineData("/api/Blog", 10)]
        [InlineData("/api/Blog/GetPagedPosts?maxRecords=50", 50)]
        [InlineData("/api/Blog/GetPagedPosts?maxRecords=100", 100)]
        public async Task Get_Blog_Posts_ReturnPagedPosts_AndCorrectContentType(string url, int maxRecords)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                HandleCookies = true,
                AllowAutoRedirect = true
            });

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            var json = await response.Content.ReadAsStringAsync();
            PagedPostsListViewModel pagedPostsList = JsonSerializer
                .Deserialize<PagedPostsListViewModel>(json);

            Assert.True(pagedPostsList.Paging
                .TotalItems == Constants.ActualRecordsAmount);
            Assert.True(pagedPostsList.Posts
                .Count == maxRecords && pagedPostsList.Paging.ItemsPerPage == maxRecords);
        }

    }
}

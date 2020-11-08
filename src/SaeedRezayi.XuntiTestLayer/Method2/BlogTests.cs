using System.Threading.Tasks;
using Xunit;
using SaeedRezayi.Services.Contracts.Blog;
using DNTCommon.Web.Core;
using SaeedRezayi.Services.Contracts.Account;
using SaeedRezayi.ViewModels.Account;
using SaeedRezayi.Common.Messages;
using SaeedRezayi.ViewModels.Blog;
using System.Collections.Generic;

namespace SaeedRezayi.XuntiTestLayer
{
    public class BlogTests : TestsBase
    {
        //private readonly HttpClient _client;


        // public BlogTests()
        // {
        // var server = new TestServer(new WebHostBuilder()
        //     .UseEnvironment("Development")
        //     .UseStartup<Startup>());

        // _client = server.CreateClient();
        // }

        // [Theory]
        // [InlineData("Get")]
        // public async Task BlogGetAllPostsAsync(string method)
        // {
        //     var request = new HttpRequestMessage(new HttpMethod(method), "api/blog/");
        //     var response = await _client.SendAsync(request);
        //     response.EnsureSuccessStatusCode();
        //     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        // }

        [Theory]
        [InlineData(1)]
        public async Task TestFindExistPostAsync(int postId)
        {
            var post = new PostViewModel();
            await ServiceProvider.RunScopedServiceAsync<IBlogService>(async blogService =>
            {
                post = await blogService.FindPostAsync(postId);
                Assert.NotNull(post);
            });
        }

        [Theory]
        [InlineData(1, "test case post1", "test case content")]
        public async void TestAddGoodPostAsync(int userId, string title, string content)
        {
            var user = new UserViewModel();

            await ServiceProvider.RunScopedServiceAsync<IUsersService>(async usersService =>
            {
                user = await usersService.FindUserAsync(userId);
            });
            Assert.NotNull(user);

            var testPost = new PostViewModel
            {
                PostLocales = new List<PostLocaleViewModel>(2)
                {
                    new PostLocaleViewModel
                    {
                        Title = title,
                        Slug = "test-case-post1",
                        Content = content,
                        LocaleCultureId = 2
                    },
                    new PostLocaleViewModel
                    {
                        Title = title,
                        Slug = "مطلب-تست-واحد-1",
                        Content = content,
                        LocaleCultureId = 1
                    }
                },
                Author = user,
                IsPublic = true,
                Visits = 1
            };

            await ServiceProvider.RunScopedServiceAsync<IBlogService>(async blogService =>
            {
                var addResult = await blogService.AddPostAsync(testPost);
                Assert.True(addResult.StatusCode == MessageStatusCodeTypes.SUCCESS, addResult.Exception?.Message);

                var findPost = await blogService.FindPostAsync(testPost.Id);
                Assert.NotNull(findPost);
                Assert.Equal(findPost == testPost, findPost != testPost);
            });
        }
    }
}

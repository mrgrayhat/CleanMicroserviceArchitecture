using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
using SaeedRezayi.DomainClasses.Authentication;
using SaeedRezayi.Api;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace SaeedRezayi.XuntiTestLayer
{
    public class AccountTest
    {
        //private readonly HttpClient _client;
        private const string BaseAddress = "https://localhost:5001/";

        private static readonly HttpClientHandler _httpClientHandler = new HttpClientHandler
        {
            UseCookies = true,
            UseDefaultCredentials = true,
            CookieContainer = new CookieContainer()
        };
        private static HttpClient _httpClient = new HttpClient(_httpClientHandler)
        {
            BaseAddress = new Uri(BaseAddress)
        };

        public AccountTest()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());

            _httpClient = server.CreateClient();
        }

        [Theory]
        [InlineData("Get", "admin", "admin")]
        public async Task Account_Login_Success(string method, string username, string password)
        {
            string requestUri = "/api/account/login";
            var stringJson = $"{{username:{ username },password:{password}}}";
            var json = JsonConvert.SerializeObject(new UserInfo
            {
                Username = username,
                Password = password
            });
            Token token = new Token();
            var request = new HttpRequestMessage(new HttpMethod(method), requestUri);
            //var response = await _client.SendAsync(requestUri);
            //response.EnsureSuccessStatusCode();
            request.Content = new StringContent(json,
                                                Encoding.UTF8,
                                                "application/json");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();


            //var token = await response.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() });


            //var appCookies = GetAntiforgeryCookies();
            //return (token, appCookies);

            string apiResponse = await response.Content.ReadAsStringAsync();
            try
            {
                // Attempt to deserialise the reponse to the desired type, otherwise throw an expetion with the response from the api.
                if (apiResponse != "")
                    token = JsonConvert.DeserializeObject<Token>(apiResponse);
                else
                    throw new Exception();
            }
            catch
            {
                throw new Exception($"An error ocurred while calling the API. It responded with the following message: {response.StatusCode} {response.ReasonPhrase}");
            }
            Console.WriteLine($"Response    : {response}");
            Console.WriteLine($"AccessToken : {token.AccessToken}");
            Console.WriteLine($"RefreshToken: {token.RefreshToken}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public void Test1()

        {

        }

        public class Token
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
        }
    }
}

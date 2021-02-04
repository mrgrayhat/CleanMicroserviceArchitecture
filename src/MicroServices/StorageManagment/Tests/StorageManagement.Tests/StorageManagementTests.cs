using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace StorageManagement.Tests
{
    public class StorageManagementTests
    {

        public StorageManagementTests()
        {

        }


        [Fact]
        public async Task DownloadFile_ReturnSuccessAsync()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5003");


            using var stream = await client.GetStreamAsync("/Storage/Download/1");

        }


    }
}

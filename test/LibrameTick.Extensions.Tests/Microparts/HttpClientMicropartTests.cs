using System.Threading.Tasks;
using Xunit;

namespace Librame.Extensions.Microparts
{
    public class HttpClientMicropartTests
    {
        [Fact]
        public async Task AllTest()
        {
            var micropart = new HttpClientMicropart(new HttpClientOptions());
            var client = micropart.Unwrap();

            var content = await client.GetStringAsync("https://www.baidu.com");
            Assert.NotEmpty(content);
        }

    }
}

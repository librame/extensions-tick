using Xunit;

namespace Librame.Extensions.Microparts
{
    public class HttpClientMicropartTests
    {
        [Fact]
        public void AllTest()
        {
            var micropart = new HttpClientMicropart(new HttpClientOptions());
            var client = micropart.Unwrap();

            var content = client.GetStringAsync("https://www.baidu.com").Result;
            Assert.NotEmpty(content);
        }

    }
}

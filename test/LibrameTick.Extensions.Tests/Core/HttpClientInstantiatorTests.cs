using Xunit;

namespace Librame.Extensions.Core
{
    public class HttpClientInstantiatorTests
    {
        [Fact]
        public void AllTest()
        {
            var instantiator = new HttpClientInstantiator(new HttpClientOptions());
            var client = instantiator.Create();

            var content = client.GetStringAsync("https://www.baidu.com").Result;
            Assert.NotEmpty(content);
        }

    }
}

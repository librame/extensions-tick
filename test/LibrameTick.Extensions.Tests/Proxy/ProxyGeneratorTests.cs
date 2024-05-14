using Librame.Extensions.Infrastructure.Proxy;
using Xunit;

namespace Librame.Extensions.Proxy
{
    public class ProxyGeneratorTests
    {

        [Fact]
        public void AllTest()
        {
            IProxyGenerator proxy = new ProxyGenerator();
            ITestCreation source = new TestCreation();

            var decorator = proxy.CreateInterfaceProxyDecorator<ITestCreation, TestCreationInterceptor>(source);
            //var proxySource = proxy.CreateInterfaceProxy<ITestCreation, TestCreationInterceptor>(source);

            var name = nameof(Librame);
            var result = decorator.Only(o => o.Create(name));

            // CurrentName: Librame Peng
            Assert.StartsWith(name, decorator.CurrentSource.CurrentName);

            // result: Hello Librame!
            Assert.Contains(name, result);
        }

    }
}

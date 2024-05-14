using Librame.Extensions.Core;
using Librame.Extensions.Infrastructure.Proxy;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Proxy
{
    public class ProxyDecoratorInjectionTests
    {
        internal static string? PreActionMessage;

        internal static string? PostActionMessage;


        [Fact]
        public void AllTest()
        {
            // 拦截容器已注册实例
            var decorator = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IProxyDecorator<ITestProxyService>>();

            var name = decorator.ProxySource.GetName();
            Assert.NotNull(name);
            Assert.Contains("pre invoked", PreActionMessage);
            Assert.Contains("post invoked", PostActionMessage);

            PreActionMessage = null;
            PostActionMessage = null;

            name = decorator.Only(p => p.GetName());
            Assert.NotNull(name);
            Assert.Contains("pre invoked", PreActionMessage);
            Assert.Contains("post invoked", PostActionMessage);
        }

    }
}

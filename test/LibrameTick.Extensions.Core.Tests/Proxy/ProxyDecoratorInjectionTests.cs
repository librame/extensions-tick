using Librame.Extensions.Core;
using Librame.Extensions.Setting;
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
            var decorator = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<IProxyDecorator<ISettingProvider<TestSetting>>>();

            var info = decorator.ProxySource.Generate();
            Assert.NotNull(info);
            Assert.Contains("pre invoked", PreActionMessage);
            Assert.Contains("post invoked", PostActionMessage);

            PreActionMessage = null;
            PostActionMessage = null;

            info = decorator.Only(p => p.Generate());
            Assert.NotNull(info);
            Assert.Contains("pre invoked", PreActionMessage);
            Assert.Contains("post invoked", PostActionMessage);
        }

    }
}

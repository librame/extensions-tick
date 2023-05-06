using Microsoft.Extensions.Localization;
using System.Linq;
using System.Text;
using Xunit;

namespace Librame.Extensions.Plugins
{
    public class TestPluginResolverTests
    {
        [Fact]
        public void AllTest()
        {
            var resolver = new TestPluginResolver();
            var pluginInfo = resolver.ResolveInfos().First();
            var localizer = (IStringLocalizer<TestPluginResource>)pluginInfo.Localizer!;

            var localizedString = localizer.GetString(p => p.Message);
            Assert.False(localizedString.ResourceNotFound);
            Assert.NotEmpty(localizedString.Value);
        }

    }
}

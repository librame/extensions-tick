using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Librame.Extensions.Setting
{
    public class JsonFileSettingProviderTests
    {

        [Fact]
        public void AllTest()
        {
            var settings = CoreExtensionBuilderHelper.CurrentServices.GetRequiredService<ISettingValues<TestSetting>>();
            var value = settings.GetSingletonValue();
            Assert.NotNull(value);
        }

    }
}

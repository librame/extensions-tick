using Librame.Extensions.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Librame.Extensions.Setting
{
    public class JsonFileSettingProviderTests
    {

        [Fact]
        public void AllTest()
        {
            var defaultTime = TestJsonFileSettingProvider.DefaultTime;

            var provider = CoreExtensionBuilderHelper.CurrentServices
                .GetRequiredService<ISettingProvider<TestSetting>>();

            var setting = provider.CurrentSetting;

            Assert.Equal(TestJsonFileSettingProvider.DefaultId, setting.Id);
            Assert.Equal(nameof(TestSetting), setting.Name);
            Assert.Equal(defaultTime.LocalDateTime, setting.CreatedTime);
            Assert.Equal(defaultTime, setting.UpdatedTime);

            setting.Id = Guid.NewGuid().ToString();
            provider.SaveChanges();

            Assert.NotEqual(TestJsonFileSettingProvider.DefaultId, setting.Id);
        }

    }
}

using Librame.Extensions.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Setting
{
    public class TestSetting : ISetting
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTimeOffset? UpdatedTime { get; set; }
    }


    public class TestJsonFileSettingProvider : JsonSettingProvider<TestSetting>
    {
        public static readonly DateTimeOffset DefaultTime = DateTimeOffset.Now;
        public static readonly string DefaultId = Guid.NewGuid().ToString();


        public TestJsonFileSettingProvider(IOptionsMonitorCache<TestSetting> cache,
            IOptionsMonitor<CoreExtensionOptions> options)
            : base(cache, builder => ConfigureBuilder(builder, options.CurrentValue), PostConfiguration)
        {
        }


        private static void PostConfiguration(TestSetting setting)
        {
            setting.Id = DefaultId;
            setting.Name = nameof(TestSetting);
            setting.CreatedTime = DefaultTime.LocalDateTime;
            setting.UpdatedTime = DefaultTime;
        }

        private static void ConfigureBuilder(IConfigurationBuilder builder, CoreExtensionOptions options)
        {
            var fileName = "test_settings.json".SetBasePath(options.Directories.ResourceDirectory);

            builder.AddJsonFile(fileName)
                .SetBasePath(PathExtensions.CurrentDirectoryWithoutDevelopmentRelativeSubpath)
                .InitializeJsonFile(Generate());
        }


        private static TestSetting Generate()
        {
            var setting = new TestSetting();

            PostConfiguration(setting);

            return setting;
        }

    }
}

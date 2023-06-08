using Librame.Extensions.Proxy;
using Microsoft.Extensions.Logging;
using System;

namespace Librame.Extensions.Setting
{
    public class TestSetting : ISettingRoot
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTimeOffset? UpdatedTime { get; set; }
    }


    public class TestJsonFileSettingProvider : AbstractJsonFileSettingProvider<TestSetting>
    {
        public TestJsonFileSettingProvider(ILoggerFactory loggerFactory)
            : base(loggerFactory, "test_settings.json".SetBasePath())
        {
        }


        [TestInterception]
        public override TestSetting Generate()
        {
            return new TestSetting
            {
                Id = Guid.NewGuid().ToString(),
                Name = nameof(TestSetting),
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow.AddHours(1),
            };
        }

    }
}

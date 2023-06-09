using Librame.Extensions.Setting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    [Sharding("%m", typeof(ModShardingStrategy))] // 使用模运算分片策略
    public class TestShardingProperty : AbstractIdentifier<long>
    {
        public string? Name { get; set; }
    }


    public class ShardDescriptorTests
    {
        private readonly IShardingContext _context;
        private readonly IIdGeneratorFactory _idGenerator;


        public ShardDescriptorTests()
        {
            _context = DataExtensionBuilderHelper.CurrentServices.GetRequiredService<IShardingContext>();
            _idGenerator = DataExtensionBuilderHelper.CurrentServices.GetRequiredService<IIdGeneratorFactory>();
        }


        [Fact]
        public void ShardingValueTest()
        {
            // 用户实体已标注使用文化信息与时间联合分片（实体也已实现对应的分片值接口，不需要手动配置）
            var user = new User
            {
                Name = $"Test Name",
                Passwd = "123456",
                Id = _idGenerator.GetMongoIdGenerator().GenerateId()
            };

            var setting = _context.ShardTable(user);
            Assert.NotNull(setting.ShardedName);
            Assert.NotEqual(setting.BaseName, setting.ShardedName);
        }

        [Fact]
        public void ShardingPropertyTest()
        {
            // 手动配置实体分片属性
            var builder = new ShardingBuilder<TestShardingProperty>(_context);
            builder.HasProperty(p => p.Id); // 使用整数标识进行模运算分片

            // 准备实体数据
            var properties = ReadyProperties();
            var settings = new List<ShardingTableSetting>();

            foreach (var prop in properties)
            {
                var setting = _context.ShardTable(prop);
                Assert.NotNull(setting.ShardedName);

                settings.Add(setting);
            }

            Assert.NotEmpty(settings);
        }

        private static IEnumerable<TestShardingProperty> ReadyProperties()
        {
            for (var i = 1; i < 10; i++)
            {
                yield return new TestShardingProperty
                {
                    Id = i,
                    Name = $"{nameof(TestShardingProperty)}{i}",
                };
            }
        }

    }

}

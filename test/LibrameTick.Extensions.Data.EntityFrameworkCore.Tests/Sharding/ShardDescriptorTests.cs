using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    public class ShardDescriptorTests
    {

        [Fact]
        public void ShardingValueTest()
        {
            var attribute = ShardingAttribute.ParseFromEntity(typeof(User), defaultTableName: null);
            var shardingContext = DataExtensionBuilderHelper.CurrentServices.GetRequiredService<IShardingContext>();
            
            var user = new User();

            var descriptor = new ShardingDescriptor(attribute, shardingContext.StrategyProvider.GetStrategy);
            descriptor.FormatSuffix(user);

            var nowSuffix = descriptor.FormattedSuffix;
            Assert.NotNull(nowSuffix);

            user.CreatedTime = user.CreatedTime.AddMonths(-2);
            descriptor.FormatSuffix(user);

            Assert.NotEqual(nowSuffix, descriptor.FormattedSuffix);
        }

        [Fact]
        public void ShardingPropertyTest()
        {
            var shardingContext = DataExtensionBuilderHelper.CurrentServices.GetRequiredService<IShardingContext>();

            // 手动配置实体分片属性
            var shardingBuilder = new ShardingBuilder<TestShardingProperty>(shardingContext);
            shardingBuilder.HasProperty(p => p.Id); // 使用整数标识进行模运算分片

            // 准备实体数据
            var properties = ReadyProperties();
            var descriptors = new List<ShardingDescriptor>();

            foreach (var prop in properties)
            {
                shardingContext.ShardTable(prop, null, out var descriptor);
                Assert.NotNull(descriptor.FormattedSuffix);

                descriptors.Add(descriptor);
            }

            Assert.NotEmpty(descriptors);

            static IEnumerable<TestShardingProperty> ReadyProperties()
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


    [Sharding("%m", typeof(ModShardingStrategy))]
    public class TestShardingProperty : AbstractIdentifier<long>
    {
        public string? Name { get; set; }
    }

}

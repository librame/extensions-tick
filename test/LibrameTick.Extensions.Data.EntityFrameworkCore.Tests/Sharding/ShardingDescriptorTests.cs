using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Librame.Extensions.Data.Sharding
{
    [ShardingTable("%mod:d2s1_%dto:d6", typeof(ModShardingStrategy), typeof(DateTimeOffsetShardingStrategy))]
    public class TestSharding : AbstractIdentifier<long>, IShardingValue<long>, IShardingValue<DateTimeOffset>
    {
        public string? Name { get; set; }

        public long GetShardedValue(long defaultValue)
            => Id; // 使用 ID 标识进行模运算分片

        public DateTimeOffset GetShardedValue(DateTimeOffset defaultValue)
            => DateTimeOffset.UtcNow;

    }


    public class ShardingDescriptorTests
    {

        [Fact]
        public void GenerateShardingNameTest()
        {
            var strategyProvider = DataExtensionBuilderHelper.CurrentServices
                .GetRequiredService<IShardingStrategyProvider>();

            // baseTableName ??= entityType.Name
            var descriptor = ShardingTableAttribute.GetTable<TestSharding>(baseTableName: null)!
                .AsDescriptor(strategyProvider);

            var data = GetData(6).ToArray();
            var shardedNames = data.Select(descriptor.GenerateShardingName).ToArray();
            
            foreach (var shardedName in shardedNames)
            {
                Assert.StartsWith(descriptor.Attribute.BaseName, shardedName);
            }
        }

        private static IEnumerable<TestSharding> GetData(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var id = i + 2;

                yield return new TestSharding
                {
                    Name = $"TestName" + id,
                    Id = id
                };
            }
        }

    }

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Saving;

internal sealed class InternalShardingSavingBehavior : AbstractSavingBehavior
{
    internal IReadOnlyList<ShardingTableSetting>? SavingShardingTables { get; private set; }


    protected override void HandleCore(ISavingContext<BaseDbContext, EntityEntry> context)
    {
        SavingShardingTables = ParseShardingTables(context.DbContext.ShardingContext, context.ChangeEntries);

        if (SavingShardingTables is null)
            return;

        // 提取需要分表的项集合
        var shardingItems = new Dictionary<ShardingItemSetting, ShardingTableSetting>();

        foreach (var table in SavingShardingTables)
        {
            foreach (var item in table.Items)
            {
                if (item.IsNeedSharding)
                    shardingItems.Add(item, table);
            }
        }

        // 得到分表的类型名称集合
        var shardingTypeNames = shardingItems.Values.WhereNotNullBy(p => p.SourceType)
            .ToDictionary(s => s.SourceType!, s => s.Items.Select(i => (i.ShardedName, i.Source)));

        // 验证数据表是否存在

        // 创建所需分表类型与映射实例
        var shardingTypes = CreateShardingTableTypes(context.DbContext, shardingTypeNames);

        // 创建不存在的数据表
        // 1.创建临时分表类
        // 2.
    }


    private static IDictionary<Type, object> CreateShardingTableTypes(BaseDbContext context,
        IDictionary<Type, IEnumerable<(string shardedName, object? source)>> tableTypes)
    {
        // 创建临时分表类型并缓存数据以便保存到数据库
        var shardingAssemblyName = context.ContextType.Assembly.GetName().Name;
        shardingAssemblyName = $"{shardingAssemblyName}_Sharding_{DateTimeOffset.UtcNow.UtcTicks}";

        var shardingTypes = new Dictionary<Type, object>();

        var moduleBuilder = shardingAssemblyName.BuildModule(out _);

        foreach (var type in tableTypes)
        {
            foreach (var (shardedName, source) in type.Value)
            {
                var shardingType = moduleBuilder.CopyType(type.Key, shardedName);
                if (source is not null)
                {
                    var mapSource = ObjectMapper.NewByMapAllPublicProperties(source, shardingType);
                    shardingTypes.Add(shardingType, mapSource);
                }
            }
        }

        return shardingTypes;
    }

    private static List<ShardingTableSetting>? ParseShardingTables(IShardingContext shardingContext,
        IEnumerable<EntityEntry>? entityEntries)
    {
        if (entityEntries is null)
            return null;

        var tables = new List<ShardingTableSetting>();

        foreach (var entry in entityEntries)
        {
            var table = shardingContext.ShardTable(entry.Metadata.ClrType, entry.Entity);
            tables.Add(table);
        }

        return tables;
    }

}

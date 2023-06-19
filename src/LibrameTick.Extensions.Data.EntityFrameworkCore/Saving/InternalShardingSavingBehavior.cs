#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Saving;

internal sealed class InternalShardingSavingBehavior : AbstractSavingBehavior
{
    internal IReadOnlyList<ShardingTableSetting>? SavingShardingTables { get; private set; }


    protected override void HandleCore(ISavingContext<BaseDbContext, EntityEntry> context)
    {
        SavingShardingTables = ParseTables(context.DbContext.ShardingContext, context.ChangeEntries);

        if (SavingShardingTables is null)
            return;

        var pairs = new Dictionary<ShardingItemSetting, ShardingTableSetting>();

        foreach (var table in SavingShardingTables)
        {
            foreach (var item in table.Items)
            {
                if (item.IsNeedSharding)
                    pairs.Add(item, table);
            }
        }

        // 验证数据表是否存在

        CreateShardingTableType(pairs.Values.Select(s => s.SourceType));

        // 创建不存在的数据表
        // 1.创建临时分表类
        // 2.
    }


    private static List<Type> CreateShardingTableType(IEnumerable<Type?> tableTypes)
    {
        foreach(var type in tableTypes)
        {
            if (type is null)
                continue;

            // 创建临时类型并缓存数据以便保存到数据库
        }
    }

    private static List<ShardingTableSetting>? ParseTables(IShardingContext shardingContext,
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

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

        var descriptors = ParseShardingDescriptors(context.DbContext, SavingShardingTables);

        // 创建所需分表类型与映射对象
        var shardingTypes = CreateShardingTypes(context.DbContext, descriptors);

        // 将分表类型与映射对象附加到上下文
        AddContext(context.DbContext, shardingTypes);
    }


    private static void AddContext(BaseDbContext context,
        IDictionary<(Type Source, Type Sharded), List<ShardingSavingDescriptor>> shardingTypes)
    {
        if (context.Model is not Model model)
            return; // 如果不支持动态添加/移除分表类型，则直接退出

        var dbSetMethod = context.ContextType.GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!;

        foreach (var pair in shardingTypes)
        {
            // 注册分表类型
            model.CopyEntityType(pair.Key.Source, pair.Key.Sharded);

            // 添加分表类型对象
            var shardingSet = dbSetMethod.MakeGenericMethod(pair.Key.Sharded).Invoke(context, null)!;
            var shardingSetType = shardingSet.GetType();

            var addParamTypes = new Type[] { pair.Key.Sharded };
            var shardingAddMethod = shardingSetType.GetMethod(nameof(DbSet<ShardingItemSetting>.Add), addParamTypes)!;

            foreach (var descr in pair.Value)
            {
                shardingAddMethod.Invoke(shardingSet, new object[] { descr.Sharded! });
            }

            // 移除旧来源类型对象
            var sourceTypes = pair.Value.GroupBy(static s => s.SourceType);
            foreach (var sourceType in sourceTypes)
            {
                var sourceSet = dbSetMethod.MakeGenericMethod(sourceType.Key).Invoke(context, null)!;
                var sourceSetType = sourceSet.GetType();

                var removeRangeParamTypes = new Type[] { typeof(IEnumerable<>).MakeGenericType(new Type[] { sourceType.Key }) };
                var sourceRemoveMethod = shardingSetType.GetMethod(nameof(DbSet<ShardingItemSetting>.RemoveRange), removeRangeParamTypes)!;

                var removeEntities = sourceType.Select(static s => s.Source).ToArray();
                sourceRemoveMethod.Invoke(sourceSet, removeEntities);
            }
        }
    }

    private static IDictionary<(Type Source, Type Sharded), List<ShardingSavingDescriptor>> CreateShardingTypes(
        BaseDbContext context, IDictionary<string, List<ShardingSavingDescriptor>> descriptors)
    {
        var typePairs = new Dictionary<(Type Source, Type Sharded), List<ShardingSavingDescriptor>>(
            PropertyEqualityComparer<(Type Source, Type Sharded)>.Create(p => p.Sharded.FullName!));

        // 创建临时分表类型并缓存数据以便保存到数据库
        var shardingAssemblyName = context.ContextType.Assembly.GetName().Name;
        shardingAssemblyName = $"{shardingAssemblyName}_Sharding_{DateTimeOffset.UtcNow.UtcTicks}";

        var moduleBuilder = shardingAssemblyName.BuildModule(out _);

        foreach (var pair in descriptors)
        {
            foreach (var descr in pair.Value)
            {
                KeyValuePair<(Type Source, Type Sharded), List<ShardingSavingDescriptor>>? currentTypePair = null;

                foreach (var typePair in typePairs)
                {
                    if (typePair.Key.Sharded.FullName!.Equals(descr.ShardedName, StringComparison.Ordinal))
                    {
                        currentTypePair = typePair;
                        break;
                    }
                }

                if (currentTypePair is null)
                {
                    var shardingType = moduleBuilder.CopyType(descr.SourceType, descr.ShardedName);
                    var sharded = ObjectMapper.NewByMapAllPublicProperties(descr.Source, shardingType);

                    typePairs.Add((descr.SourceType, shardingType), new List<ShardingSavingDescriptor>
                    {
                        descr.ChangeSharded(sharded, shardingType)
                    });
                }
                else
                {
                    var shardingType = currentTypePair.Value.Key.Sharded;
                    var sharded = ObjectMapper.NewByMapAllPublicProperties(descr.Source, shardingType);

                    currentTypePair.Value.Value.Add(descr.ChangeSharded(sharded, shardingType));
                }
            }
        }

        return typePairs;
    }

    private static IDictionary<string, List<ShardingSavingDescriptor>> ParseShardingDescriptors(
        BaseDbContext context, IReadOnlyList<ShardingTableSetting> shardingTables)
    {
        // 解析实体配置的所需分表设置描述符集合
        var pairs = new Dictionary<string, List<ShardingSavingDescriptor>>();

        var dbModel = context.Model.GetRelationalModel();
        var dbTableNames = dbModel.Tables.Select(static s => s.Name).ToArray();

        foreach (var table in shardingTables)
        {
            if (table.SourceType is null)
                continue;

            foreach (var item in table.Items)
            {
                if (item.IsNeedSharding && !dbTableNames.Contains(item.ShardedName))
                {
                    var descriptor = new ShardingSavingDescriptor(item.ShardedName, item.Source!, table.SourceType);
                    if (pairs.TryGetValue(item.ShardedName, out var value))
                    {
                        value.Add(descriptor);
                    }
                    else
                    {
                        pairs.Add(item.ShardedName, new List<ShardingSavingDescriptor> { descriptor });
                    }
                }
            }
        }

        return pairs;
    }

    private static List<ShardingTableSetting>? ParseShardingTables(IShardingContext shardingContext,
        IEnumerable<EntityEntry>? entityEntries)
    {
        if (entityEntries is null || !entityEntries.Any())
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

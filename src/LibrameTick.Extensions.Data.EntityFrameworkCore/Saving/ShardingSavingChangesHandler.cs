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

/// <summary>
/// 定义继承 <see cref="AbstractSavingChangesHandler"/> 的分片保存变化处理程序。
/// </summary>
public sealed class ShardingSavingChangesHandler : AbstractSavingChangesHandler
{

    /// <summary>
    /// 预处理保存上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="ISavingChangesContext"/>。</param>
    protected override void PreHandlingCore(ISavingChangesContext context)
    {
        var descriptorSettings = context.DataContext.CurrentServices.
            ShardingContext.ShardingTables(context.DataContext);

        if (descriptorSettings is null) return;

        // 创建所需分表类型与映射对象
        CreateShardedTypes(context.DataContext, descriptorSettings);

        // 将分表类型与映射对象重置到上下文
        ResetContext(context.DataContext, descriptorSettings);

        var model = context.DataContext.Model;
    }


    private static void ResetContext(DataContext context,
        Dictionary<ShardingDescriptor, List<ShardingItemSetting>> descriptorSettings)
    {
        if (context.Model is not Model model)
            return; // 如果不支持动态添加/移除分表类型，则直接退出

#pragma warning disable EF1001 // Internal EF Core API usage.

        var dbSetMethod = context.ContextType.GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!;
        var shardedEntityTypes = new List<EntityType>();

        foreach (var descriptor in descriptorSettings)
        {
            foreach (var setting in descriptor.Value)
            {
                var shardedType = setting.ShardedType!.UnderlyingSystemType;

                // 注册分表类型
                var shardedEntityType = model.CopyEntityType(descriptor.Key.SourceType, shardedType, shardedEntityTypes);
                if (shardedEntityType is not null)
                    shardedEntityTypes.Add(shardedEntityType);

                // 添加分表类型对象
                var shardedSet = dbSetMethod.MakeGenericMethod(shardedType).Invoke(context, null)!;
                var shardedSetType = shardedSet.GetType();

                var addParameterTypes = new Type[] { shardedType };
                var shardingAddMethod = shardedSetType.GetMethod(nameof(DbSet<ShardingItemSetting>.Add), addParameterTypes)!;

                shardingAddMethod.Invoke(shardedSet, new object[] { setting.Sharded! });

                // 移除旧来源类型对象
                var sourceSet = dbSetMethod.MakeGenericMethod(descriptor.Key.SourceType).Invoke(context, null)!;
                var sourceSetType = sourceSet.GetType();

                var removeParameterTypes = new Type[] { descriptor.Key.SourceType };
                var sourceRemoveMethod = sourceSetType.GetMethod(nameof(DbSet<ShardingItemSetting>.Remove), removeParameterTypes)!;

                sourceRemoveMethod.Invoke(sourceSet, new object[] { setting.Source! });
            }
        }

#pragma warning restore EF1001 // Internal EF Core API usage.

    }


    private static void CreateShardedTypes(DataContext context,
        Dictionary<ShardingDescriptor, List<ShardingItemSetting>> descriptorSettings)
    {
        // 创建临时分表类型并缓存数据以便保存到数据库
        var now = context.CurrentServices.CoreOptions.Clock.GetNow();

        var shardingAssemblyName = context.ContextType.Assembly.GetName().Name;
        shardingAssemblyName = $"{shardingAssemblyName}_Sharding_{now.Ticks}";

        var moduleBuilder = shardingAssemblyName.BuildModule(out _);

        foreach (var descriptor in descriptorSettings)
        {
            foreach (var setting in descriptor.Value)
            {
                var shardedType = moduleBuilder.CopyType(descriptor.Key.SourceType, setting.CurrentName);
                var sharded = ObjectMapper.NewByMapAllPublicProperties(setting.Source!, shardedType);

                setting.ChangeSharded(sharded, shardedType);
            }
        }
    }

    //private static IDictionary<string, List<ShardingSavingDescriptor>> ParseShardingDescriptors(
    //    DataContext context, IReadOnlyList<ShardingTableSetting> shardingTables)
    //{
    //    // 解析实体配置的所需分表设置描述符集合
    //    var pairs = new Dictionary<string, List<ShardingSavingDescriptor>>();

    //    var dbModel = context.Model.GetRelationalModel();
    //    var dbTableNames = dbModel.Tables.Select(static s => s.Name).ToArray();

    //    foreach (var table in shardingTables)
    //    {
    //        if (table.ShardedType is null)
    //            continue;

    //        foreach (var item in table.Items)
    //        {
    //            if (!dbTableNames.Contains(item.ThisShardedName))
    //            {
    //                var descriptor = new ShardingSavingDescriptor(item.ThisShardedName, item.Source!, table.ShardedType);
    //                if (pairs.TryGetValue(item.ThisShardedName, out var value))
    //                {
    //                    value.Add(descriptor);
    //                }
    //                else
    //                {
    //                    pairs.Add(item.ThisShardedName, new List<ShardingSavingDescriptor> { descriptor });
    //                }
    //            }
    //        }
    //    }

    //    return pairs;
    //}

    //private static List<ShardingTableSetting>? ParseShardingTables(IShardingContext shardingContext,
    //    IEnumerable<EntityEntry> entityEntries)
    //{
    //    if (!entityEntries.Any())
    //        return null;

    //    var tables = new List<ShardingTableSetting>();

    //    foreach (var entry in entityEntries)
    //    {
    //        var table = shardingContext.ShardTable(entry.Metadata.ClrType, entry.Entity);
    //        tables.Add(table);
    //    }

    //    return tables;
    //}

}

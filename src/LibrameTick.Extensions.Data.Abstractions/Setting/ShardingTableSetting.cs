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

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义分表设置。
/// </summary>
public class ShardingTableSetting : ISetting
{
    /// <summary>
    /// 分片设置集合。
    /// </summary>
    public List<ShardingSetting> Tables { get; set; } = [];


    /// <summary>
    /// 获取或创建分表设置。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分表名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <param name="createdAction">给定的已创建方法。</param>
    /// <returns>返回包含 <see cref="ShardingTableSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    public virtual (ShardingSetting tableSetting, ShardingItemSetting itemSetting) GetOrCreate(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source, Action? createdAction)
    {
        if (TryGet(descriptor.SourceType, out var table))
        {
            if (!table.TryGetItem(shardedName, out var item))
            {
                var lastName = table.Items.LastOrDefault()?.CurrentName;

                item = ShardingItemSetting.Create(shardedName, lastName, sourceId, source);
                table.Items.Add(item);

                createdAction?.Invoke();
            }

            return (table, item);
        }
        else
        {
            var item = ShardingItemSetting.Create(shardedName, lastName: null, sourceId, source);

            table = new ShardingSetting(descriptor);
            table.Items.Add(item);

            Tables.Add(table);

            createdAction?.Invoke();

            return (table, item);
        }
    }


    /// <summary>
    /// 尝试添加分表设置。
    /// </summary>
    /// <param name="table">给定的 <see cref="ShardingTableSetting"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAdd(ShardingSetting table)
    {
        if (Tables.Any(p => p.Equals(table)))
            return false;

        Tables.Add(table);
        return true;
    }

    /// <summary>
    /// 尝试获取指定实体类型的分表设置。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="result">输出 <see cref="ShardingSetting"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool TryGet(Type entityType, [MaybeNullWhen(false)] out ShardingSetting result)
    {
        result = Tables.SingleOrDefault(p => p.SourceType == entityType);
        return result is not null;
    }

}

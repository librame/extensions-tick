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
/// 定义分库设置。
/// </summary>
public class ShardingDatabaseSetting : ISetting
{
    /// <summary>
    /// 分片设置集合。
    /// </summary>
    public List<ShardingSetting> Databases { get; set; } = [];


    /// <summary>
    /// 获取或创建分库设置。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedName">给定的分库名称。</param>
    /// <param name="sourceId">给定的来源标识。</param>
    /// <param name="source">给定的来源（暂不支持持久化）。</param>
    /// <param name="createdAction">给定的已创建方法。</param>
    /// <returns>返回包含 <see cref="ShardingDatabaseSetting"/> 与 <see cref="ShardingItemSetting"/> 的元组。</returns>
    public virtual (ShardingSetting databaseSetting, ShardingItemSetting itemSetting) GetOrCreate(
        ShardingDescriptor descriptor, string shardedName, string? sourceId, object? source, Action? createdAction)
    {
        if (TryGet(descriptor.SourceType, out var database))
        {
            if (!database.TryGetItem(shardedName, out var item))
            {
                var lastName = database.Items.LastOrDefault()?.CurrentName;

                item = ShardingItemSetting.Create(shardedName, lastName, sourceId, source);
                database.Items.Add(item);

                createdAction?.Invoke();
            }

            return (database, item);
        }
        else
        {
            var item = ShardingItemSetting.Create(shardedName, lastName: null, sourceId, source);

            database = new ShardingSetting(descriptor);
            database.Items.Add(item);

            Databases.Add(database);

            createdAction?.Invoke();

            return (database, item);
        }
    }


    /// <summary>
    /// 添加分库设置。
    /// </summary>
    /// <param name="database">给定的 <see cref="ShardingSetting"/>。</param>
    /// <returns>返回是否添加的布尔值。</returns>
    public virtual bool TryAdd(ShardingSetting database)
    {
        if (Databases.Any(p => p.Equals(database)))
            return false;

        Databases.Add(database);
        return true;
    }

    /// <summary>
    /// 尝试获取指定数据上下文类型的分库设置。
    /// </summary>
    /// <param name="contextType">给定的数据上下文类型。</param>
    /// <param name="result">输出 <see cref="ShardingDatabaseSetting"/>。</param>
    /// <returns>返回是否存在的布尔值。</returns>
    public virtual bool TryGet(Type contextType, [NotNullWhen(true)] out ShardingSetting? result)
    {
        result = Databases.SingleOrDefault(p => p.SourceType == contextType);
        return result is not null;
    }

}

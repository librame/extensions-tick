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
public sealed class ShardingDatabaseSetting : AbstractShardingSetting
{
    /// <summary>
    /// 构造一个默认 <see cref="ShardingDatabaseSetting"/>。
    /// </summary>
    public ShardingDatabaseSetting()
    {
    }

    /// <summary>
    /// 使用 <see cref="ShardingDescriptor"/> 构造一个 <see cref="ShardingDatabaseSetting"/>。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    public ShardingDatabaseSetting(ShardingDescriptor descriptor)
        : base(descriptor)
    {
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="ShardingDatabaseSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    public ShardingDatabaseSetting(AbstractShardingSetting setting)
        : base(setting)
    {
    }


    ///// <summary>
    ///// 获取或新增分片项设置。
    ///// </summary>
    ///// <param name="shardedName">给定的分片名称。</param>
    ///// <param name="originalName">给定的原始名称。</param>
    ///// <param name="accessorId">给定的存取器标识。</param>
    ///// <param name="addAction">给定新增后的动作（可选）。</param>
    ///// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    //public virtual ShardingItemSetting GetOrAddItem(string shardedName, string originalName,
    //    string accessorId, Action? addAction = null)
    //{
    //    if (!TryGetItem(shardedName, out var item))
    //    {
    //        item = ShardingItemSetting.Create(shardedName, originalName, accessorId);
    //        Items.Add(item);

    //        addAction?.Invoke();
    //    }
    //    //else
    //    //{
    //    //    item.IsNeedSharding = false;
    //    //}

    //    return item;
    //}


    ///// <summary>
    ///// 使用分片描述符创建分库设置。
    ///// </summary>
    ///// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    ///// <param name="shardedName">给定的分片名称。</param>
    ///// <param name="originalName">给定的原始名称。</param>
    ///// <param name="connectionString">给定的连接字符串。</param>
    ///// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
    //public static ShardingDatabaseSetting Create(ShardingDescriptor descriptor,
    //    string shardedName, string originalName, string connectionString)
    //    => Create(descriptor, shardedName, originalName, connectionString, out _);

    ///// <summary>
    ///// 使用分片描述符创建分库设置。
    ///// </summary>
    ///// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    ///// <param name="shardedName">给定的分片名称。</param>
    ///// <param name="lastName">给定的原始名称。</param>
    ///// <param name="connectionString">给定的连接字符串。</param>
    ///// <param name="result">输出 <see cref="ShardingItemSetting"/>。</param>
    ///// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
    //public static ShardingDatabaseSetting Create(ShardingDescriptor descriptor,
    //    string shardedName, string lastName, string connectionString, out ShardingItemSetting result)
    //{
    //    var setting = new ShardingDatabaseSetting(descriptor);

    //    result = ShardingItemSetting.Create(shardedName, lastName, connectionString);
    //    setting.Items.Add(result);

    //    return setting;
    //}

}

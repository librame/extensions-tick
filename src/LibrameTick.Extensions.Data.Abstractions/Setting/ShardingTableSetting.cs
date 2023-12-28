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
public sealed class ShardingTableSetting : AbstractShardingSetting
{
    /// <summary>
    /// 构造一个默认 <see cref="ShardingTableSetting"/>。
    /// </summary>
    public ShardingTableSetting()
    {
    }

    /// <summary>
    /// 使用 <see cref="ShardingDescriptor"/> 构造一个 <see cref="ShardingTableSetting"/>。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    public ShardingTableSetting(ShardingDescriptor descriptor)
        : base(descriptor)
    {
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="ShardingTableSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    public ShardingTableSetting(AbstractShardingSetting setting)
        : base(setting)
    {
    }


    ///// <summary>
    ///// 获取或新增分片项设置。
    ///// </summary>
    ///// <param name="shardedName">给定的分片名称。</param>
    ///// <param name="originalName">给定的原始名称。</param>
    ///// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
    ///// <param name="addAction">给定新增后的动作（可选）。</param>
    ///// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    //public ShardingItemSetting GetOrAddItem(string shardedName, string originalName,
    //    IObjectIdentifier? identifier, Action? addAction = null)
    //{
    //    if (!TryGetItem(shardedName, out var item))
    //    {
    //        item = ShardingItemSetting.Create(shardedName, originalName, identifier?.GetObjectId().ToString());
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
    ///// 使用分片描述符创建分表设置。
    ///// </summary>
    ///// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    ///// <param name="shardedName">给定的分片名称。</param>
    ///// <param name="originalName">给定的原始名称。</param>
    ///// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
    ///// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    //public static ShardingTableSetting Create(ShardingDescriptor descriptor,
    //    string shardedName, string originalName, IObjectIdentifier? identifier)
    //    => Create(descriptor, shardedName, originalName, identifier, out _);

    ///// <summary>
    ///// 使用分片描述符创建分表设置。
    ///// </summary>
    ///// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    ///// <param name="shardedName">给定的分片名称。</param>
    ///// <param name="originalName">给定的原始名称。</param>
    ///// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
    ///// <param name="result">输出 <see cref="ShardingItemSetting"/>。</param>
    ///// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    //public static ShardingTableSetting Create(ShardingDescriptor descriptor,
    //    string shardedName, string originalName, IObjectIdentifier? identifier, out ShardingItemSetting result)
    //{
    //    var setting = new ShardingTableSetting(descriptor);

    //    result = ShardingItemSetting.Create(shardedName, originalName, identifier?.GetObjectId().ToString());
    //    setting.Items.Add(result);

    //    return setting;
    //}

}

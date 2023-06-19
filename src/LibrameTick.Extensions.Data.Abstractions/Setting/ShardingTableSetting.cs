#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义分表设置。
/// </summary>
public class ShardingTableSetting : AbstractShardingSetting
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
    protected ShardingTableSetting(ShardingDescriptor descriptor)
        : base(descriptor)
    {
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="ShardingTableSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    protected ShardingTableSetting(AbstractShardingSetting setting)
        : base(setting)
    {
    }


    /// <summary>
    /// 获取或新增分片项设置。
    /// </summary>
    /// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <param name="addAction">给定新增后的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    public virtual ShardingItemSetting GetOrAddItem(IObjectIdentifier? identifier, string shardedName,
        Action? addAction = null)
    {
        if (!TryGetItem(shardedName, out var item))
        {
            item = CreateItem(identifier, shardedName);
            Items.Add(item);

            addAction?.Invoke();
        }
        else
        {
            item.IsNeedSharding = false;
        }

        return item;
    }


    /// <summary>
    /// 使用分片描述符创建分表设置。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <param name="result">输出 <see cref="ShardingItemSetting"/>。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting Create(ShardingDescriptor descriptor,
        IObjectIdentifier? identifier, string shardedName, out ShardingItemSetting result)
    {
        var setting = new ShardingTableSetting(descriptor);

        result = CreateItem(identifier, shardedName);
        setting.Items.Add(result);

        return setting;
    }

    /// <summary>
    /// 创建分库项设置。
    /// </summary>
    /// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    public static ShardingItemSetting CreateItem(IObjectIdentifier? identifier, string shardedName)
    {
        return new ShardingItemSetting
        {
            IsNeedSharding = true,
            ShardedName = shardedName,
            SourceId = identifier?.GetObjectId().ToString() // 分库使用标识符作用引用标识
        };
    }

}

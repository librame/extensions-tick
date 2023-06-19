#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Setting;

/// <summary>
/// 定义分库设置。
/// </summary>
public class ShardingDatabaseSetting : AbstractShardingSetting
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
    protected ShardingDatabaseSetting(ShardingDescriptor descriptor)
        : base(descriptor)
    {
    }

    /// <summary>
    /// 使用 <see cref="AbstractShardingSetting"/> 构造一个 <see cref="ShardingDatabaseSetting"/>。
    /// </summary>
    /// <param name="setting">给定的 <see cref="AbstractShardingSetting"/>。</param>
    protected ShardingDatabaseSetting(AbstractShardingSetting setting)
        : base(setting)
    {
    }


    /// <summary>
    /// 获取或新增分片项设置。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <param name="addAction">给定新增后的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    public virtual ShardingItemSetting GetOrAddItem(IAccessor accessor, string shardedName,
        Action? addAction = null)
    {
        if (!TryGetItem(shardedName, out var item))
        {
            item = CreateItem(accessor, shardedName);
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
    /// 使用分片描述符创建分库设置。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <param name="result">输出 <see cref="ShardingItemSetting"/>。</param>
    /// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
    public static ShardingDatabaseSetting Create(ShardingDescriptor descriptor,
        IAccessor accessor, string shardedName, out ShardingItemSetting result)
    {
        var setting = new ShardingDatabaseSetting(descriptor);

        result = CreateItem(accessor, shardedName);
        setting.Items.Add(result);

        return setting;
    }

    /// <summary>
    /// 创建分库项设置。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <returns>返回 <see cref="ShardingItemSetting"/>。</returns>
    public static ShardingItemSetting CreateItem(IAccessor accessor, string shardedName)
    {
        return new ShardingItemSetting
        {
            IsNeedSharding = true,
            ShardedName = shardedName,
            SourceId = accessor.AccessorId // 分库使用访问器标识作用引用标识
        };
    }

}

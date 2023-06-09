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
    /// 使用分片描述符创建分库设置。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
    public static ShardingDatabaseSetting Create(ShardingDescriptor descriptor,
        IAccessor accessor, string shardedName)
    {
        var setting = new ShardingDatabaseSetting(descriptor)
        {
            ShardedName = shardedName,
            SourceId = accessor.AccessorId // 分库使用访问器标识作用引用标识
        };

        return setting;
    }

}

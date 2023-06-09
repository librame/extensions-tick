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
    /// 使用分片描述符创建分表设置。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="identifier">给定的 <see cref="IObjectIdentifier"/>。</param>
    /// <param name="shardedName">给定的分片名称。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    public static ShardingTableSetting Create(ShardingDescriptor descriptor,
        IObjectIdentifier? identifier, string shardedName)
    {
        var setting = new ShardingTableSetting(descriptor)
        {
            ShardedName = shardedName,
            SourceId = identifier?.GetObjectId().ToString()
        };

        return setting;
    }

}

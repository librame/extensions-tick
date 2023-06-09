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
using Librame.Extensions.Dispatchers;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义一个分片上下文接口。
/// </summary>
public interface IShardingContext
{
    /// <summary>
    /// 调度器工厂。
    /// </summary>
    IDispatcherFactory DispatcherFactory { get; }

    /// <summary>
    /// 设置提供程序。
    /// </summary>
    IShardingSettingProvider SettingProvider { get; }

    /// <summary>
    /// 策略提供程序。
    /// </summary>
    IShardingStrategyProvider StrategyProvider { get; }

    /// <summary>
    /// 对象跟踪器。
    /// </summary>
    IShardingTracker Tracker { get; }


    /// <summary>
    /// 对存取器的数据库分片。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
    ShardingDatabaseSetting ShardDatabase(IAccessor accessor, out ShardingDescriptor descriptor,
        Action<ShardingDescriptor, ShardingDatabaseSetting>? shardedAction = null);

    /// <summary>
    /// 对实体的数据表分片。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <param name="defaultTableName">给定的默认表名。</param>
    /// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    /// <param name="shardedAction">给定已分片的动作（可选）。</param>
    /// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    ShardingTableSetting ShardTable(Type entityType, object? entity, string? defaultTableName,
        out ShardingDescriptor descriptor, Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null);

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatching;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义一个分片上下文接口。
/// </summary>
public interface IShardingContext
{
    /// <summary>
    /// 分片查找器。
    /// </summary>
    IShardingFinder Finder { get; }

    /// <summary>
    /// 设置提供程序。
    /// </summary>
    IShardingSettingManager SettingManager { get; }

    /// <summary>
    /// 策略提供程序。
    /// </summary>
    IShardingStrategyProvider StrategyProvider { get; }

    /// <summary>
    /// 调度器工厂。
    /// </summary>
    IDispatcherFactory DispatcherFactory { get; }


    /// <summary>
    /// 初始化数据上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    IShardingContext Initialize(IDataContext context);


    /// <summary>
    /// 对存取器分库。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    /// <returns>返回分库的连接字符串。</returns>
    string? ShardingDatabase(IDataContext context);

    /// <summary>
    /// 对存取器的表集合分表。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    /// <returns>返回需要分表的字典集合。</returns>
    Dictionary<ShardingDescriptor, List<ShardingItemSetting>>? ShardingTables(IDataContext context);


    ///// <summary>
    ///// 对实体的数据表分片。
    ///// </summary>
    ///// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    ///// <param name="entityType">给定的实体类型。</param>
    ///// <param name="entity">给定的实体对象。</param>
    ///// <param name="originalName">给定的原始名称。</param>
    ///// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    ///// <param name="shardedAction">给定已分片的动作（可选）。</param>
    ///// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    //ShardingTableSetting ShardTable(IAccessor accessor, Type entityType, object? entity, string originalName,
    //    out ShardingDescriptor descriptor, Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null);

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义一个分片管理器接口。
/// </summary>
public interface IShardingManager
{
    /// <summary>
    /// 调度器工厂。
    /// </summary>
    IDispatcherFactory DispatcherFactory { get; }


    #region ShardingEntity

    /// <summary>
    /// 增加或设置分片实体。
    /// </summary>
    /// <param name="entity">给定的 <see cref="ShardingEntity"/>。</param>
    /// <returns>返回 <see cref="IShardingManager"/>。</returns>
    IShardingManager AddOrSetEntity(ShardingEntity entity);

    /// <summary>
    /// 获取指定实体键的分片实体。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    ShardingEntity GetEntity(Type entityType);

    #endregion


    #region IShardingStrategy

    /// <summary>
    /// 增加指定类型的分片策略。
    /// </summary>
    /// <param name="strategy">给定的 <see cref="IShardingStrategy"/>。</param>
    /// <returns>返回 <see cref="IShardingManager"/>。</returns>
    IShardingManager AddStrategy(IShardingStrategy strategy);

    /// <summary>
    /// 获取指定类型的分片策略。
    /// </summary>
    /// <param name="strategyType">给定的策略类型。</param>
    /// <returns>返回 <see cref="IShardingStrategy"/>。</returns>
    IShardingStrategy GetStrategy(Type strategyType);

    #endregion

}

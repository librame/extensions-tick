#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义抽象实现 <see cref="IShardingManager"/> 的分片管理器。
/// </summary>
public abstract class AbstractShardingManager : IShardingManager
{
    private readonly ConcurrentDictionary<Type, ShardingEntity> _entities;
    private readonly ConcurrentDictionary<Type, IShardingStrategy> _strategies;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingManager"/>。
    /// </summary>
    protected AbstractShardingManager()
    {
        _entities = new();
        _strategies = new();
    }


    #region ShardingEntity

    /// <summary>
    /// 增加或设置分片实体。
    /// </summary>
    /// <param name="entity">给定的 <see cref="ShardingEntity"/>。</param>
    /// <returns>返回 <see cref="IShardingManager"/>。</returns>
    public virtual IShardingManager AddOrSetEntity(ShardingEntity entity)
    {
        _entities.AddOrUpdate(entity.EntityType, entity,
            (key, value) => value.AddOrSetProperties(entity.Properties));
        return this;
    }

    /// <summary>
    /// 获取指定实体键的分片实体。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <returns>返回 <see cref="ShardingEntity"/>。</returns>
    public virtual ShardingEntity GetEntity(Type entityType)
        => _entities[entityType];

    #endregion


    #region IShardingStrategy

    /// <summary>
    /// 增加指定类型的分片策略。
    /// </summary>
    /// <param name="strategy">给定的 <see cref="IShardingStrategy"/>。</param>
    /// <returns>返回 <see cref="IShardingManager"/>。</returns>
    public virtual IShardingManager AddStrategy(IShardingStrategy strategy)
    {
        _strategies.AddOrUpdate(strategy.StrategyType, strategy, (key, value) => strategy);
        return this;
    }

    /// <summary>
    /// 获取指定类型的分片策略。
    /// </summary>
    /// <param name="strategyType">给定的策略类型。</param>
    /// <returns>返回 <see cref="IShardingStrategy"/>。</returns>
    public virtual IShardingStrategy GetStrategy(Type strategyType)
        => _strategies[strategyType];

    #endregion

}

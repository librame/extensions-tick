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
/// 定义抽象实现 <see cref="IShardingStrategyProvider"/> 的分片策略提供程序。
/// </summary>
public abstract class AbstractShardingStrategyProvider : IShardingStrategyProvider
{
    private readonly ConcurrentDictionary<Type, IShardingStrategy> _strategies;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingStrategyProvider"/>。
    /// </summary>
    protected AbstractShardingStrategyProvider()
    {
        _strategies = new();
    }


    /// <summary>
    /// 增加指定类型的分片策略。
    /// </summary>
    /// <param name="strategy">给定的 <see cref="IShardingStrategy"/>。</param>
    /// <returns>返回 <see cref="IShardingStrategyProvider"/>。</returns>
    public virtual IShardingStrategyProvider AddStrategy(IShardingStrategy strategy)
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

}

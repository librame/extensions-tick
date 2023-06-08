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
/// 定义分片策略提供程序接口。
/// </summary>
public interface IShardingStrategyProvider
{
    /// <summary>
    /// 增加指定类型的分片策略。
    /// </summary>
    /// <param name="strategy">给定的 <see cref="IShardingStrategy"/>。</param>
    /// <returns>返回 <see cref="IShardingStrategyProvider"/>。</returns>
    IShardingStrategyProvider AddStrategy(IShardingStrategy strategy);

    /// <summary>
    /// 获取指定类型的分片策略。
    /// </summary>
    /// <param name="strategyType">给定的策略类型。</param>
    /// <returns>返回 <see cref="IShardingStrategy"/>。</returns>
    IShardingStrategy GetStrategy(Type strategyType);
}

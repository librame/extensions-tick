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
/// 定义可用于分库分表的分片管理器接口。
/// </summary>
public interface IShardingManager
{
    /// <summary>
    /// 获取指定策略类型的分片策略。
    /// </summary>
    /// <param name="strategyType">给定的策略类型。</param>
    /// <returns>返回 <see cref="IShardingStrategy"/>。</returns>
    IShardingStrategy? GetStrategy(Type strategyType);
}

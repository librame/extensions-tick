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
/// 定义 <see cref="ShardingDescriptor"/> 静态扩展。
/// </summary>
public static class ShardingDescriptorExtensions
{

    /// <summary>
    /// 转为分片描述符。
    /// </summary>
    /// <param name="attribute">给定的 <see cref="ShardingAttribute"/>。</param>
    /// <param name="strategyProvider">给定的 <see cref="IShardingStrategyProvider"/>。</param>
    /// <param name="initialShardingValues">给定的初始 <see cref="IReadOnlyList{IShardingValue}"/>（可选）。</param>
    /// <returns>返回 <see cref="ShardingDescriptor"/>。</returns>
    public static ShardingDescriptor AsDescriptor(this ShardingAttribute attribute,
        IShardingStrategyProvider strategyProvider, IReadOnlyList<IShardingValue>? initialShardingValues = null)
        => new(strategyProvider, attribute, initialShardingValues);

}

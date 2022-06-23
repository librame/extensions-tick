#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义一个分片属性。
/// </summary>
public class ShardingProperty : IEquatable<ShardingProperty>
{
    /// <summary>
    /// 构造一个 <see cref="ShardingProperty"/>。
    /// </summary>
    /// <param name="key">给定的属性键。</param>
    /// <param name="strategy">给定的 <see cref="IShardingStrategy"/>。</param>
    public ShardingProperty(TypeNamedKey key, IShardingStrategy strategy)
    {
        Key = key;
        Strategy = strategy;
    }


    /// <summary>
    /// 属性键。
    /// </summary>
    public TypeNamedKey Key { get; init; }

    /// <summary>
    /// 分片策略。
    /// </summary>
    public IShardingStrategy Strategy { get; init; }


    /// <summary>
    /// 是否相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="ShardingProperty"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(ShardingProperty? other)
        => Key == other?.Key;

}

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
/// 定义一个实现 <see cref="IShardingValue"/> 的泛型分片值接口。
/// </summary>
/// <typeparam name="TValue">指定的值类型。</typeparam>
public interface IShardingValue<TValue> : IShardingValue
{
    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <param name="defaultValue">给定的默认值（主要做标记用，可支持单类实现多个接口）。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    TValue GetShardedValue(TValue? defaultValue);
}


/// <summary>
/// 标记一个分片值接口。
/// </summary>
public interface IShardingValue : IEquatable<IShardingValue>
{
    /// <summary>
    /// 类型相等。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="IShardingValue"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    bool IEquatable<IShardingValue>.Equals(IShardingValue? other)
        => other?.GetType() == GetType();
}

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
/// 定义一个实现 <see cref="IShardingValue{TValue}"/> 的泛型单个分片值。
/// </summary>
/// <typeparam name="TValue">指定的值类型。</typeparam>
/// <remarks>
/// 构造一个 <see cref="SingleShardingValue{TValue}"/>。
/// </remarks>
/// <param name="valueFactory">给定的值工厂方法。</param>
public sealed class SingleShardingValue<TValue>(Func<TValue> valueFactory) : IShardingValue<TValue>
{
    private readonly Func<TValue> _valueFactory = valueFactory;


    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    public TValue GetShardedValue(TValue? defaultValue)
        => _valueFactory();

}

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
public class SingleShardingValue<TValue> : IShardingValue<TValue>
{
    private readonly TValue _initialValue;


    /// <summary>
    /// 构造一个 <see cref="SingleShardingValue{TValue}"/>。
    /// </summary>
    /// <param name="initialValue">给定的初始值。</param>
    public SingleShardingValue(TValue initialValue)
    {
        _initialValue = initialValue;
    }


    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    public virtual TValue GetShardedValue(TValue? defaultValue)
        => _initialValue;

}

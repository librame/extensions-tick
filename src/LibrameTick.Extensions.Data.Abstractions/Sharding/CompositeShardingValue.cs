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
/// 定义一个实现 <see cref="IShardingValue"/> 的复合分片值。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="CompositeShardingValue"/>。
/// </remarks>
/// <param name="shardingValues">给定的 <see cref="IEnumerable{IShardingValue}"/>。</param>
public sealed class CompositeShardingValue(IEnumerable<IShardingValue> shardingValues) : IShardingValue, IComposable<IShardingValue>
{
    private readonly IEnumerable<IShardingValue> _shardingValues = shardingValues;


    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <typeparamref name="TValue"/>。</returns>
    /// <exception cref="NotImplementedException">
    /// Not implemented sharding value type.
    /// </exception>
    public TValue GetShardedValue<TValue>(TValue? defaultValue)
    {
        foreach (var value in _shardingValues)
        {
            if (value is IShardingValue<TValue> current)
                return current.GetShardedValue(defaultValue);
        }

        throw new NotImplementedException($"Not implemented sharding value type '{typeof(IShardingValue<TValue>)}'.");
    }


    #region IComposable<IShardingValue>

    /// <summary>
    /// 复合的元素数。
    /// </summary>
    public int Count
        => _shardingValues.Count();

    /// <summary>
    /// 获取枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{IShardingValue}"/>。</returns>
    public IEnumerator<IShardingValue> GetEnumerator()
        => _shardingValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    #endregion

}

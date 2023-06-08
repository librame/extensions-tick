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
/// 定义抽象实现 <see cref="IShardingStrategy"/> 的泛型分片策略。
/// </summary>
/// <typeparam name="TValue">指定的分片值类型。</typeparam>
public abstract class AbstractShardingStrategy<TValue> : IShardingStrategy<TValue>, IEnumerable<ShardingStrategyParameter<TValue>>
{
    private readonly ConcurrentDictionary<string, ShardingStrategyParameter<TValue>> _parameters;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingStrategy{TValue}"/>。
    /// </summary>
    /// <param name="defaultValueFactory">给定的默认值工厂方法。</param>
    protected AbstractShardingStrategy(Func<TValue> defaultValueFactory)
    {
        _parameters = new();

        DefaultValue = new Lazy<TValue>(defaultValueFactory);
    }


    /// <summary>
    /// 默认值。
    /// </summary>
    public Lazy<TValue> DefaultValue { get; init; }

    /// <summary>
    /// 所有键名集合。
    /// </summary>
    public ICollection<string> AllKeys
        => _parameters.Keys;

    /// <summary>
    /// 获取指定键名的参数。
    /// </summary>
    /// <param name="key">给定的键名。</param>
    /// <returns>返回 <see cref="ShardingStrategyParameter{TValue}"/>。</returns>
    public ShardingStrategyParameter<TValue> this[string key]
        => _parameters[key];

    /// <summary>
    /// 获取指定索引的参数。
    /// </summary>
    /// <param name="index">给定的索引。</param>
    /// <returns>返回 <see cref="ShardingStrategyParameter{TValue}"/>。</returns>
    public ShardingStrategyParameter<TValue> this[int index]
        => _parameters.Values.ElementAt(index);


    /// <summary>
    /// 参数比较（默认区分大小写）。
    /// </summary>
    protected virtual StringComparison ParameterComparison
        => StringComparison.Ordinal;


    /// <summary>
    /// 添加参数。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    /// <param name="valueFormatter">给定的值格式化器。</param>
    /// <param name="defaultValue">给定的默认值（可选；默认为 <see cref="string.Empty"/>）。</param>
    /// <param name="keyIndicator">给定的键指示符（可选；默认为 <see cref="ShardingStrategyParameter{TValue}.DefaultKeyIndicator"/>）。</param>
    protected void AddParameter(string name, Func<TValue, string> valueFormatter,
        string? defaultValue = null, string? keyIndicator = null)
        => AddParameter(new ShardingStrategyParameter<TValue>(name, valueFormatter, defaultValue, keyIndicator));

    /// <summary>
    /// 添加参数。
    /// </summary>
    /// <param name="parameter">给定的 <see cref="ShardingStrategyParameter{TValue}"/>。</param>
    protected void AddParameter(ShardingStrategyParameter<TValue> parameter)
        => _parameters.AddOrUpdate(parameter.Key, parameter, (key, value) => parameter);


    /// <summary>
    /// 包含任何可格式化的键名。
    /// </summary>
    /// <param name="formatter">给定的格式化器。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool ContainsKey(string formatter)
        => AllKeys.Any(formatter.Contains);


    /// <summary>
    /// 格式化分片。
    /// </summary>
    /// <param name="formatter">给定的格式化器。</param>
    /// <param name="shardingValue">给定的 <see cref="IShardingValue"/>。</param>
    /// <returns>返回字符串。</returns>
    public virtual string Format(string formatter, IShardingValue? shardingValue)
    {
        if (!ContainsKey(formatter))
            return formatter;

        TValue? value = default;

        if (shardingValue is IShardingValue<TValue> realValue)
        {
            value = realValue.GetShardedValue(DefaultValue.Value);
        }
        else if (shardingValue is CompositeShardingValue realValues)
        {
            value = realValues.GetShardedValue(DefaultValue.Value);
        }

        return FormatCore(formatter, value ?? DefaultValue.Value);
    }

    /// <summary>
    /// 格式化分片核心。
    /// </summary>
    /// <param name="formatter">给定的格式化器。</param>
    /// <param name="value">给定的 <typeparamref name="TValue"/>。</param>
    /// <returns>返回字符串。</returns>
    protected virtual string FormatCore(string formatter, TValue value)
    {
        foreach (var p in _parameters)
        {
            formatter = formatter.Replace(p.Key, p.Value.ValueFormatter(value), ParameterComparison);

            if (formatter.Contains(p.Key))
                formatter = formatter.Replace(p.Key, p.Value.DefaultValue);
        }

        return formatter;
    }


    /// <summary>
    /// 获取参数枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{T}"/>。</returns>
    public IEnumerator<ShardingStrategyParameter<TValue>> GetEnumerator()
        => _parameters.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}

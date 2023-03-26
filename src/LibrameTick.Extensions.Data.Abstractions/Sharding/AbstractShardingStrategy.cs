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
public abstract class AbstractShardingStrategy<TValue> : IShardingStrategy, IEnumerable<ShardedStrategyParameter<TValue>>
{
    private readonly ConcurrentDictionary<string, ShardedStrategyParameter<TValue>> _parameters;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingStrategy{TValue}"/>。
    /// </summary>
    protected AbstractShardingStrategy()
    {
        _parameters = new();
    }


    /// <summary>
    /// 默认值。
    /// </summary>
    public abstract Lazy<TValue> DefaultValue { get; }

    /// <summary>
    /// 策略类型。
    /// </summary>
    public virtual Type StrategyType
        => GetType();

    /// <summary>
    /// 所有键名集合。
    /// </summary>
    public ICollection<string> AllKeys
        => _parameters.Keys;

    /// <summary>
    /// 获取指定键名的参数。
    /// </summary>
    /// <param name="key">给定的键名。</param>
    /// <returns>返回 <see cref="ShardedStrategyParameter{TValue}"/>。</returns>
    public ShardedStrategyParameter<TValue> this[string key]
        => _parameters[key];

    /// <summary>
    /// 获取指定索引的参数。
    /// </summary>
    /// <param name="index">给定的索引。</param>
    /// <returns>返回 <see cref="ShardedStrategyParameter{TValue}"/>。</returns>
    public ShardedStrategyParameter<TValue> this[int index]
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
    /// <param name="keyIndicator">给定的键指示符（可选；默认为 <see cref="ShardedStrategyParameter{TValue}.DefaultKeyIndicator"/>）。</param>
    protected void AddParameter(string name, Func<TValue, string> valueFormatter,
        string? defaultValue = null, string? keyIndicator = null)
        => AddParameter(new ShardedStrategyParameter<TValue>(name, valueFormatter, defaultValue, keyIndicator));

    /// <summary>
    /// 添加参数。
    /// </summary>
    /// <param name="parameter">给定的 <see cref="ShardedStrategyParameter{TValue}"/>。</param>
    protected void AddParameter(ShardedStrategyParameter<TValue> parameter)
        => _parameters.AddOrUpdate(parameter.Key, parameter, (key, value) => parameter);


    /// <summary>
    /// 格式化分片后缀。
    /// </summary>
    /// <param name="sharded">给定的 <see cref="ShardedDescriptor"/>。</param>
    /// <returns>返回字符串。</returns>
    public virtual string FormatSuffix(ShardedDescriptor sharded)
    {
        var suffix = sharded.Suffix;
        var isDefaultValue = IsDefaultValue(sharded, out TValue value);

        foreach (var p in _parameters)
        {
            suffix = suffix.Replace(p.Key, p.Value.ValueFormatter(value), ParameterComparison);

            if (suffix.Contains(p.Key))
                suffix = suffix.Replace(p.Key, p.Value.DefaultValue);
        }

        return sharded.Suffix = suffix;
    }

    /// <summary>
    /// 是否为默认值。
    /// </summary>
    /// <param name="sharded">给定的 <see cref="ShardedDescriptor"/>。</param>
    /// <param name="value">输出 <typeparamref name="TValue"/>。</param>
    /// <returns>返回布尔值。</returns>
    protected virtual bool IsDefaultValue(ShardedDescriptor sharded, out TValue value)
    {
        // 如果引用对象是值对象
        if (sharded.ReferenceValue is TValue referenceValue)
        {
            value = referenceValue;
            return false;
        }

        // 如果引用对象是属性值对象
        if (sharded.ReferenceValue is not null && sharded.Entity is not null
            && sharded.Entity.EntityType == (sharded.ReferenceType ?? sharded.ReferenceValue.GetType()))
        {
            var property = GetEntityProperty(sharded.Entity);
            if (property is not null)
            {
                var propertyValue = property.GetValue(sharded.ReferenceValue);
                ArgumentNullException.ThrowIfNull(propertyValue);

                value = (TValue)propertyValue!;
                return false;
            }
        }

        value = DefaultValue.Value;
        return true;
    }

    /// <summary>
    /// 获取分片实体的属性信息。
    /// </summary>
    /// <param name="entity">给定的 <see cref="ShardingEntity"/>。</param>
    /// <returns>返回 <see cref="PropertyInfo"/>。</returns>
    protected virtual PropertyInfo? GetEntityProperty(ShardingEntity entity)
    {
        PropertyInfo? propertyInfo = null;

        foreach (var property in entity.EntityType.GetProperties())
        {
            foreach (var key in entity.PropertyKeys)
            {
                if (property.PropertyType == key.SourceType
                    && property.Name == key.SourceAliase)
                {
                    propertyInfo = property;
                    break;
                }
            }

            if (propertyInfo is not null)
                break;
        }

        return propertyInfo;
    }


    /// <summary>
    /// 获取参数枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{T}"/>。</returns>
    public IEnumerator<ShardedStrategyParameter<TValue>> GetEnumerator()
        => _parameters.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}

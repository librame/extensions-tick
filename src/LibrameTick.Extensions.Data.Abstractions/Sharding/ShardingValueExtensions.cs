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
/// 定义 <see cref="IShardingValue"/> 的静态扩展。
/// </summary>
public static class ShardingValueExtensions
{
    private static readonly Type _valueGenericTypeDefinition = typeof(IShardingValue<>);
    private static readonly Type _propertyGenericTypeDefinition = typeof(IShardingProperty<,>);


    /// <summary>
    /// 将分片属性转为分片值。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="value">给定的 <see cref="IShardingProperty{T, TProperty}"/>。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <returns>返回 <see cref="IShardingValue{TProperty}"/>。</returns>
    public static IShardingValue<TProperty> AsShardingValue<TEntity, TProperty>(
        this IShardingProperty<TEntity, TProperty> value, object entity)
    {
        var realValue = ((IShardingValue)value).AsShardingValue(entity);
        ArgumentNullException.ThrowIfNull(realValue, nameof(value));

        return (realValue as IShardingValue<TProperty>)!;
    }

    /// <summary>
    /// 将集合中的所有分片属性转为分片值。
    /// </summary>
    /// <param name="values">给定可能包含分片属性的 <see cref="IShardingValue"/> 集合。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <returns>返回分片值的 <see cref="IShardingValue"/> 集合。</returns>
    public static IEnumerable<IShardingValue> AsShardingValues(this IEnumerable<IShardingValue> values, object entity)
    {
        foreach (var value in values)
        {
            var realValue = value.AsShardingValue(entity);
            if (realValue is not null)
                yield return realValue;
        }
    }

    private static IShardingValue? AsShardingValue(this IShardingValue value, object entity)
    {
        var type = value.GetType();
        var typeDefinition = type.GetGenericTypeDefinition();

        if (typeDefinition == _valueGenericTypeDefinition)
        {
            return value;
        }
        else if (typeDefinition == _propertyGenericTypeDefinition)
        {
            var method = type.GetMethod(nameof(IShardingValue<string>.GetShardedValue));

            var defaultValueType = type.GenericTypeArguments[1];
            var defaultValue = defaultValueType.IsNullableType() ? null : Activator.CreateInstance(defaultValueType);

            var realValueObject = method!.Invoke(entity, new object?[] { entity, defaultValue });
            var realType = typeof(SingleShardingValue<>).MakeGenericType(defaultValueType);

            var realValue = Activator.CreateInstance(realType, realValueObject) as IShardingValue;
            return realValue;
        }
        else
        {
            return null;
        }
    }

}

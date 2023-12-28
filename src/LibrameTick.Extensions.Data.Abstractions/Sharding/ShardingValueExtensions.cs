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
    /// 获取对象实现的泛型分片值实例集合。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回 <see cref="IEnumerable{IShardingValue}"/>。</returns>
    public static IEnumerable<IShardingValue> GetImplementedShardingValues(this object obj)
    {
        if (obj is null) yield break;

        var objType = obj.GetType();

        foreach (var type in objType.GetInterfaces())
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == _valueGenericTypeDefinition)
                yield return (IShardingValue)obj;
        }
    }


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
        var realValue = GetRealValue(value, entity) as IShardingValue<TProperty>;
        ArgumentNullException.ThrowIfNull(realValue, nameof(value));

        return realValue;
    }

    /// <summary>
    /// 将集合中的所有分片属性转为分片值。
    /// </summary>
    /// <param name="values">给定可能包含分片属性的 <see cref="IShardingValue"/> 集合。</param>
    /// <param name="entity">给定的实体对象。</param>
    /// <returns>返回分片值的 <see cref="IShardingValue"/> 集合。</returns>
    public static IReadOnlyCollection<IShardingValue> AsShardingValues(this IReadOnlyCollection<IShardingValue> values, object entity)
        => values.SelectWithoutNull(v => GetRealValue(v, entity)).AsReadOnlyCollection();

    private static IShardingValue? GetRealValue(IShardingValue value, object entity)
    {
        var type = value.GetType();
        var typeDefinition = type.GetGenericTypeDefinition();

        if (typeDefinition.IsImplementedType(_valueGenericTypeDefinition))
        {
            return value;
        }
        else if (typeDefinition.IsImplementedType(_propertyGenericTypeDefinition))
        {
            var method = type.GetMethod(nameof(IShardingProperty<Storing.Audit, long>.GetShardedValue));
            ArgumentNullException.ThrowIfNull(method, nameof(type));

            var realValueObject = method.Invoke(value, [entity]);
            ArgumentNullException.ThrowIfNull(realValueObject, nameof(value));

            var realType = typeof(SingleShardingValue<>).MakeGenericType(type.GenericTypeArguments[1]);
            var realValue = (IShardingValue)realType.NewByExpression(realValueObject);

            return realValue;
        }
        else
        {
            return null;
        }
    }

}

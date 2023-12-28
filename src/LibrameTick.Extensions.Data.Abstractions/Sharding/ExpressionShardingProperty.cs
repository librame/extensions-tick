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
/// 定义一个实现 <see cref="IShardingProperty{T, TProperty}"/> 的泛型表达式分片属性。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
/// <typeparam name="TProperty">指定的属性类型。</typeparam>
/// <remarks>
/// 构造一个 <see cref="ExpressionShardingProperty{T, TProperty}"/>。
/// </remarks>
/// <param name="propertyExpression">给定的属性表达式。</param>
public sealed class ExpressionShardingProperty<T, TProperty>(Expression<Func<T, TProperty>> propertyExpression)
    : IShardingProperty<T, TProperty>
{
    //private readonly Expression<Func<T, TProperty>> _propertyExpression;
    private readonly Func<T, TProperty> _propertyInvoker = propertyExpression.Compile();


    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <param name="instance">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回 <typeparamref name="TProperty"/>。</returns>
    public TProperty GetShardedValue(T instance)
        => _propertyInvoker(instance);

}

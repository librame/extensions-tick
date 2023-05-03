#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个静态属性相等比较器。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public static class PropertyEqualityComparer<T>
{

    /// <summary>
    /// 创建相等比较器。
    /// </summary>
    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    /// <param name="propertySelector">给定用于比较的属性选择器。</param>
    /// <returns>返回 <see cref="IEqualityComparer{T}"/>。</returns>
    public static IEqualityComparer<T> Create<TProperty>(Func<T, TProperty> propertySelector)
        where TProperty : IEquatable<TProperty>
        => new PropertyEqualityComparer<T, TProperty>(propertySelector);

}


/// <summary>
/// 定义一个继承 <see cref="EqualityComparer{T}"/> 的属性相等比较器。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
/// <typeparam name="TProperty">指定的属性类型。</typeparam>
public class PropertyEqualityComparer<T, TProperty> : EqualityComparer<T>
    where TProperty : IEquatable<TProperty>
{
    private readonly Func<T, TProperty> _propertySelector;


    /// <summary>
    /// 构造一个 <see cref="PropertyEqualityComparer{T, TProperty}"/>。
    /// </summary>
    /// <param name="propertySelector">给定用于比较的属性选择器。</param>
    public PropertyEqualityComparer(Func<T, TProperty> propertySelector)
    {
        _propertySelector = propertySelector;
    }


    /// <summary>
    /// 是否相等。
    /// </summary>
    /// <param name="x">给定的 <typeparamref name="T"/>。</param>
    /// <param name="y">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(T? x, T? y)
        => x is not null && y is not null && _propertySelector(x).Equals(_propertySelector(y));

    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <param name="obj">给定的 <typeparamref name="T"/>。</param>
    /// <returns>返回整数。</returns>
    public override int GetHashCode([DisallowNull] T obj)
        => _propertySelector(obj)?.GetHashCode() ?? 0;

}

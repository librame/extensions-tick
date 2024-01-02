#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Attribute"/> 静态扩展。
/// </summary>
public static class AttributeExtensions
{

    /// <summary>
    /// 尝试获取不支持继承的特性。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="attribute">输出 <typeparamref name="TAttribute"/>。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    public static bool TryGetAttribute<TAttribute>(this Type sourceType,
        [NotNullWhen(true)] out TAttribute? attribute)
        where TAttribute : Attribute
    {
        attribute = sourceType.GetCustomAttribute<TAttribute>(inherit: false);
        return attribute is not null;
    }

    /// <summary>
    /// 尝试获取支持继承的特性。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="attribute">输出 <typeparamref name="TAttribute"/>。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    public static bool TryGetAttributeWithInherit<TAttribute>(this Type sourceType,
        [NotNullWhen(true)] out TAttribute? attribute)
        where TAttribute : Attribute
    {
        attribute = sourceType.GetCustomAttribute<TAttribute>(inherit: true);
        return attribute is not null;
    }


    /// <summary>
    /// 尝试获取不支持继承的特性集合。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="attributes">输出 <see cref="IEnumerable{TAttribute}"/>。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    public static bool TryGetAttributes<TAttribute>(this Type sourceType,
        [NotNullWhen(true)] out IEnumerable<TAttribute>? attributes)
        where TAttribute : Attribute
    {
        attributes = sourceType.GetCustomAttributes<TAttribute>(inherit: false);
        return attributes.Any();
    }

    /// <summary>
    /// 尝试获取支持继承的特性集合。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="attributes">输出 <see cref="IEnumerable{TAttribute}"/>。</param>
    /// <returns>返回是否获取的布尔值。</returns>
    public static bool TryGetAttributesWithInherit<TAttribute>(this Type sourceType,
        [NotNullWhen(true)] out IEnumerable<TAttribute>? attributes)
        where TAttribute : Attribute
    {
        attributes = sourceType.GetCustomAttributes<TAttribute>(inherit: true);
        return attributes.Any();
    }

}

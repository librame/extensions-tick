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
    /// 是否已定义特性。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="type">给定的类型。</param>
    /// <param name="attribute">输出 <typeparamref name="TAttribute"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsDefined<TAttribute>(this Type type,
        [MaybeNullWhen(false)] out TAttribute? attribute)
        where TAttribute : Attribute
    {
        attribute = type.GetCustomAttribute<TAttribute>();
        return attribute is not null;
    }

    /// <summary>
    /// 是否已定义特性。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="type">给定的类型。</param>
    /// <param name="inherit">是否检查继承的父级特性。</param>
    /// <param name="attribute">输出 <typeparamref name="TAttribute"/>。</param>
    /// <returns>返回布尔值。</returns>
    public static bool IsDefined<TAttribute>(this Type type, bool inherit,
        [MaybeNullWhen(false)] out TAttribute? attribute)
        where TAttribute : Attribute
    {
        attribute = type.GetCustomAttribute<TAttribute>(inherit);
        return attribute is not null;
    }


    /// <summary>
    /// 尝试获取特性。
    /// </summary>
    /// <typeparam name="TAttribute">指定的特性类型。</typeparam>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="attribute">输出取得的特性。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TryGetAttribute<TAttribute>(this Type sourceType,
        [MaybeNullWhen(false)] out TAttribute attribute)
        where TAttribute : Attribute
    {
        attribute = sourceType.GetCustomAttribute<TAttribute>();
        return attribute is not null;
    }

    /// <summary>
    /// 尝试获取特性。
    /// </summary>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="attributeType">给定要取得的特性类型。</param>
    /// <param name="attribute">输出取得的特性。</param>
    /// <returns>返回布尔值。</returns>
    public static bool TryGetAttribute(this Type sourceType, Type attributeType,
        [MaybeNullWhen(false)] out Attribute attribute)
    {
        attribute = sourceType.GetCustomAttribute(attributeType);
        return attribute is not null;
    }

}

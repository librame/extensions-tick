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
/// 定义属性通知命名键。
/// </summary>
/// <typeparam name="TSource">指定的源类型。</typeparam>
public class PropertyNoticeNamedKey<TSource> : PropertyNoticeNamedKey
{
    /// <summary>
    /// 构造一个 <see cref="PropertyNoticeNamedKey{TSource}"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public PropertyNoticeNamedKey(string propertyName, string? sourceAliase = null)
        : base(propertyName, new TypeNamedKey<TSource>(sourceAliase))
    {
        BaseKey = (TypeNamedKey<TSource>)base.BaseKey;
    }

    /// <summary>
    /// 构造一个 <see cref="PropertyNoticeNamedKey{TSource}"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="baseKey">给定的 <see cref="TypeNamedKey{TSource}"/> 基础键。</param>
    public PropertyNoticeNamedKey(string propertyName, TypeNamedKey<TSource> baseKey)
        : base(propertyName, baseKey)
    {
        BaseKey = baseKey;
    }


    /// <summary>
    /// 基础键。
    /// </summary>
    public new TypeNamedKey<TSource> BaseKey { get; init; }


    /// <summary>
    /// 使用新属性名称创建属性通知命名键。
    /// </summary>
    /// <param name="newPropertyName">给定的新属性名称。</param>
    /// <returns>返回 <see cref="PropertyNoticeNamedKey{TSource}"/>。</returns>
    public new PropertyNoticeNamedKey<TSource> WithPropertyName(string newPropertyName)
        => new PropertyNoticeNamedKey<TSource>(newPropertyName, BaseKey);

}


/// <summary>
/// 定义属性通知命名键。
/// </summary>
public class PropertyNoticeNamedKey : IEquatable<PropertyNoticeNamedKey>
{
    /// <summary>
    /// 构造一个 <see cref="TypeNamedKey"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="sourceType">给定的源类型。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public PropertyNoticeNamedKey(string propertyName, Type sourceType, string? sourceAliase = null)
        : this(propertyName, new TypeNamedKey(sourceType, sourceAliase))
    {
    }

    /// <summary>
    /// 构造一个 <see cref="TypeNamedKey"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="baseKey">给定的 <see cref="TypeNamedKey"/> 基础键。</param>
    public PropertyNoticeNamedKey(string propertyName, TypeNamedKey baseKey)
    {
        PropertyName = propertyName;
        BaseKey = baseKey;
    }


    /// <summary>
    /// 属性名称。
    /// </summary>
    public string PropertyName { get; init; }

    /// <summary>
    /// 基础键。
    /// </summary>
    public TypeNamedKey BaseKey { get; init; }


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="PropertyNoticeNamedKey"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(PropertyNoticeNamedKey? other)
        => other is not null && BaseKey.Equals(other.BaseKey);


    /// <summary>
    /// 使用新属性名称创建属性通知命名键。
    /// </summary>
    /// <param name="newPropertyName">给定的新属性名称。</param>
    /// <returns>返回 <see cref="PropertyNoticeNamedKey"/>。</returns>
    public PropertyNoticeNamedKey WithPropertyName(string newPropertyName)
        => new PropertyNoticeNamedKey(newPropertyName, BaseKey);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{BaseKey}_{PropertyName}";

}

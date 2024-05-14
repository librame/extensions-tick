#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Notification;

/// <summary>
/// 定义泛型属性类型命名键。
/// </summary>
/// <typeparam name="TSource">指定的源类型。</typeparam>
public class PropertyTypeNamedKey<TSource> : PropertyNoticeNamedKey
{
    /// <summary>
    /// 构造一个 <see cref="PropertyTypeNamedKey{TSource}"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="named">给定的命名（可选）。</param>
    public PropertyTypeNamedKey(string propertyName, string? named = null)
        : base(propertyName, new TypeNamedKey<TSource>(named))
    {
        BaseKey = (TypeNamedKey<TSource>)base.BaseKey;
    }

    /// <summary>
    /// 构造一个 <see cref="PropertyTypeNamedKey{TSource}"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="baseKey">给定的 <see cref="TypeNamedKey{TSource}"/> 基础键。</param>
    public PropertyTypeNamedKey(string propertyName, TypeNamedKey<TSource> baseKey)
        : base(propertyName, baseKey)
    {
        BaseKey = baseKey;
    }


    /// <summary>
    /// 基础键。
    /// </summary>
    public new TypeNamedKey<TSource> BaseKey { get; init; }


    /// <summary>
    /// 使用新属性名称创建属性类型命名键。
    /// </summary>
    /// <param name="newPropertyName">给定的新属性名称。</param>
    /// <returns>返回 <see cref="PropertyTypeNamedKey{TSource}"/>。</returns>
    public new PropertyTypeNamedKey<TSource> WithPropertyName(string newPropertyName)
        => new(newPropertyName, BaseKey);

}


/// <summary>
/// 定义属性类型命名键。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="TypeNamedKey"/>。
/// </remarks>
/// <param name="propertyName">给定的属性名称。</param>
/// <param name="baseKey">给定的 <see cref="TypeNamedKey"/> 基础键。</param>
public class PropertyNoticeNamedKey(string propertyName, TypeNamedKey baseKey) : IEquatable<PropertyNoticeNamedKey>
{
    /// <summary>
    /// 构造一个 <see cref="TypeNamedKey"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="sourceType">给定的来源类型。</param>
    /// <param name="named">给定的命名（可选）。</param>
    public PropertyNoticeNamedKey(string propertyName, Type sourceType, string? named = null)
        : this(propertyName, new TypeNamedKey(sourceType, named))
    {
    }


    /// <summary>
    /// 属性名称。
    /// </summary>
    public string PropertyName { get; init; } = propertyName;

    /// <summary>
    /// 基础键。
    /// </summary>
    public TypeNamedKey BaseKey { get; init; } = baseKey;


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as PropertyNoticeNamedKey);

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
        => new(newPropertyName, BaseKey);


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
        => $"{BaseKey}{BaseKey.Connector}{PropertyName}";

}

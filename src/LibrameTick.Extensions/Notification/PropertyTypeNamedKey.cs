#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Notification;

/// <summary>
/// 定义属性类型命名键。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="TypedNamedKey"/>。
/// </remarks>
/// <param name="propertyName">给定的属性名称。</param>
/// <param name="baseKey">给定的 <see cref="TypedNamedKey"/> 基础键。</param>
public class PropertyNoticeNamedKey(string propertyName, TypedNamedKey baseKey)
    : IEquatable<PropertyNoticeNamedKey>
{
    /// <summary>
    /// 构造一个 <see cref="TypedNamedKey"/>。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="sourceType">给定的来源类型。</param>
    /// <param name="named">给定的命名（可选）。</param>
    public PropertyNoticeNamedKey(string propertyName, Type sourceType, string? named = null)
        : this(propertyName, new TypedNamedKey(sourceType, named))
    {
    }


    /// <summary>
    /// 属性名称。
    /// </summary>
    public string PropertyName { get; init; } = propertyName;

    /// <summary>
    /// 基础键。
    /// </summary>
    public TypedNamedKey BaseKey { get; init; } = baseKey;


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


    /// <summary>
    /// 创建泛型属性类型命名键。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <see cref="TypedNamedKey"/>。</returns>
    public static PropertyNoticeNamedKey Create<T>(string propertyName, string? named = null)
        => new(propertyName, TypedNamedKey.Create<T>(named));

    /// <summary>
    /// 创建带连接符的泛型属性类型命名键。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="connector">给定的连接符。</param>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <see cref="TypedNamedKey"/>。</returns>
    public static PropertyNoticeNamedKey CreateWithConnector<T>(string propertyName, string connector, string? named = null)
        => new(propertyName, TypedNamedKey.CreateWithConnector<T>(connector, named));

    /// <summary>
    /// 创建带连接符的属性类型命名键。
    /// </summary>
    /// <param name="propertyName">给定的属性名称。</param>
    /// <param name="type">给定的类型。</param>
    /// <param name="connector">给定的连接符。</param>
    /// <param name="named">给定的命名（可选）。</param>
    /// <returns>返回 <see cref="TypedNamedKey"/>。</returns>
    public static PropertyNoticeNamedKey CreateWithConnector(string propertyName, Type type,
        string connector, string? named = null)
        => new(propertyName, TypedNamedKey.CreateWithConnector(type, connector, named));

}

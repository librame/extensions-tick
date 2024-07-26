#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制序列化成员转换器的自定义特性。
/// </summary>
/// <param name="name">给定的转换器名称（可选）。</param>
/// <param name="type">给定的转换器类型（可选）。</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = true, AllowMultiple = false)]
public class BinaryConverterAttribute(string? name = null, Type? type = null) : Attribute
{
    /// <summary>
    /// 获取或设置转换器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public string? Name { get; set; } = name;

    /// <summary>
    /// 获取或设置转换器类型。
    /// </summary>
    /// <value>
    /// 返回类型。
    /// </value>
    public Type? Type { get; set; } = type;
}

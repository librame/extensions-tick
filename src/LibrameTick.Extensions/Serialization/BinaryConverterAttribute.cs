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
/// <typeparam name="TConverter">指定的转换器类型。</typeparam>
/// <param name="name">给定的转换器名称（可选）。</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = true, AllowMultiple = false)]
public class BinaryConverterAttribute<TConverter>(string? name = null)
    : BinaryConverterAttribute(name, typeof(TConverter))
{
}


/// <summary>
/// 定义二进制序列化成员转换器的自定义特性。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = true, AllowMultiple = false)]
public class BinaryConverterAttribute : Attribute
{
    /// <summary>
    /// 构造一个 <see cref="BinaryConverterAttribute"/>。
    /// </summary>
    /// <param name="name">给定的转换器名称（可选）。</param>
    /// <param name="convertedType">给定的被转换类型（可选）。</param>
    /// <exception cref="ArgumentException">
    /// The '<paramref name="name"/>' and '<paramref name="convertedType"/>' cannot be both null.
    /// </exception>
    public BinaryConverterAttribute(string? name = null, Type? convertedType = null)
    {
        if (name is null && convertedType is null)
        {
            throw new ArgumentException($"The '{nameof(name)}' and '{nameof(convertedType)}' cannot be both null.");
        }

        Name = name;
        ConvertedType = convertedType;
    }


    /// <summary>
    /// 获取转换器名称。
    /// </summary>
    /// <value>
    /// 返回名称字符串。
    /// </value>
    public string? Name { get; init; }

    /// <summary>
    /// 获取被转换类型。
    /// </summary>
    /// <value>
    /// 返回类型。
    /// </value>
    public Type? ConvertedType { get; init; }
}

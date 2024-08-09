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
/// 定义二进制转换器解析器接口。
/// </summary>
public interface IBinaryConverterResolver
{
    /// <summary>
    /// 获取二进制序列化选项。
    /// </summary>
    /// <value>
    /// 返回 <see cref="BinarySerializerOptions"/>。
    /// </value>
    BinarySerializerOptions Options { get; }


    /// <summary>
    /// 解析指定类型的二进制转换器。
    /// </summary>
    /// <typeparam name="TConverted">指定的被转换类型。</typeparam>
    /// <param name="attribute">给定的 <see cref="BinaryConverterAttribute"/>。</param>
    /// <returns>返回 <see cref="BinaryConverter{TConverted}"/>。</returns>
    BinaryConverter<TConverted>? ResolveConverter<TConverted>(BinaryConverterAttribute? attribute);

    /// <summary>
    /// 解析指定类型的二进制转换器。
    /// </summary>
    /// <typeparam name="TConverted">指定的被转换类型。</typeparam>
    /// <param name="name">给定的转换器名称（可选）。</param>
    /// <returns>返回 <see cref="BinaryConverter{TConverted}"/>。</returns>
    BinaryConverter<TConverted>? ResolveConverter<TConverted>(string? name = null);

    /// <summary>
    /// 解析指定类型的二进制转换器。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <param name="attribute">给定的 <see cref="BinaryConverterAttribute"/>。</param>
    /// <returns>返回 <see cref="IBinaryConverter"/>。</returns>
    IBinaryConverter? ResolveConverter(Type typeToConvert, BinaryConverterAttribute? attribute);

    /// <summary>
    /// 解析指定类型的二进制转换器。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <param name="name">给定的转换器名称（可选）。</param>
    /// <returns>返回 <see cref="IBinaryConverter"/>。</returns>
    IBinaryConverter? ResolveConverter(Type typeToConvert, string? name = null);
}

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
/// 定义 JSON 字符串形式的 <see cref="Encoding"/> 节点转换器。
/// </summary>
public sealed class JsonStringEncodingConverterFactory : JsonConverterFactory
{
    /// <summary>
    /// 判断能否转换为 <see cref="Encoding"/> 类型。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <returns>返回能否转换的布尔值。</returns>
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert == typeof(Encoding);

    /// <summary>
    /// 创建 <see cref="Encoding"/> 转换器。
    /// </summary>
    /// <param name="typeToConvert">给定要转换的类型。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
    /// <returns>返回 <see cref="JsonConverter"/>。</returns>
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => new JsonStringEncodingConverter();

}

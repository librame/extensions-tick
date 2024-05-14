#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Serialization;

/// <summary>
/// 定义 JSON 字符串形式的 <see cref="Encoding"/> 节点转换器。
/// </summary>
public class JsonStringEncodingConverter : JsonConverter<Encoding>
{

    /// <summary>
    /// 读取 JSON。
    /// </summary>
    /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
    /// <param name="typeToConvert">给定的类型转换。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
    /// <returns>返回 <see cref="Encoding"/>。</returns>
    public override Encoding? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetOrDefault(Encoding.GetEncoding, Encoding.UTF8);

    /// <summary>
    /// 写入 JSON。
    /// </summary>
    /// <param name="writer">给定的 <see cref="Utf8JsonWriter"/>。</param>
    /// <param name="value">给定的 <see cref="Encoding"/>。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
    public override void Write(Utf8JsonWriter writer, Encoding value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.BodyName);

}

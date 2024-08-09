#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.JsonConverters;

/// <summary>
/// 精简 <see cref="Type"/> JSON 字符串节点转换器。
/// </summary>
public class JsonStringReducedTypeConverter : JsonConverter<Type>
{

    /// <summary>
    /// 读取 JSON。
    /// </summary>
    /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
    /// <param name="typeToConvert">给定的类型转换。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
    /// <returns>返回 <see cref="Type"/>。</returns>
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => reader.GetOrNull(static str => Type.GetType(str));

    /// <summary>
    /// 写入 JSON。
    /// </summary>
    /// <param name="writer">给定的 <see cref="Utf8JsonWriter"/>。</param>
    /// <param name="value">给定的 <see cref="Type"/>。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
    public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.GetFriendlyName());

}

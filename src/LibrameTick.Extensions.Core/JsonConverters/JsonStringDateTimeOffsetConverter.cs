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

///// <summary>
///// <see cref="DateTimeOffset"/> JSON 节点转换器。
///// </summary>
//public class JsonStringDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
//{

//    /// <summary>
//    /// 读取 JSON。
//    /// </summary>
//    /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
//    /// <param name="typeToConvert">给定的类型转换。</param>
//    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
//    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
//    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        => DateTimeOffset.Parse(reader.GetRequiredString());

//    /// <summary>
//    /// 写入 JSON。
//    /// </summary>
//    /// <param name="writer">给定的 <see cref="Utf8JsonWriter"/>。</param>
//    /// <param name="value">给定的 <see cref="Encoding"/>。</param>
//    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
//    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
//        => writer.WriteStringValue(value.AsDateTimeString());

//}

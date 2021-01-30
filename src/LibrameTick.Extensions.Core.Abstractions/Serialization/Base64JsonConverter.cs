#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core.Serialization
{
    /// <summary>
    /// BASE64 类型的 <see cref="JsonConverter"/>。
    /// </summary>
    public class Base64JsonConverter : JsonConverter<byte[]>
    {
        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
        /// <param name="typeToConvert">给定的类型转换。</param>
        /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
        /// <returns>返回 <see cref="Encoding"/>。</returns>
        public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => Convert.FromBase64String(reader.GetNotEmptyString());

        /// <summary>
        /// 写入 JSON。
        /// </summary>
        /// <param name="writer">给定的 <see cref="Utf8JsonWriter"/>。</param>
        /// <param name="value">给定的 <see cref="Encoding"/>。</param>
        /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
            => writer.WriteStringValue(Convert.ToBase64String(value));

    }
}

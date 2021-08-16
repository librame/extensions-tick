#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Serialization
{
    /// <summary>
    /// 字节数组 JSON 节点加密转换器（默认使用 AES 加解密）。
    /// </summary>
    public class JsonStringByteArrayEncryptionConverter : JsonConverter<byte[]>
    {

        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
        /// <param name="typeToConvert">给定的类型转换。</param>
        /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
        /// <returns>返回字节数组。</returns>
        public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => reader.GetBytesFromBase64().FromAes();

        /// <summary>
        /// 写入 JSON。
        /// </summary>
        /// <param name="writer">给定的 <see cref="Utf8JsonWriter"/>。</param>
        /// <param name="value">给定的字节数组。</param>
        /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
        public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.AsAes());

    }
}

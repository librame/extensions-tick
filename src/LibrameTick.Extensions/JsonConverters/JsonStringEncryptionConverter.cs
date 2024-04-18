﻿#region License

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
/// 字符串 JSON 节点加密转换器（默认使用 AES 加解密）。
/// </summary>
public class JsonStringEncryptionConverter : JsonConverter<string>
{
    /// <summary>
    /// 当前算法管理器。
    /// </summary>
    public static Dependencies.IExtensionsDependency Dependency
        => Dependencies.ExtensionsDependencyInstantiator.Instance;


    /// <summary>
    /// 读取 JSON。
    /// </summary>
    /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
    /// <param name="typeToConvert">给定的类型转换。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
    /// <returns>返回字符串。</returns>
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => Dependency.Encoding.GetString(reader.GetBytesFromBase64().FromAes());

    /// <summary>
    /// 写入 JSON。
    /// </summary>
    /// <param name="writer">给定的 <see cref="Utf8JsonWriter"/>。</param>
    /// <param name="value">给定的字符串。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>。</param>
    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        => writer.WriteBase64StringValue(Dependency.Encoding.GetBytes(value).AsAes());

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Librame.Extensions
{
    /// <summary>
    /// <see cref="JsonSerializer"/> 静态扩展。
    /// </summary>
    public static class JsonSerializerExtensions
    {

        #region ReadJson and WriteJson

        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <typeparam name="T">指定的反序列化类型。</typeparam>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static T? ReadJson<T>(this string filePath, Encoding? encoding = null,
            JsonSerializerOptions? options = null)
        {
            var json = File.ReadAllText(filePath, encoding ?? EncodingExtensions.UTF8Encoding);
            return JsonSerializer.Deserialize<T>(json, options);
        }

        /// <summary>
        /// 读取 JSON。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="type">给定的反序列化对象类型。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选）。</param>
        /// <returns>返回反序列化对象。</returns>
        public static object? ReadJson(this string filePath, Type type, Encoding? encoding = null,
            JsonSerializerOptions? options = null)
        {
            var json = File.ReadAllText(filePath, encoding ?? EncodingExtensions.UTF8Encoding);
            return JsonSerializer.Deserialize(json, type, options);
        }


        /// <summary>
        /// 写入 JSON（默认支持枚举类型）。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="value">给定的对象值。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认为 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
        /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选）。</param>
        /// <param name="autoCreateDirectory">自动创建目录（可选；默认启用）。</param>
        /// <returns>返回 JSON 字符串。</returns>
        public static string WriteJson(this string filePath, object value, Encoding? encoding = null,
            JsonSerializerOptions? options = null, bool autoCreateDirectory = true)
        {
            if (options == null)
            {
                options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                options.Converters.Add(new JsonStringEnumConverter());
            }

            var json = JsonSerializer.Serialize(value, options);

            if (autoCreateDirectory)
            {
                var dir = Path.GetDirectoryName(filePath);
                dir!.CreateDirectory();
            }

            File.WriteAllText(filePath, json, encoding ?? EncodingExtensions.UTF8Encoding);
            return json;
        }

        #endregion

    }
}

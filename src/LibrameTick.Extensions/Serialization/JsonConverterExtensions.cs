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
    /// <see cref="JsonConverter"/> 静态扩展。
    /// </summary>
    public static class JsonConverterExtensions
    {

        /// <summary>
        /// 获取必需的字符串。
        /// </summary>
        /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string GetRequiredString(this Utf8JsonReader reader)
        {
            var str = reader.GetString();

            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException($"'{nameof(reader)}.{nameof(reader.GetString)}()' invoke result is null or empty.");

            return str;
        }

    }
}

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
using System.Text.Json;

namespace Librame.Extensions.Core.Serialization
{
    static class JsonConverterExtensions
    {
        /// <summary>
        /// 获取不为空的字符串。
        /// </summary>
        /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string GetNotEmptyString(this Utf8JsonReader reader)
        {
            var str = reader.GetString();

            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException($"'{nameof(reader)}.{nameof(reader.GetString)}()' invoke result is null or empty.");

            return str;
        }

    }
}

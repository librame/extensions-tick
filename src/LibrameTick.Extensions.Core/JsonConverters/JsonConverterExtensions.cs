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
///// <see cref="JsonConverter"/> 静态扩展。
///// </summary>
//public static class JsonConverterExtensions
//{

//    /// <summary>
//    /// 获取给定转换器的可空值。
//    /// </summary>
//    /// <typeparam name="TValue">指定的值类型。</typeparam>
//    /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
//    /// <param name="converter">给定的值转换器。</param>
//    /// <returns>返回 <see cref="Nullable{TValue}"/>。</returns>
//    public static TValue? GetOrNull<TValue>(this Utf8JsonReader reader, Func<string, TValue> converter)
//    {
//        var str = reader.GetString();

//        return string.IsNullOrEmpty(str) ? default : converter(str);
//    }

//    /// <summary>
//    /// 获取必需的字符串。
//    /// </summary>
//    /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
//    /// <returns>返回字符串。</returns>
//    public static string GetRequiredString(this Utf8JsonReader reader)
//    {
//        var str = reader.GetString();

//        if (string.IsNullOrEmpty(str))
//            throw new ArgumentNullException($"'{nameof(reader)}.{nameof(reader.GetString)}()' invoke result is null or empty.");

//        return str;
//    }

//}

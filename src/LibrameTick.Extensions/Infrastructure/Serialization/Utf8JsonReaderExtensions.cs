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
/// 定义 <see cref="Utf8JsonReader"/> 静态扩展。
/// </summary>
public static class Utf8JsonReaderExtensions
{

    /// <summary>
    /// 获取或默认指定结果。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="reader">给定的 <see cref="Utf8JsonReader"/>。</param>
    /// <param name="convert">给定的转换方法。</param>
    /// <param name="defaultResult">给定如果为空字符串的默认结果。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    public static TResult GetOrDefault<TResult>(this Utf8JsonReader reader,
        Func<string, TResult> convert, TResult defaultResult)
    {
        var str = reader.GetString();

        if (string.IsNullOrEmpty(str))
        {
            return defaultResult;
        }

        return convert(str);
    }

}

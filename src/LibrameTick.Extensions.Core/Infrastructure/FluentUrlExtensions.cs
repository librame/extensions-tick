#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义 <see cref="FluentUrl"/> 静态扩展。
/// </summary>
public static class FluentUrlExtensions
{

    /// <summary>
    /// 异步获取字符串。
    /// </summary>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="client">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="options">给定的 <see cref="JsonSerializerOptions"/>（可选；默认使用 <see cref="JsonDependency"/> 选项）。</param>
    /// <returns>返回一个包含 <typeparamref name="T"/> 的异步操作。</returns>
    public static async Task<T?> GetFromJsonAsync<T>(this FluentUrl url, HttpClient client,
        JsonSerializerOptions? options = null)
    {
        var json = await client.GetStringAsync(url.CurrentValue).ConfigureAwait(false);

        return JsonSerializer.Deserialize<T>(json, options ?? JsonDependency.Options);
    }

}

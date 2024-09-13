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
/// 定义 <see cref="FluentUrl"/> 与 <see cref="HttpClient"/> 静态扩展。
/// </summary>
public static class FluentUrlHttpClientExtensions
{

    /// <summary>
    /// 异步获取 HTTP 响应的二进制反序列化类型实例。
    /// </summary>
    /// <remarks>
    /// 此方法仅支持使用 <see cref="Serialization.BinarySerializer"/> 序列化的字节数组数据。
    /// </remarks>
    /// <typeparam name="T">指定的二进制类型。</typeparam>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="httpClient">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <param name="serializerOptions">给定的 <see cref="Serialization.BinarySerializerOptions"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含二进制反序列化类型实例的异步操作。</returns>
    public static async Task<T?> GetBinaryAsync<T>(this FluentUrl url, HttpClient httpClient,
        int failRetries = 0, TimeSpan? failRetryInterval = null, bool failRetryIntervalIncrement = true,
        Serialization.BinarySerializerOptions? serializerOptions = null, CancellationToken cancellationToken = default)
    {
        var byteArray = await url.GetByteArrayAsync(httpClient, failRetries, failRetryInterval,
            failRetryIntervalIncrement, cancellationToken);

        return Serialization.BinaryExtensions.FromBinary<T>(byteArray, serializerOptions);
    }

    /// <summary>
    /// 异步获取 HTTP 响应的二进制反序列化类型对象。
    /// </summary>
    /// <remarks>
    /// 此方法仅支持使用 <see cref="Serialization.BinarySerializer"/> 序列化的字节数组数据。
    /// </remarks>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="httpClient">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="returnType">给定的二进制反序列化类型。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <param name="serializerOptions">给定的 <see cref="Serialization.BinarySerializerOptions"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含二进制反序列化类型对象的异步操作。</returns>
    public static async Task<object?> GetBinaryAsync(this FluentUrl url, HttpClient httpClient, Type returnType,
        int failRetries = 0, TimeSpan? failRetryInterval = null, bool failRetryIntervalIncrement = true,
        Serialization.BinarySerializerOptions? serializerOptions = null, CancellationToken cancellationToken = default)
    {
        var byteArray = await url.GetByteArrayAsync(httpClient, failRetries, failRetryInterval,
            failRetryIntervalIncrement, cancellationToken);

        return Serialization.BinaryExtensions.FromBinary(byteArray, returnType, serializerOptions);
    }


    /// <summary>
    /// 异步获取 HTTP 响应的 JSON 反序列化类型对象。
    /// </summary>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="httpClient">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="returnType">给定的 JSON 反序列化类型。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <param name="serializerOptions">给定的 <see cref="JsonSerializerOptions"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 JSON 反序列化类型对象的异步操作。</returns>
    public static async Task<object?> GetJsonAsync(this FluentUrl url, HttpClient httpClient, Type returnType,
        int failRetries = 0, TimeSpan? failRetryInterval = null, bool failRetryIntervalIncrement = true,
        JsonSerializerOptions? serializerOptions = null, CancellationToken cancellationToken = default)
    {
        var json = await url.GetStringAsync(httpClient, failRetries, failRetryInterval,
            failRetryIntervalIncrement, cancellationToken);

        return Serialization.JsonExtensions.FromJson(json, returnType, serializerOptions);
    }

    /// <summary>
    /// 异步获取 HTTP 响应的 JSON 反序列化类型实例。
    /// </summary>
    /// <typeparam name="T">指定的 JSON 类型。</typeparam>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="httpClient">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <param name="serializerOptions">给定的 <see cref="JsonSerializerOptions"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 JSON 反序列化类型实例的异步操作。</returns>
    public static async Task<T?> GetJsonAsync<T>(this FluentUrl url, HttpClient httpClient,
        int failRetries = 0, TimeSpan? failRetryInterval = null, bool failRetryIntervalIncrement = true,
        JsonSerializerOptions? serializerOptions = null, CancellationToken cancellationToken = default)
    {
        var json = await url.GetStringAsync(httpClient, failRetries, failRetryInterval,
            failRetryIntervalIncrement, cancellationToken);

        return Serialization.JsonExtensions.FromJson<T>(json, serializerOptions);
    }


    /// <summary>
    /// 异步获取 HTTP 响应字节数组。
    /// </summary>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="httpClient">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字节数组的异步操作。</returns>
    public static async Task<byte[]> GetByteArrayAsync(this FluentUrl url, HttpClient httpClient,
        int failRetries = 0, TimeSpan? failRetryInterval = null, bool failRetryIntervalIncrement = true,
        CancellationToken cancellationToken = default)
    {
        var result = await url.HandleResultAsync(async uri =>
        {
            return await httpClient.GetByteArrayAsync(uri, cancellationToken).ConfigureAwait(false);
        },
        failRetries, failRetryInterval, failRetryIntervalIncrement).ConfigureAwait(false);

        return result!;
    }

    /// <summary>
    /// 异步获取 HTTP 响应字符串。
    /// </summary>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="httpClient">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public static async Task<string> GetStringAsync(this FluentUrl url, HttpClient httpClient,
        int failRetries = 0, TimeSpan? failRetryInterval = null, bool failRetryIntervalIncrement = true,
        CancellationToken cancellationToken = default)
    {
        var result = await url.HandleResultAsync(async uri =>
        {
            return await httpClient.GetStringAsync(uri, cancellationToken).ConfigureAwait(false);
        },
        failRetries, failRetryInterval, failRetryIntervalIncrement).ConfigureAwait(false);

        return result!;
    }

    /// <summary>
    /// 异步获取 HTTP 响应消息。
    /// </summary>
    /// <param name="url">给定的 <see cref="FluentUrl"/>。</param>
    /// <param name="httpClient">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="failRetries">给定的失败重试次数（可选；默认为 0 次表示不重试）。</param>
    /// <param name="failRetryInterval">给定的单次失败重试间隔（可选；默认为 NULL 表示无间隔。此项须在指定 <paramref name="failRetries"/> 次数大于 0 时有效）。</param>
    /// <param name="failRetryIntervalIncrement">是否递增单次失败重试的间隔（可选；默认为 TRUE 表示启用递增。此项须在指定 <paramref name="failRetryInterval"/> 不为 NULL 时有效）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="HttpResponseMessage"/> 的异步操作。</returns>
    public static async Task<HttpResponseMessage> GetAsync(this FluentUrl url, HttpClient httpClient,
        int failRetries = 0, TimeSpan? failRetryInterval = null, bool failRetryIntervalIncrement = true,
        CancellationToken cancellationToken = default)
    {
        var result = await url.HandleResultAsync(async uri =>
        {
            return await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        },
        failRetries, failRetryInterval, failRetryIntervalIncrement).ConfigureAwait(false);

        return result!;
    }

}

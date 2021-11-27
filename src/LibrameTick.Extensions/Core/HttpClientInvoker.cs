#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义 HTTP 客户端调用器。
/// </summary>
public static class HttpClientInvoker
{
    /// <summary>
    /// 使用调用选项配置 HTTP 客户端。
    /// </summary>
    /// <param name="client">给定的 <see cref="HttpClient"/>。</param>
    /// <param name="options">给定的 <see cref="HttpClientInvokingOptions"/>。</param>
    /// <returns>返回 <see cref="HttpClient"/>。</returns>
    public static HttpClient ConfigureClient(HttpClient client, HttpClientInvokingOptions options)
    {
        client.Timeout = TimeSpan.Parse(options.Timeout);

        client.DefaultRequestHeaders.UserAgent.TryParseAdd(options.UserAgent);

        return client;
    }

    /// <summary>
    /// 调用 HTTP 客户端。
    /// </summary>
    /// <param name="options">给定的 <see cref="HttpClientInvokingOptions"/>。</param>
    /// <returns>返回 <see cref="HttpClient"/>。</returns>
    public static HttpClient InvokeClient(HttpClientInvokingOptions options)
    {
        var client = options.MessageHandler is null
            ? new HttpClient()
            : new HttpClient(options.MessageHandler);

        return ConfigureClient(client, options);
    }

}

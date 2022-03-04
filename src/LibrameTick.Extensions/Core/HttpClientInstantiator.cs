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
/// 定义一个创建 <see cref="HttpClient"/> 的实例化器。
/// </summary>
public class HttpClientInstantiator : AbstractInstantiable<HttpClient, HttpClientOptions>
{
    /// <summary>
    /// 构造一个 <see cref="HttpClientInstantiator"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="HttpClientOptions"/>。</param>
    public HttpClientInstantiator(HttpClientOptions options)
        : base(options)
    {
    }


    /// <summary>
    /// 创建实例。
    /// </summary>
    /// <returns>返回 <see cref="HttpClient"/>。</returns>
    public override HttpClient Create()
    {
        var client = Options.MessageHandler is null
            ? new HttpClient()
            : new HttpClient(Options.MessageHandler);

        return Configure(client);
    }

    private HttpClient Configure(HttpClient client)
    {
        if (!string.IsNullOrEmpty(Options.Timeout))
            client.Timeout = TimeSpan.Parse(Options.Timeout);

        if (!string.IsNullOrEmpty(Options.UserAgent))
            client.DefaultRequestHeaders.UserAgent.TryParseAdd(Options.UserAgent);

        return client;
    }

}

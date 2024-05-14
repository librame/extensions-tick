#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Microparts;

///// <summary>
///// 定义一个创建 <see cref="HttpClient"/> 的微构件。
///// </summary>
//public class HttpClientMicropart : AbstractMicropart<HttpClientOptions, HttpClient>
//{
//    /// <summary>
//    /// 构造一个 <see cref="HttpClientMicropart"/>。
//    /// </summary>
//    /// <param name="options">给定的 <see cref="HttpClientOptions"/>。</param>
//    public HttpClientMicropart(HttpClientOptions options)
//        : base(options)
//    {
//    }


//    /// <summary>
//    /// 解开微构件。
//    /// </summary>
//    /// <returns>返回 <see cref="HttpClient"/>。</returns>
//    public override HttpClient Unwrap()
//    {
//        var client = Options.MessageHandler is null
//            ? new HttpClient()
//            : new HttpClient(Options.MessageHandler);

//        return Configure(client);
//    }

//    private HttpClient Configure(HttpClient client)
//    {
//        if (Options.Timeout != TimeSpan.Zero)
//            client.Timeout = Options.Timeout;

//        if (!string.IsNullOrEmpty(Options.UserAgent))
//            client.DefaultRequestHeaders.UserAgent.TryParseAdd(Options.UserAgent);

//        return client;
//    }

//}

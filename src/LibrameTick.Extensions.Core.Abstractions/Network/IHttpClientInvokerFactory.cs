#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Network;

/// <summary>
/// 定义 HTTP 客户端调用器工厂接口。
/// </summary>
public interface IHttpClientInvokerFactory
{
    /// <summary>
    /// 创建客户端调用。
    /// </summary>
    /// <param name="options">给定的 <see cref="HttpClientOptions"/>。</param>
    /// <returns>返回 <see cref="HttpClient"/>。</returns>
    HttpClient CreateClient(HttpClientOptions options);
}

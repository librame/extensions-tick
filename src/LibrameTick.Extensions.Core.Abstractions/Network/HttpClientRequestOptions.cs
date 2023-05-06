#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Microparts;

namespace Librame.Extensions.Network;

/// <summary>
/// 定义实现 <see cref="IRequestOptions"/> 的 HTTP 客户端请求选项。
/// </summary>
public class HttpClientRequestOptions : IRequestOptions
{
    /// <summary>
    /// HTTP 客户端选项。
    /// </summary>
    public HttpClientOptions HttpClient { get; set; } = new();


    /// <summary>
    /// 基础 URL。
    /// </summary>
    public string BaseUrl { get; set; } = "localhost";

    /// <summary>
    /// 验证标识。
    /// </summary>
    public string? VerifyId { get; set; }


    /// <summary>
    /// 端点选项列表。
    /// </summary>
    public List<HttpEndpointOptions> Endpoints { get; init; } = new();


    /// <summary>
    /// 添加端点选项。
    /// </summary>
    /// <param name="path">给定的端点路径。</param>
    /// <param name="action">给定要配置的 <see cref="HttpEndpointOptions"/> 动作（可选）。</param>
    /// <returns>返回 <see cref="HttpEndpointOptions"/>。</returns>
    public virtual HttpEndpointOptions AddEndpoint(string path,
        Action<HttpEndpointOptions>? action = null)
    {
        var endpoint = new HttpEndpointOptions(this, path);
        endpoint.Path = path;

        action?.Invoke(endpoint);
        Endpoints.Add(endpoint);

        return endpoint;
    }

}
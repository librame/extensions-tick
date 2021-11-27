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
/// 定义实现 <see cref="IOptionsNotifier"/> 的 HTTP 客户端请求选项。
/// </summary>
public class HttpClientRequestOptions : AbstractOptionsNotifier, IRequestOptionsNotifier
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="HttpClientRequestOptions"/>（此构造函数适用于独立使用 <see cref="HttpClientRequestOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public HttpClientRequestOptions(string sourceAliase)
        : base(sourceAliase)
    {
        HttpClientInvoking = new(Notifier);
    }

    /// <summary>
    /// 使用给定的 <see cref="IPropertyNotifier"/> 构造一个 <see cref="HttpClientRequestOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public HttpClientRequestOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
        HttpClientInvoking = new(Notifier);
    }


    /// <summary>
    /// HTTP 客户端调用选项。
    /// </summary>
    public HttpClientInvokingOptions HttpClientInvoking { get; init; }


    /// <summary>
    /// 基础 URL。
    /// </summary>
    public string BaseUrl
    {
        get => Notifier.GetOrAdd(nameof(BaseUrl), string.Empty);
        set => Notifier.AddOrUpdate(nameof(BaseUrl), value);
    }

    /// <summary>
    /// 验证标识。
    /// </summary>
    public string VerifyId
    {
        get => Notifier.GetOrAdd(nameof(VerifyId), string.Empty);
        set => Notifier.AddOrUpdate(nameof(VerifyId), value);
    }


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
    public virtual HttpEndpointOptions AddEndpoint(string path, Action<HttpEndpointOptions>? action = null)
    {
        var endpoint = new HttpEndpointOptions(this);
        endpoint.Path = path;

        action?.Invoke(endpoint);
        Endpoints.Add(endpoint);

        return endpoint;
    }

}
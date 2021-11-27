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
/// 定义实现 <see cref="IOptionsNotifier"/> 的 HTTP 端点选项。
/// </summary>
public class HttpEndpointOptions : AbstractOptionsNotifier
{
    private readonly IRequestOptionsNotifier _request;


    /// <summary>
    /// 使用给定的 <see cref="IPropertyNotifier"/> 构造一个 <see cref="HttpEndpointOptions"/>。
    /// </summary>
    /// <param name="request">给定的父级 <see cref="IRequestOptionsNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public HttpEndpointOptions(IRequestOptionsNotifier request, string? sourceAliase = null)
        : base(request.Notifier, sourceAliase)
    {
        _request = request;
    }


    /// <summary>
    /// 路径。
    /// </summary>
    public string Path
    {
        get => Notifier.GetOrAdd(nameof(Path), string.Empty);
        set => Notifier.AddOrUpdate(nameof(Path), value);
    }

    /// <summary>
    /// 等待超时（使用 <see cref="TimeSpan"/> 支持的字符串形式；默认不等待）。
    /// </summary>
    public string WaitTimeout
    {
        get => Notifier.GetOrAdd(nameof(WaitTimeout), string.Empty);
        set => Notifier.AddOrUpdate(nameof(WaitTimeout), value);
    }

    /// <summary>
    /// 方法（详情参考 <see cref="HttpClientMethods"/>）。
    /// </summary>
    public HttpClientMethods Method
    {
        get => Notifier.GetOrAdd(nameof(Method), HttpClientMethods.Get);
        set => Notifier.AddOrUpdate(nameof(Method), value);
    }

    /// <summary>
    /// 内容类型（详情参考 <see cref="HttpClientContentTypes"/>）。
    /// </summary>
    public HttpClientContentTypes ContentType
    {
        get => Notifier.GetOrAdd(nameof(ContentType), HttpClientContentTypes.FormUrlEncoded);
        set => Notifier.AddOrUpdate(nameof(ContentType), value);
    }

    /// <summary>
    /// 参数集合（即键值对集合。键值以英文等号分隔，多对以英文分号界定）。
    /// </summary>
    public string Parameters
    {
        get => Notifier.GetOrAdd(nameof(Parameters), string.Empty);
        set => Notifier.AddOrUpdate(nameof(Parameters), value);
    }

    /// <summary>
    /// 响应 Cookie 名称（如果不为空，则表示获取此值用于验证标识）。
    /// </summary>
    public string ResponseCookieName
    {
        get => Notifier.GetOrAdd(nameof(ResponseCookieName), string.Empty);
        set => Notifier.AddOrUpdate(nameof(ResponseCookieName), value);
    }

    /// <summary>
    /// 响应 Cookie 索引（当存在多个同名响应 Cookie 时，指定索引处的值用于验证标识）。
    /// </summary>
    public int ResponseCookieIndex
    {
        get => Notifier.GetOrAdd(nameof(ResponseCookieIndex), 0);
        set => Notifier.AddOrUpdate(nameof(ResponseCookieIndex), value);
    }

    /// <summary>
    /// 响应结束动作（参数为响应内容字符串）。
    /// </summary>
    [JsonIgnore]
    public Action<string>? ResponsedAction { get; set; }


    /// <summary>
    /// 获取参数的字典集合形式。
    /// </summary>
    /// <returns>返回 <see cref="Dictionary{TKey, TValue}"/>。</returns>
    public virtual Dictionary<string, string>? GetParameters()
    {
        if (string.IsNullOrEmpty(Parameters))
            return null;

        var parameters = new Dictionary<string, string>();

        var pairs = Parameters.Trim(';').Split(';');
        foreach (var pair in pairs)
        {
            if (pair.Contains('='))
            {
                var part = pair.Split('=');
                parameters.Add(part[0].Trim(), part[1].Trim());
            }
            else
            {
                parameters.Add(pair, string.Empty);
            }
        }

        return parameters;
    }


    /// <summary>
    /// 转为 URI 字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public virtual string ToUriString()
    {
        var sb = new StringBuilder();
        sb.Append(_request.BaseUrl);
        sb.Append(Path);

        if (string.IsNullOrEmpty(_request.VerifyId))
            return sb.ToString();

        if (Path.Contains('?'))
            sb.Append('&');
        else
            sb.Append('?');

        sb.Append(_request.VerifyId);
        return sb.ToString();
    }

    /// <summary>
    /// 转为 URI。
    /// </summary>
    /// <returns>返回 <see cref="Uri"/>。</returns>
    public virtual Uri ToUri()
        => new Uri(ToUriString());

}

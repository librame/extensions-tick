#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Network;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的 HTTP 端点选项。
/// </summary>
public class HttpEndpointOptions : IOptions
{
    private readonly IRequestOptions _requestOptions;


    /// <summary>
    /// 使用给定的 <see cref="IRequestOptions"/> 构造一个 <see cref="HttpEndpointOptions"/>。
    /// </summary>
    /// <param name="requestOptions">给定的父级 <see cref="IRequestOptions"/>。</param>
    /// <param name="path">给定的路径。</param>
    public HttpEndpointOptions(IRequestOptions requestOptions, string path)
    {
        _requestOptions = requestOptions;
        Path = path;
    }


    /// <summary>
    /// 路径。
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 等待超时（使用 <see cref="TimeSpan"/> 支持的字符串形式；默认不等待）。
    /// </summary>
    public string? WaitTimeout { get; set; }

    /// <summary>
    /// 方法（详情参考 <see cref="HttpClientMethods"/>）。
    /// </summary>
    public HttpClientMethods Method { get; set; }
        = HttpClientMethods.Get;

    /// <summary>
    /// 内容类型（详情参考 <see cref="HttpClientContentTypes"/>）。
    /// </summary>
    public HttpClientContentTypes ContentType { get; set; }
        = HttpClientContentTypes.FormUrlEncoded;

    /// <summary>
    /// 参数集合（即键值对集合。键值以英文等号分隔，多对以英文分号界定）。
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// 响应 Cookie 名称（如果不为空，则表示获取此值用于验证标识）。
    /// </summary>
    public string? ResponseCookieName { get; set; }

    /// <summary>
    /// 响应 Cookie 索引（当存在多个同名响应 Cookie 时，指定索引处的值用于验证标识）。
    /// </summary>
    public int ResponseCookieIndex { get; set; } = 0;

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
        sb.Append(_requestOptions.BaseUrl);
        sb.Append(Path);

        if (string.IsNullOrEmpty(_requestOptions.VerifyId))
            return sb.ToString();

        if (Path.Contains('?'))
            sb.Append('&');
        else
            sb.Append('?');

        sb.Append(_requestOptions.VerifyId);
        return sb.ToString();
    }

    /// <summary>
    /// 转为 URI。
    /// </summary>
    /// <returns>返回 <see cref="Uri"/>。</returns>
    public virtual Uri ToUri()
        => new Uri(ToUriString());

}

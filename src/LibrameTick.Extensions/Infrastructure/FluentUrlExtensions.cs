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
    /// 默认的 URI 方案为 HTTP。
    /// </summary>
    public const string DefaultScheme = "http";


    /// <summary>
    /// 设置绝对 URL。
    /// </summary>
    /// <param name="initialUrl">给定的初始 URL。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public static FluentUrl SetAbsoluteUrl(this string initialUrl)
        => new(new(initialUrl, UriKind.Absolute));

    /// <summary>
    /// 设置合并 URL。
    /// </summary>
    /// <param name="path">给定的路径。</param>
    /// <param name="host">给定的主机。</param>
    /// <param name="port">给定的端口。</param>
    /// <param name="scheme">给定的方案名称（可选；默认使用 <see cref="DefaultScheme"/>）。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public static FluentUrl SetCombineUrl(this string path, string host, int port, string? scheme = DefaultScheme)
        => new UriBuilder(scheme, host, port, path).SetUrl();

    /// <summary>
    /// 设置 URL。
    /// </summary>
    /// <param name="builder">给定的 <see cref="UriBuilder"/>。</param>
    /// <returns>返回 <see cref="FluentUrl"/>。</returns>
    public static FluentUrl SetUrl(this UriBuilder builder)
        => new(builder.Uri);

}

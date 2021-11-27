#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Storage;

/// <summary>
/// 定义抽象实现 <see cref="IWebFilePermission"/>。
/// </summary>
public abstract class AbstractWebFilePermission : IWebFilePermission
{
    private readonly WebFileOptions _options;


    /// <summary>
    /// 构造一个 <see cref="AbstractWebFilePermission"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="WebFileOptions"/>。</param>
    protected AbstractWebFilePermission(WebFileOptions options)
    {
        _options = options;
    }


    /// <summary>
    /// 异步获取访问令牌（通常由认证服务器下发）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public virtual Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunTask(() => _options.AuthAccessToken);

    /// <summary>
    /// 异步获取基础认证码（通常由用户名和密码组成）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public virtual Task<string> GetBasicCodeAsync(CancellationToken cancellationToken = default)
    {
        return cancellationToken.RunTask(() =>
        {
            return $"{_options.AuthUsername}:{_options.AuthPassword}"
                .FromEncodingString()
                .AsBase64String();
        });
    }

    /// <summary>
    /// 异步获取持票人认证令牌（如：JWT 认证）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public virtual Task<string> GetBearerTokenAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunTask(() => _options.AuthJwtToken);

    /// <summary>
    /// 异步获取 Cookie 值。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public virtual Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunTask(() => "CookieValue");

}

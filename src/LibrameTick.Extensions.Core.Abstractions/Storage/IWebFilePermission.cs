#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Storage;

/// <summary>
/// 定义文件权限接口。
/// </summary>
public interface IWebFilePermission
{
    /// <summary>
    /// 异步获取访问令牌（通常由认证服务器下发）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    ValueTask<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步获取基础认证码（通常由用户名和密码组成）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    ValueTask<string?> GetBasicCodeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步获取持票人认证令牌（如：JWT 认证）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    ValueTask<string?> GetBearerTokenAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步获取 Cookie 值。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    ValueTask<string?> GetCookieValueAsync(CancellationToken cancellationToken = default);
}

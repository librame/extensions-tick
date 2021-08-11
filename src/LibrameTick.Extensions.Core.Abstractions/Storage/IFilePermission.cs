#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Storage
{
    /// <summary>
    /// 定义文件权限接口。
    /// </summary>
    public interface IFilePermission
    {
        /// <summary>
        /// 获取访问令牌。
        /// </summary>
        /// <returns>返回字符串。</returns>
        string GetAccessToken();

        /// <summary>
        /// 异步获取访问令牌。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取授权码。
        /// </summary>
        /// <returns>返回字符串。</returns>
        string GetAuthorizationCode();

        /// <summary>
        /// 异步获取授权码。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> GetAuthorizationCodeAsync(CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取 Cookie 值。
        /// </summary>
        /// <returns>返回字符串。</returns>
        string GetCookieValue();

        /// <summary>
        /// 异步获取 Cookie 值。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default);
    }
}

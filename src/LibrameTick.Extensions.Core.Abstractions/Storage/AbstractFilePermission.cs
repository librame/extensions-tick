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
    /// 定义抽象实现 <see cref="IFilePermission"/>。
    /// </summary>
    public abstract class AbstractFilePermission : IFilePermission
    {
        private readonly RequestOptions _options;


        /// <summary>
        /// 构造一个 <see cref="AbstractFilePermission"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="RequestOptions"/>。</param>
        protected AbstractFilePermission(RequestOptions options)
        {
            _options = options;
        }


        /// <summary>
        /// 获取访问令牌。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetAccessToken()
            => _options.AccessTokenLength.GenerateByteArray().AsBase64String();

        /// <summary>
        /// 异步获取访问令牌。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(GetAccessToken);


        /// <summary>
        /// 获取授权码。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetAuthorizationCode()
            => _options.AuthorizationCodeLength.GenerateByteArray().AsBase64String();

        /// <summary>
        /// 异步获取授权码。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetAuthorizationCodeAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(GetAuthorizationCode);


        /// <summary>
        /// 获取 Cookie 值。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public virtual string GetCookieValue()
            => _options.CookieValueLength.GenerateByteArray().AsBase64String();

        /// <summary>
        /// 异步获取 Cookie 值。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含字符串的异步操作。</returns>
        public virtual Task<string> GetCookieValueAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(GetCookieValue);

    }
}

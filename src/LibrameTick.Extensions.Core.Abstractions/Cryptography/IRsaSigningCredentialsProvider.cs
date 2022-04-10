#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义继承 <see cref="ISigningCredentialsProvider"/> 的 RSA 签名证书提供程序接口。
/// </summary>
public interface IRsaSigningCredentialsProvider : ISigningCredentialsProvider
{
    /// <summary>
    /// 从签名证书中加载 RSA。
    /// </summary>
    /// <param name="credentials">给定的 <see cref="SigningCredentials"/>（可选；默认使用 <see cref="ISigningCredentialsProvider.Load()"/> 加载签名证书）。</param>
    /// <returns>返回 <see cref="RSA"/>。</returns>
    RSA LoadRsa(SigningCredentials? credentials = null);
}

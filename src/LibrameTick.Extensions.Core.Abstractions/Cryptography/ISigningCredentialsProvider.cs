#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Cryptography;

/// <summary>
/// 定义签名证书提供程序接口。
/// </summary>
public interface ISigningCredentialsProvider
{
    /// <summary>
    /// 加载签名证书。
    /// </summary>
    /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
    SigningCredentials? Load();

    /// <summary>
    /// 验证签名证书。
    /// </summary>
    /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
    /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
    SigningCredentials Verify(SigningCredentials credentials);
}

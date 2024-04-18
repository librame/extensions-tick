#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies;

/// <summary>
/// 定义抽象实现 <see cref="ISigningCredentialsProvider"/> 的签名证书提供程序。
/// </summary>
public abstract class AbstractSigningCredentialsProvider : ISigningCredentialsProvider
{
    /// <summary>
    /// 加载签名证书。
    /// </summary>
    /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
    public abstract SigningCredentials? Load();


    /// <summary>
    /// 验证签名证书。
    /// </summary>
    /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
    /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
    public virtual SigningCredentials Verify(SigningCredentials credentials)
    {
        if (!(credentials.Key is AsymmetricSecurityKey
            || credentials.Key is JsonWebKey && ((JsonWebKey)credentials.Key).HasPrivateKey))
        {
            throw new InvalidOperationException("Invalid signing key.");
        }

        return credentials;
    }

}

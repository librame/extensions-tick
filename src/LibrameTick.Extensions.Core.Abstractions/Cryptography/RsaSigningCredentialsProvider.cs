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
/// 定义继承 <see cref="AbstractSigningCredentialsProvider"/> 的 RSA 签名证书提供程序。
/// </summary>
public class RsaSigningCredentialsProvider : AbstractSigningCredentialsProvider, IRsaSigningCredentialsProvider
{
    private readonly IRsaKeyProvider _rsaKeyProvider;


    /// <summary>
    /// 构造一个 <see cref="RsaSigningCredentialsProvider"/>。
    /// </summary>
    /// <param name="rsaKeyProvider">给定的 <see cref="IRsaKeyProvider"/>。</param>
    public RsaSigningCredentialsProvider(IRsaKeyProvider rsaKeyProvider)
    {
        _rsaKeyProvider = rsaKeyProvider;
    }


    /// <summary>
    /// 加载签名证书。
    /// </summary>
    /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
    public override SigningCredentials? Load()
    {
        // 如果不存在 RSA 密钥
        if (!_rsaKeyProvider.Exist())
            return null; // 则不新生成直接返回

        var rsaKey = _rsaKeyProvider.Load();

        var credentials = new SigningCredentials(rsaKey.ToRsaSecurityKey(),
            SecurityAlgorithms.RsaSha256);

        return Verify(credentials);
    }


    /// <summary>
    /// 从签名证书中加载 RSA。
    /// </summary>
    /// <param name="credentials">给定的 <see cref="SigningCredentials"/>（可选；默认使用 <see cref="ISigningCredentialsProvider.Load()"/> 加载签名证书）。</param>
    /// <returns>返回 <see cref="RSA"/>。</returns>
    public virtual RSA LoadRsa(SigningCredentials? credentials = null)
    {
        if (credentials is null)
            credentials = Load();

        if (credentials is null)
            throw new ArgumentNullException(nameof(credentials));

        if (credentials.Key is X509SecurityKey x509Key)
            return (RSA)x509Key.PrivateKey; // x509Key.Certificate

        if (credentials.Key is RsaSecurityKey rsaKey)
        {
            if (rsaKey.Rsa is null)
            {
                var rsa = RSA.Create();
                rsa.ImportParameters(rsaKey.Parameters);

                return rsa;
            }

            return rsaKey.Rsa;
        }

        throw new NotSupportedException($"Not supported signing credentials.");
    }

}

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
/// 定义非对称数字证书对。
/// </summary>
public sealed class AsymmetricCertificatePair
{
    /// <summary>
    /// 构造一个 <see cref="AsymmetricCertificatePair"/> 实例。
    /// </summary>
    /// <param name="pvtCert">给定的私有证书。</param>
    /// <param name="pubCert">给定的公有证书。</param>
    /// <exception cref="ArgumentException">
    /// The private and public certificates cannot be null at the same time.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The issuer of the private and public certificates are different.
    /// </exception>
    public AsymmetricCertificatePair(X509Certificate2? pvtCert, X509Certificate2? pubCert)
    {
        if (pvtCert is null && pubCert is null)
        {
            throw new ArgumentException("The private and public certificates cannot be null at the same time.");
        }
        else if (pvtCert is not null && pubCert is not null && !pvtCert.Issuer.Equals(pubCert.Issuer, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The issuer of the private and public certificates are different.");
        }

        PrivateCert = pvtCert;
        PublicCert = pubCert;
    }


    /// <summary>
    /// 获取私有证书。
    /// </summary>
    public X509Certificate2? PrivateCert { get; init; }

    /// <summary>
    /// 获取公共证书。
    /// </summary>
    public X509Certificate2? PublicCert { get; init; }


    /// <summary>
    /// 获取必要的私有证书。
    /// </summary>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="PrivateCert"/> is null.
    /// </exception>
    public X509Certificate2 GetRequiredPrivateCert()
    {
        var pvtCert = PrivateCert;
        ArgumentNullException.ThrowIfNull(pvtCert, nameof(PrivateCert));

        return pvtCert;
    }

    /// <summary>
    /// 获取必要的公有证书。
    /// </summary>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="PublicCert"/> is null.
    /// </exception>
    public X509Certificate2 GetRequiredPublicCert()
    {
        var pubCert = PublicCert;
        ArgumentNullException.ThrowIfNull(pubCert, nameof(PublicCert));

        return pubCert;
    }

}

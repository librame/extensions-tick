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
/// 定义继承 <see cref="CertificateFilePersistenceProvider"/> 的 RSA 私有数字证书文件持久化提供程序。
/// </summary>
/// <param name="filePath">给定的文件路径。</param>
/// <param name="password">给定的安全密码。</param>
/// <param name="subjectName">给定的 <see cref="X500DistinguishedName"/> 主题名称。</param>
/// <param name="hashName">给定的 <see cref="HashAlgorithmName"/>。</param>
/// <param name="sigPadding">给定的 <see cref="RSASignaturePadding"/>。</param>
/// <param name="expiredFunc">给定过期的方法。</param>
public sealed class RsaPrivateCertificateFilePersistenceProvider(string filePath, SecureString? password, X500DistinguishedName subjectName,
    HashAlgorithmName hashName, RSASignaturePadding sigPadding, Func<DateTimeOffset, DateTimeOffset> expiredFunc)
    : CertificateFilePersistenceProvider(X509ContentType.Pfx, password, filePath, () => InitializeCert(subjectName, hashName, sigPadding, expiredFunc))
{

    private static X509Certificate2 InitializeCert(X500DistinguishedName subjectName, HashAlgorithmName hashName,
        RSASignaturePadding sigPadding, Func<DateTimeOffset, DateTimeOffset> expiredFunc)
    {
        var rsa = RSA.Create();
        var req = new CertificateRequest(subjectName, rsa, hashName, sigPadding);

        var now = DateTimeOffset.Now;
        var cert = req.CreateSelfSigned(now, expiredFunc(now));

        return cert;
    }

}

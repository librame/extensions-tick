#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies.Cryptography;

/// <summary>
/// 定义数字证书生成。
/// </summary>
public static class CertificateGeneration
{

    /// <summary>
    /// 检测证书是否已过期。
    /// </summary>
    /// <param name="certificate">给定的 <see cref="X509Certificate2"/>。</param>
    /// <returns>返回是否已过期的布尔值。</returns>
    /// <exception cref="InvalidOperationException">
    /// The certificate has expired.
    /// </exception>
    public static void ThrowIfExpired(X509Certificate2 certificate)
    {
        if (certificate.NotAfter <= DateTime.Now)
        {
            throw new InvalidOperationException($"The certificate '{certificate.FriendlyName}' has expired.");
        }
    }


    /// <summary>
    /// 导出公钥文件。文件扩展名通常为 .cer/.crt。
    /// </summary>
    /// <param name="fileName">给定的文件名。</param>
    /// <param name="certificate">给定的 <see cref="X509Certificate2"/>。</param>
    public static void ExportPublicKey(string fileName, X509Certificate2 certificate)
    {
        // Create Base 64 encoded CER (public key only)
        File.WriteAllText(fileName, BuildPublicKeyString(certificate.Export(X509ContentType.Cert)));


        // 构建公钥文件内容
        static string BuildPublicKeyString(byte[] bytes)
        {
            var sb = new StringBuilder();

            sb.AppendLine("-----BEGIN CERTIFICATE-----");
            sb.AppendLine(Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks));
            sb.AppendLine("-----END CERTIFICATE-----");

            return sb.ToString();
        }
    }

    /// <summary>
    /// 导出私钥文件。文件扩展名通常为 .pfx/.p12。
    /// </summary>
    /// <param name="fileName">给定的文件名。</param>
    /// <param name="certificate">给定的 <see cref="X509Certificate2"/>。</param>
    /// <param name="password">给定的 <see cref="SecureString"/>。</param>
    public static void ExportPrivateKey(string fileName, X509Certificate2 certificate, SecureString? password)
    {
        // Create PFX (PKCS #12) with private key
        File.WriteAllBytes(fileName, certificate.Export(X509ContentType.Pfx, password));
    }


    #region CreateECDSACertificate

    /// <summary>
    /// 创建基于 Librame 组织的 ECDSA 自签名数字证书。
    /// </summary>
    /// <param name="commonName">给定的公共名称。</param>
    /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
    /// <param name="expiredFunc">给定基于当前时间的证书无效结束日期方法。</param>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    public static X509Certificate2 CreateLibrameECDsaCertificate(string commonName, CultureInfo culture,
        Func<DateTimeOffset, DateTimeOffset> expiredFunc)
    {
        var subjectName = CreateLibrameSubjectName(commonName, culture);

        return CreateECDsaCertificate(subjectName, expiredFunc);
    }

    /// <summary>
    /// 创建 ECDSA 自签名数字证书。
    /// </summary>
    /// <param name="subjectName">给定的 <see cref="X500DistinguishedName"/> 主题名称。</param>
    /// <param name="expiredFunc">给定基于当前时间的证书无效结束日期方法。</param>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    public static X509Certificate2 CreateECDsaCertificate(X500DistinguishedName subjectName,
        Func<DateTimeOffset, DateTimeOffset> expiredFunc)
    {
        var now = DateTimeOffset.Now;

        return CreateECDsaCertificate(subjectName, now, expiredFunc(now));
    }

    /// <summary>
    /// 创建 ECDSA 自签名数字证书。
    /// </summary>
    /// <param name="subjectName">给定的 <see cref="X500DistinguishedName"/> 主题名称。</param>
    /// <param name="notBefore">给定证书生效的开始日期。</param>
    /// <param name="notAfter">给定证书无效的结束日期。</param>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    public static X509Certificate2 CreateECDsaCertificate(X500DistinguishedName subjectName,
        DateTimeOffset notBefore, DateTimeOffset notAfter)
    {
        var ecdsa = ECDsa.Create();
        var req = new CertificateRequest(subjectName, ecdsa, HashAlgorithmName.SHA256);

        var cert = req.CreateSelfSigned(notBefore, notAfter);
        return cert;
    }


    /// <summary>
    /// 创建基于 Librame 组织的数字证书主题名称。
    /// </summary>
    /// <param name="commonName">给定的公共名称。</param>
    /// <param name="culture">给定的 <see cref="CultureInfo"/>。</param>
    /// <returns>返回 <see cref="X500DistinguishedName"/>。</returns>
    private static X500DistinguishedName CreateLibrameSubjectName(string commonName, CultureInfo culture)
    {
        var cultureCode = culture.TwoLetterISOLanguageName.ToUpperInvariant();

        return CreateLibrameSubjectName(commonName, cultureCode);
    }

    /// <summary>
    /// 创建基于 Librame 组织的数字证书主题名称。
    /// </summary>
    /// <param name="commonName">给定的公共名称。</param>
    /// <param name="cultureCode">给定的文化区域代码。</param>
    /// <returns>返回 <see cref="X500DistinguishedName"/>。</returns>
    private static X500DistinguishedName CreateLibrameSubjectName(string commonName, string cultureCode)
    {
        // 分隔符后跟空格：[X500：/C=CountryCode(=2)/ O=Organization(<=64)/ OU=OrganizationUnit(<=32)/ CN=CommonName(<=64)]
        // [X500：/C=US/ O=Microsoft/ OU=WGA/ CN=TedSt]
        return new($"C={cultureCode}/ O=Librame/ OU=Librame/ CN={commonName}");
    }

    #endregion

}

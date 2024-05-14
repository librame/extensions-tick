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
/// 定义继承 <see cref="CertificateFilePersistenceProvider"/> 的公有数字证书文件持久化提供程序。
/// </summary>
/// <param name="filePath">给定的文件路径。</param>
/// <param name="pvtCert">给定的私钥证书。</param>
public sealed class PublicCertificateFilePersistenceProvider(string filePath, X509Certificate2 pvtCert)
    : CertificateFilePersistenceProvider(X509ContentType.Cert, password: null, filePath, () => pvtCert)
{
}

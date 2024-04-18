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
/// 定义继承 <see cref="AbstractFilePersistenceProvider{X509Certificate2}"/> 的数字证书文件持久化提供程序。
/// </summary>
/// <param name="contentType">给定的内容类型。</param>
/// <param name="password">给定的安全密码。</param>
/// <param name="filePath">给定的文件路径。</param>
/// <param name="encoding">给定的字符编码。</param>
/// <param name="initialFunc">给定的实例初始方法。</param>
public class CertificateFilePersistenceProvider(X509ContentType contentType, SecureString? password,
    string filePath, Encoding encoding, Func<X509Certificate2> initialFunc)
    : AbstractFilePersistenceProvider<X509Certificate2>(isWatching: false, filePath, encoding, initialFunc)
{
    /// <summary>
    /// 获取内容类型。
    /// </summary>
    public X509ContentType ContentType => contentType;

    /// <summary>
    /// 获取安全密码。
    /// </summary>
    public SecureString? Password => password;


    /// <summary>
    /// 加载实例。
    /// </summary>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    public override X509Certificate2 Load()
    {
        if (ContentType == X509ContentType.Pfx)
        {
            return new(FilePath, Password);
        }
        else
        {
            return new(FilePath);
        }
    }

    /// <summary>
    /// 保存实例。
    /// </summary>
    /// <param name="persistence">给定的 <see cref="X509Certificate2"/>。</param>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    public override X509Certificate2 Save(X509Certificate2 persistence)
    {
        if (ContentType == X509ContentType.Pfx)
        {
            CertificateGeneration.ExportPrivateKey(FilePath, persistence, Password);
        }
        else
        {
            CertificateGeneration.ExportPublicKey(FilePath, persistence);
        }

        return persistence;
    }

}

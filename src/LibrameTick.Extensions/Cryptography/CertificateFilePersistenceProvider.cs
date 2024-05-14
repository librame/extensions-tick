#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure.Persistence;

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义继承 <see cref="AbstractFilePersistenceProvider{X509Certificate2}"/> 的数字证书文件持久化提供程序。
/// </summary>
/// <param name="contentType">给定的内容类型。</param>
/// <param name="password">给定的安全密码。</param>
/// <param name="filePath">给定的文件路径。</param>
/// <param name="initialFunc">给定的实例初始方法。</param>
public class CertificateFilePersistenceProvider(X509ContentType contentType, SecureString? password,
    string filePath, Func<X509Certificate2> initialFunc)
    : AbstractFilePersistenceProvider<X509Certificate2>(isWatching: false, filePath, encoding: null, initialFunc)
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
        X509Certificate2 cert;

        if (ContentType == X509ContentType.Pfx)
        {
            cert = LoadPrivateKey();
        }
        else
        {
            cert = LoadPublicKey();
        }

        if (cert.NotAfter <= DateTime.Now)
        {
            // 证书过期则重新生成
            cert = Save(InitialFunc());
        }

        return cert;
    }

    /// <summary>
    /// 加载私钥。
    /// </summary>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    protected virtual X509Certificate2 LoadPrivateKey()
        => new(FilePath, Password);

    /// <summary>
    /// 加载公钥。
    /// </summary>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    protected virtual X509Certificate2 LoadPublicKey()
        => new(FilePath);


    /// <summary>
    /// 保存实例。
    /// </summary>
    /// <param name="persistence">给定的 <see cref="X509Certificate2"/>。</param>
    /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
    public override X509Certificate2 Save(X509Certificate2 persistence)
    {
        if (ContentType == X509ContentType.Pfx)
        {
            SavePrivateKey(persistence);
        }
        else
        {
            SavePublicKey(persistence);
        }

        return persistence;
    }

    /// <summary>
    /// 保存私钥。
    /// </summary>
    /// <param name="persistence">给定的 <see cref="X509Certificate2"/>。</param>
    protected virtual void SavePrivateKey(X509Certificate2 persistence)
        => File.WriteAllBytes(FilePath, persistence.Export(X509ContentType.Pfx, Password));

    /// <summary>
    /// 保存公钥。
    /// </summary>
    /// <param name="persistence">给定的 <see cref="X509Certificate2"/>。</param>
    protected virtual void SavePublicKey(X509Certificate2 persistence)
        => File.WriteAllText(FilePath, persistence.ExportCertificatePem());

}

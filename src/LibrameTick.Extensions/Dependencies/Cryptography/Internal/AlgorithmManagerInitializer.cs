#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies.Cryptography.Internal;

internal sealed class AlgorithmManagerInitializer : IExtensionsDependencyInitializer<IAlgorithmManager>
{

    public IAlgorithmManager Initialize(IExtensionsDependency dependency)
    {
        var keyring = GetOrCreateAlgorithmKeyring(dependency);

        var ecdsaCert = GetOrCreateECDsaCertificate(dependency, keyring);

        return new AlgorithmManager(keyring, ecdsaCert);
    }

    private static AlgorithmKeyring GetOrCreateAlgorithmKeyring(IExtensionsDependency dependency)
    {
        var fileName = BuildAssemblyFileName(dependency, ".keyring");

        var provider = new JsonFilePersistenceProvider<AlgorithmKeyring>(fileName, dependency.Encoding, GetDefaultKeyring);

        return provider.Current;


        // 获取默认密码密钥环
        static AlgorithmKeyring GetDefaultKeyring()
        {
            var keyring = new AlgorithmKeyring();

            keyring.GenerateAll();

            return keyring;
        }
    }


    private static X509Certificate2? _defaultECDsaCertificate;

    private static X509Certificate2 GetOrCreateECDsaCertificate(IExtensionsDependency dependency, AlgorithmKeyring keyring)
    {
        // Create PFX (PKCS #12) with private key
        var pfxFileName = BuildAssemblyFileName(dependency, ".pfx");
        var privateKeyProvider = new CertificateFilePersistenceProvider(X509ContentType.Pfx, keyring.ExportSecureString(),
            pfxFileName, dependency.Encoding, () => GetDefaultECDsaCertificate(dependency));

        CertificateGeneration.ThrowIfExpired(privateKeyProvider.Current);

        // Create Base 64 encoded CER (public key only)
        var cerFileName = Path.ChangeExtension(pfxFileName, ".cer");
        if (!File.Exists(cerFileName))
        {
            // 同时导出公钥备用
            CertificateGeneration.ExportPublicKey(cerFileName, privateKeyProvider.Current);
        }

        return privateKeyProvider.Current;


        // 生成默认 ECDSA 数字证书
        static X509Certificate2 GetDefaultECDsaCertificate(IExtensionsDependency dependency)
        {
            if (_defaultECDsaCertificate is null)
            {
                // 通用名称不支持任何分隔符
                var commonName = dependency.AssemblyNameString.Replace(".", newValue: null);

                // 创建默认三年过期的自签名数字证书
                _defaultECDsaCertificate = CertificateGeneration.CreateLibrameECDsaCertificate(commonName, CultureInfo.CurrentCulture,
                    expiredFunc: now => now.AddYears(3));
            }

            return _defaultECDsaCertificate;
        }
    }

    private static string BuildAssemblyFileName(IExtensionsDependency dependency, string extension)
        => $"{dependency.PathManager.InitialPath}{Path.DirectorySeparatorChar}{dependency.AssemblyNameString}{extension}";

}

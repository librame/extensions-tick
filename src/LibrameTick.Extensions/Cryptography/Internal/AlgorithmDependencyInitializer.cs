#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Cryptography.Internal;

internal sealed class AlgorithmDependencyInitializer : IDependencyInitializer<IAlgorithmDependency>
{

    public IAlgorithmDependency Initialize(IDependencyContext context, DependencyCharacteristic characteristic)
    {
        var keyring = InitializeAlgorithmKeyring(context);

        keyring.Rsa = InitializeRsaCertPair(context, keyring);
        keyring.Ecdsa = InitializeEcdsaCertPair(context, keyring);

        return new AlgorithmDependency(new(keyring));
    }


    private static AlgorithmKeyring InitializeAlgorithmKeyring(IDependencyContext context)
    {
        var filePath = AlgorithmFilePath.BuildAlgorithmKeyringFilePath(context);
        var provider = new AlgorithmKeyringJsonFilePersistenceProvider(filePath, context.Encoding);

        return provider.Current;
    }

    private static AsymmetricCertificatePair InitializeRsaCertPair(IDependencyContext context, AlgorithmKeyring keyring)
    {
        var filePath = AlgorithmFilePath.BuildAsymmetricPrivateCertFilePath(context);

        var commonName = Path.GetFileNameWithoutExtension(filePath);
        var subjectName = GetLibrameSubjectName(commonName, CultureInfo.CurrentCulture);

        var provider = new RsaPrivateCertificateFilePersistenceProvider(filePath, keyring.ExportSecureString(),
            subjectName, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1, InitializeCertExpired);

        var pubFilePath = AlgorithmFilePath.BuildPublicCertFilePath(filePath);
        var pubProvider = new PublicCertificateFilePersistenceProvider(pubFilePath, provider.Current);

        return new(provider.Current, pubProvider.Current);
    }

    private static AsymmetricCertificatePair InitializeEcdsaCertPair(IDependencyContext context, AlgorithmKeyring keyring)
    {
        var filePath = AlgorithmFilePath.BuildSignaturePrivateCertFilePath(context);

        var commonName = Path.GetFileNameWithoutExtension(filePath);
        var subjectName = GetLibrameSubjectName(commonName, CultureInfo.CurrentCulture);

        var provider = new EcdsaPrivateCertificateFilePersistenceProvider(filePath, keyring.ExportSecureString(),
            subjectName, HashAlgorithmName.SHA256, InitializeCertExpired);

        var pubFilePath = AlgorithmFilePath.BuildPublicCertFilePath(filePath);
        var pubProvider = new PublicCertificateFilePersistenceProvider(pubFilePath, provider.Current);

        return new(provider.Current, pubProvider.Current);
    }

    private static X500DistinguishedName GetLibrameSubjectName(string commonName, CultureInfo culture)
    {
        var cultureCode = culture.TwoLetterISOLanguageName.ToUpperInvariant();

        return GetLibrameSubjectName(commonName, cultureCode);
    }

    private static X500DistinguishedName GetLibrameSubjectName(string commonName, string cultureCode)
    {
        // 分隔符后跟空格：[X500：/C=CountryCode(=2)/ O=Organization(<=64)/ OU=OrganizationUnit(<=32)/ CN=CommonName(<=64)]
        // [X500：/C=US/ O=Microsoft/ OU=WGA/ CN=TedSt]
        return new($"C={cultureCode}/ O=Librame/ OU=Librame.Extensions/ CN={commonName}");
    }

    private static DateTimeOffset InitializeCertExpired(DateTimeOffset now)
        => now.AddYears(3); // 初始证书默认三年过期

}

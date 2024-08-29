#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Cryptography.Internal;

internal sealed class AlgorithmDependency(AlgorithmKeyring keyring) : Disposable, IAlgorithmDependency
{
    private readonly ConcurrentDictionary<string, IDisposable> _disposables = new();


    public AlgorithmKeyring Keyring { get; init; } = keyring;


    #region Hash

    public Lazy<MD5> LazyMd5 => GetKeyringLazy(keys => MD5.Create());

    public Lazy<SHA1> LazySha1 => GetKeyringLazy(keys => SHA1.Create());

    public Lazy<SHA256> LazySha256 => GetKeyringLazy(keys => SHA256.Create());

    public Lazy<SHA384> LazySha384 => GetKeyringLazy(keys => SHA384.Create());

    public Lazy<SHA512> LazySha512 => GetKeyringLazy(keys => SHA512.Create());

    #endregion


    #region HMAC Hash

    public Lazy<HMACMD5> LazyHmacMd5 => GetKeyringLazy(keys => new HMACMD5(keys.HmacHash.Md5.Key));

    public Lazy<HMACSHA1> LazyHmacSha1 => GetKeyringLazy(keys => new HMACSHA1(keys.HmacHash.Sha1.Key));

    public Lazy<HMACSHA256> LazyHmacSha256 => GetKeyringLazy(keys => new HMACSHA256(keys.HmacHash.Sha256.Key));

    public Lazy<HMACSHA384> LazyHmacSha384 => GetKeyringLazy(keys => new HMACSHA384(keys.HmacHash.Sha384.Key));

    public Lazy<HMACSHA512> LazyHmacSha512 => GetKeyringLazy(keys => new HMACSHA512(keys.HmacHash.Sha512.Key));

    #endregion


    #region DES

    public Lazy<TripleDES> Lazy3Des => GetKeyringLazy(Initial3Des);


    private static TripleDES Initial3Des(AlgorithmKeyring keyring)
    {
        var des = TripleDES.Create();

        des.Key = keyring.Des.Key;
        des.IV = keyring.Des.Nonce;

        return des;
    }

    #endregion


    #region AES

    public Lazy<AesCcm> LazyAesCcm => GetKeyringLazy(keys => new AesCcm(keys.AesCcm.Key));

    public Lazy<AesGcm> LazyAesGcm => GetKeyringLazy(keys => new AesGcm(keys.AesGcm.Key, keys.AesGcm.Tag.Length));

    public Lazy<Aes> LazyAes => GetKeyringLazy(InitialAes);


    private static Aes InitialAes(AlgorithmKeyring keyring)
    {
        var aes = Aes.Create();

        aes.Key = keyring.Aes.Key;
        aes.IV = keyring.Aes.Nonce;

        return aes;
    }

    #endregion


    #region RSA

    public Lazy<AsymmetricAlgorithmPair<RSA>> LazyRsaPair => GetKeyringLazy(InitialRsaPair);


    private static AsymmetricAlgorithmPair<RSA> InitialRsaPair(AlgorithmKeyring keyring)
    {
        // 需同时获取 RSA 算法对，若各自独立延迟加载，将导致公钥加密后私钥解密失败“Unknown error (0xc100000d)”的情况
        var privateRsa = keyring.GetRequiredRsa().GetRequiredPrivateCert().GetRSAPrivateKey();
        var publicRsa = keyring.GetRequiredRsa().GetRequiredPublicCert().GetRSAPublicKey();

        ArgumentNullException.ThrowIfNull(privateRsa);
        ArgumentNullException.ThrowIfNull(publicRsa);

        return new(privateRsa, publicRsa);
    }

    #endregion


    #region ECDSA

    public Lazy<AsymmetricAlgorithmPair<ECDsaCng>> LazyEcdsaPair => GetKeyringLazy(InitialEcdsaPair);


    private static AsymmetricAlgorithmPair<ECDsaCng> InitialEcdsaPair(AlgorithmKeyring keyring)
    {
        var privateEcdsa = keyring.GetRequiredEcdsa().GetRequiredPrivateCert().GetECDsaPrivateKey();
        var publicEcdsa = keyring.GetRequiredEcdsa().GetRequiredPublicCert().GetECDsaPublicKey();

        ArgumentNullException.ThrowIfNull(privateEcdsa);
        ArgumentNullException.ThrowIfNull(publicEcdsa);

        return new((ECDsaCng)privateEcdsa, (ECDsaCng)publicEcdsa);
    }

    #endregion


    private Lazy<T> GetKeyringLazy<T>(Func<AlgorithmKeyring, T> func)
        where T : IDisposable
        => new((T)_disposables.GetOrAdd(GenerateKey<T>(), key => func(Keyring)));


    private static string GenerateKey<T>()
        => GenerateKey(typeof(T));

    private static string GenerateKey(Type type)
        => type.FullName ?? type.Name;


    protected override bool ReleaseManaged()
    {
        foreach (var disposable in _disposables)
        {
            disposable.Value?.Dispose();
        }

        _disposables.Clear();

        return true;
    }

}

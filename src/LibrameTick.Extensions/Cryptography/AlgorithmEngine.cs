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

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义支持哈希、对称加密、非对称加密等算法引擎。
/// </summary>
/// <param name="keyring">给定的 <see cref="AlgorithmKeyring"/>。</param>
public class AlgorithmEngine(AlgorithmKeyring keyring) : AbstractDisposable
{
    private readonly ConcurrentDictionary<string, IDisposable> _disposables = new();


    /// <summary>
    /// 获取算法密钥环。
    /// </summary>
    /// <value>
    /// 返回 <see cref="AlgorithmKeyring"/>。
    /// </value>
    public AlgorithmKeyring Keyring { get; init; } = keyring;


    #region Hash

    /// <summary>
    /// 获取延迟 MD5 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{MD5}"/>。
    /// </value>
    public Lazy<MD5> LazyMd5 => GetKeyringLazy(keys => MD5.Create());

    /// <summary>
    /// 获取延迟 SHA1 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{SHA1}"/>。
    /// </value>
    public Lazy<SHA1> LazySha1 => GetKeyringLazy(keys => SHA1.Create());

    /// <summary>
    /// 获取延迟 SHA256 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{SHA256}"/>。
    /// </value>
    public Lazy<SHA256> LazySha256 => GetKeyringLazy(keys => SHA256.Create());

    /// <summary>
    /// 获取延迟 SHA384 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{SHA384}"/>。
    /// </value>
    public Lazy<SHA384> LazySha384 => GetKeyringLazy(keys => SHA384.Create());

    /// <summary>
    /// 获取延迟 SHA512 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{SHA512}"/>。
    /// </value>
    public Lazy<SHA512> LazySha512 => GetKeyringLazy(keys => SHA512.Create());

    #endregion


    #region HMAC Hash

    /// <summary>
    /// 获取延迟 HMAC MD5 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{HMACMD5}"/>。
    /// </value>
    public Lazy<HMACMD5> LazyHmacMd5
        => GetKeyringLazy(keys => new HMACMD5(keys.HmacHash.Md5.Key));

    /// <summary>
    /// 获取延迟 HMAC SHA1 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{HMACSHA1}"/>。
    /// </value>
    public Lazy<HMACSHA1> LazyHmacSha1
        => GetKeyringLazy(keys => new HMACSHA1(keys.HmacHash.Sha1.Key));

    /// <summary>
    /// 获取延迟 HMAC SHA256 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{HMACSHA256}"/>。
    /// </value>
    public Lazy<HMACSHA256> LazyHmacSha256
        => GetKeyringLazy(keys => new HMACSHA256(keys.HmacHash.Sha256.Key));

    /// <summary>
    /// 获取延迟 HMAC SHA384 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{HMACSHA384}"/>。
    /// </value>
    public Lazy<HMACSHA384> LazyHmacSha384
        => GetKeyringLazy(keys => new HMACSHA384(keys.HmacHash.Sha384.Key));

    /// <summary>
    /// 获取延迟 HMAC SHA512 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{HMACSHA512}"/>。
    /// </value>
    public Lazy<HMACSHA512> LazyHmacSha512
        => GetKeyringLazy(keys => new HMACSHA512(keys.HmacHash.Sha512.Key));

    #endregion


    #region DES

    /// <summary>
    /// 获取延迟 TripleDES 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{TripleDES}"/>。
    /// </value>
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

    /// <summary>
    /// 获取延迟 AES CCM 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{AesCcm}"/>。
    /// </value>
    public Lazy<AesCcm> LazyAesCcm
        => GetKeyringLazy(keys => new AesCcm(keys.AesCcm.Key));

    /// <summary>
    /// 获取延迟 AES GCM 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{AesGcm}"/>。
    /// </value>
    public Lazy<AesGcm> LazyAesGcm
        => GetKeyringLazy(keys => new AesGcm(keys.AesGcm.Key, keys.AesGcm.Tag.Length));

    /// <summary>
    /// 获取延迟 AES 哈希算法实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{Aes}"/>。
    /// </value>
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

    /// <summary>
    /// 获取延迟 RSA 算法对实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{AsymmetricAlgorithmPair}"/>。
    /// </value>
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

    /// <summary>
    /// 获取延迟 ECDSA 算法对实例。
    /// </summary>
    /// <value>
    /// 返回 <see cref="Lazy{AsymmetricAlgorithmPair}"/>。
    /// </value>
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


    /// <summary>
    /// 释放已托管资源。
    /// </summary>
    /// <returns>返回是否释放的布尔值。</returns>
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

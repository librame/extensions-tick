#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// 算法静态扩展。
/// </summary>
public static class AlgorithmExtensions
{
    // 使用自动密钥作为默认的通用密钥基础设施（CKI）
    private static readonly Autokeys.CkiOptions DefaultCki
        = Bootstraps.Bootstrapper.GetAutokey().Get().Cki;


    /// <summary>
    /// 使用自动密钥作为默认的通用密钥基础设施（CKI）选项填充当前实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="Autokeys.CkiOptions"/>。</param>
    /// <returns>返回 <see cref="Autokeys.CkiOptions"/>。</returns>
    public static Autokeys.CkiOptions PopulateDefaultCki(this Autokeys.CkiOptions options)
    {
        options.PopulateAll(DefaultCki);
        return options;
    }


    #region Hash

    private static readonly Lazy<MD5> _md5 =
        new Lazy<MD5>(MD5.Create());

    private static readonly Lazy<SHA1> _sha1 =
        new Lazy<SHA1>(SHA1.Create());

    private static readonly Lazy<SHA256> _sha256 =
        new Lazy<SHA256>(SHA256.Create());

    private static readonly Lazy<SHA384> _sha384 =
        new Lazy<SHA384>(SHA384.Create());

    private static readonly Lazy<SHA512> _sha512 =
        new Lazy<SHA512>(SHA512.Create());


    /// <summary>
    /// 计算 MD5 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsMd5Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsMd5().AsBase64String();

    /// <summary>
    /// 计算 SHA1 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha1Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha1().AsBase64String();

    /// <summary>
    /// 计算 SHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha256Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha256().AsBase64String();

    /// <summary>
    /// 计算 SHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha384Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha384().AsBase64String();

    /// <summary>
    /// 计算 SHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsSha512Base64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsSha512().AsBase64String();


    /// <summary>
    /// 计算 MD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsMd5(this byte[] buffer)
        => _md5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha1(this byte[] buffer)
        => _sha1.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha256(this byte[] buffer)
        => _sha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha384(this byte[] buffer)
        => _sha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 SHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsSha512(this byte[] buffer)
        => _sha512.Value.ComputeHash(buffer);

    #endregion


    #region HMAC Hash

    private static readonly Lazy<HMACMD5> _hmacMd5 =
        new Lazy<HMACMD5>(() => new HMACMD5(DefaultCki.HmacHash.Md5.Key!));

    private static readonly Lazy<HMACSHA1> _hmacSha1 =
        new Lazy<HMACSHA1>(() => new HMACSHA1(DefaultCki.HmacHash.Sha1.Key!));

    private static readonly Lazy<HMACSHA256> _hmacSha256 =
        new Lazy<HMACSHA256>(() => new HMACSHA256(DefaultCki.HmacHash.Sha256.Key!));

    private static readonly Lazy<HMACSHA384> _hmacSha384 =
        new Lazy<HMACSHA384>(() => new HMACSHA384(DefaultCki.HmacHash.Sha384.Key!));

    private static readonly Lazy<HMACSHA512> _hmacSha512 =
        new Lazy<HMACSHA512>(() => new HMACSHA512(DefaultCki.HmacHash.Sha512.Key!));


    /// <summary>
    /// 计算 HMACMD5 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacMd5Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacMd5().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA1 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha1Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha1().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA256 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha256Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha256().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA384 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha384Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha384().AsBase64String();

    /// <summary>
    /// 计算 HMACSHA512 哈希值，并返回 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsHmacSha512Base64String(this string plaintext,
        Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsHmacSha512().AsBase64String();


    /// <summary>
    /// 计算 HMACMD5 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacMd5(this byte[] buffer)
        => _hmacMd5.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA1 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha1(this byte[] buffer)
        => _hmacSha1.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA256 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha256(this byte[] buffer)
        => _hmacSha256.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA384 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha384(this byte[] buffer)
        => _hmacSha384.Value.ComputeHash(buffer);

    /// <summary>
    /// 计算 HMACSHA512 哈希值。
    /// </summary>
    /// <param name="buffer">给定要计算的字节数组。</param>
    /// <returns>返回经过计算的字节数组。</returns>
    public static byte[] AsHmacSha512(this byte[] buffer)
        => _hmacSha512.Value.ComputeHash(buffer);

    #endregion


    #region AES

    private static readonly Lazy<Aes> _aes =
        new Lazy<Aes>(InitialAes);

    private static Aes InitialAes()
    {
        var aes = Aes.Create();

        aes.Key = DefaultCki.Aes.Key!;
        aes.IV = DefaultCki.Aes.Nonce!;

        return aes;
    }


    /// <summary>
    /// AES 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAes().AsBase64String();

    /// <summary>
    /// AES 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string FromAesWithBase64String(this string ciphertext, Encoding? encoding = null)
        => ciphertext.FromBase64String().FromAes().AsEncodingString(encoding);


    /// <summary>
    /// AES 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsAes(this byte[] plaintext)
    {
        var transform = _aes.Value.CreateEncryptor();
        return transform.TransformFinalBlock(plaintext, 0, plaintext.Length);
    }

    /// <summary>
    /// AES 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAes(this byte[] ciphertext)
    {
        var transform = _aes.Value.CreateDecryptor();
        return transform.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
    }

    #endregion


    #region AES-CCM

    private static readonly Lazy<AesCcm> _aesCcm =
        new Lazy<AesCcm>(() => new AesCcm(DefaultCki.AesCcm.Key!));


    /// <summary>
    /// AES-CCM 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesCcmWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAesCcm().AsBase64String();

    /// <summary>
    /// AES-CCM 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string FromAesCcmWithBase64String(this string ciphertext, Encoding? encoding = null)
        => ciphertext.FromBase64String().FromAesCcm().AsEncodingString(encoding);


    /// <summary>
    /// AES-CCM 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsAesCcm(this byte[] plaintext)
    {
        var ciphertext = new byte[plaintext.Length];

        _aesCcm.Value.Encrypt(DefaultCki.AesCcm.Nonce!,
            plaintext, ciphertext, DefaultCki.AesCcm.Tag!);

        return ciphertext;
    }

    /// <summary>
    /// AES-CCM 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAesCcm(this byte[] ciphertext)
    {
        var plaintext = new byte[ciphertext.Length];

        _aesCcm.Value.Decrypt(DefaultCki.AesCcm.Nonce!,
            ciphertext, DefaultCki.AesCcm.Tag!, plaintext);

        return plaintext;
    }

    #endregion


    #region AES-GCM

    private static readonly Lazy<AesGcm> _aesGcm =
        new Lazy<AesGcm>(() => new AesGcm(DefaultCki.AesGcm.Key!));


    /// <summary>
    /// AES-GCM 加密 BASE64 字符串形式。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 编码的加密字符串。</returns>
    public static string AsAesGcmWithBase64String(this string plaintext, Encoding? encoding = null)
        => plaintext.FromEncodingString(encoding).AsAesGcm().AsBase64String();

    /// <summary>
    /// AES-GCM 解密 BASE64 字符串形式。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="EncodingExtensions.UTF8Encoding"/>）。</param>
    /// <returns>返回经过 BASE64 解码的解密字符串。</returns>
    public static string FromAesGcmWithBase64String(this string ciphertext, Encoding? encoding = null)
        => ciphertext.FromBase64String().FromAesGcm().AsEncodingString(encoding);


    /// <summary>
    /// AES-GCM 加密。
    /// </summary>
    /// <param name="plaintext">给定的明文。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public static byte[] AsAesGcm(this byte[] plaintext)
    {
        var ciphertext = new byte[plaintext.Length];

        _aesGcm.Value.Encrypt(DefaultCki.AesGcm.Nonce!,
            plaintext, ciphertext, DefaultCki.AesGcm.Tag!);

        return ciphertext;
    }

    /// <summary>
    /// AES-GCM 解密。
    /// </summary>
    /// <param name="ciphertext">给定的密文。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public static byte[] FromAesGcm(this byte[] ciphertext)
    {
        var plaintext = new byte[ciphertext.Length];

        _aesGcm.Value.Decrypt(DefaultCki.AesGcm.Nonce!,
            ciphertext, DefaultCki.AesGcm.Tag!, plaintext);

        return plaintext;
    }

    #endregion

}

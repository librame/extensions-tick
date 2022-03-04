#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using CryptoAesCcm = System.Security.Cryptography.AesCcm;
using CryptoAesGcm = System.Security.Cryptography.AesGcm;

namespace Librame.Extensions.Autokeys;

/// <summary>
/// 定义一个通用密钥基础设施（CKI）选项。
/// </summary>
public class CkiOptions : Core.IOptions
{
    /// <summary>
    /// HMAC 哈希密钥选项。
    /// </summary>
    public HmacHashOptions HmacHash { get; set; } = new();

    /// <summary>
    /// AES 密钥选项。
    /// </summary>
    public KeyNonceOptions Aes { get; set; } = new();

    /// <summary>
    /// AES-CCM 密钥选项。
    /// </summary>
    public KeyNonceTagOptions AesCcm { get; set; } = new();

    /// <summary>
    /// AES-GCM 密钥选项。
    /// </summary>
    public KeyNonceTagOptions AesGcm { get; set; } = new();


    /// <summary>
    /// 生成通用密钥基础设施（CKI）所有密钥。
    /// </summary>
    public virtual void GenerateAll()
    {
        HmacHash.GenerateAll();

        // 以字节为单位，参数长度可以是 16、24 或 32 字节（128、192 或 256 位）
        Aes.Generate(256, 128);

        AesCcm.Generate(256,
            CryptoAesCcm.NonceByteSizes.MaxSize * 8, // 13
            CryptoAesCcm.TagByteSizes.MaxSize * 8); // 16

        AesGcm.Generate(256,
            CryptoAesGcm.NonceByteSizes.MaxSize * 8, // 12
            CryptoAesGcm.TagByteSizes.MaxSize * 8); // 16
    }

    /// <summary>
    /// 填充通用密钥基础设施（CKI）所有密钥。
    /// </summary>
    /// <param name="md5Key">给定的 MD5 密钥。</param>
    /// <param name="sha1Key">给定的 SHA1 密钥。</param>
    /// <param name="sha256Key">给定的 SHA256 密钥。</param>
    /// <param name="sha384Key">给定的 SHA384 密钥。</param>
    /// <param name="sha512Key">给定的 SHA512 密钥。</param>
    /// <param name="aesKey">给定的 AES 密钥。</param>
    /// <param name="aesNonce">给定的 AES 初始向量（IV）。</param>
    /// <param name="aesCcmKey">给定的 AES-CCM 密钥。</param>
    /// <param name="aesCcmNonce">给定的 AES-CCM 初始向量（IV）。</param>
    /// <param name="aesCcmTag">给定的 AES-CCM 验证标记。</param>
    /// <param name="aesGcmKey">给定的 AES-GCM 密钥。</param>
    /// <param name="aesGcmNonce">给定的 AES-GCM 初始向量（IV）。</param>
    /// <param name="aesGcmTag">给定的 AES-GCM 验证标记。</param>
    public virtual void PopulateAll(byte[] md5Key, byte[] sha1Key,
        byte[] sha256Key, byte[] sha384Key, byte[] sha512Key,
        byte[] aesKey, byte[] aesNonce,
        byte[] aesCcmKey, byte[] aesCcmNonce, byte[] aesCcmTag,
        byte[] aesGcmKey, byte[] aesGcmNonce, byte[] aesGcmTag)
    {
        HmacHash.PopulateAll(md5Key, sha1Key, sha256Key, sha384Key, sha512Key);

        Aes.Populate(aesKey, aesNonce);
        AesCcm.Populate(aesCcmKey, aesCcmNonce, aesCcmTag);
        AesGcm.Populate(aesGcmKey, aesGcmNonce, aesGcmTag);
    }

    /// <summary>
    /// 填充通用密钥基础设施（CKI）所有密钥。
    /// </summary>
    /// <param name="options">给定的 <see cref="CkiOptions"/>。</param>
    public virtual void PopulateAll(CkiOptions options)
    {
        HmacHash.PopulateAll(options.HmacHash);

        Aes.Populate(options.Aes);
        AesCcm.Populate(options.AesCcm);
        AesGcm.Populate(options.AesGcm);
    }


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(HmacHash)}:{HmacHash}|{nameof(Aes)}:{Aes};{nameof(AesCcm)}:{AesCcm};{nameof(AesGcm)}:{AesGcm}";

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using SysAesCcm = System.Security.Cryptography.AesCcm;
using SysAesGcm = System.Security.Cryptography.AesGcm;

namespace Librame.Extensions.Dependencies.Cryptography;

/// <summary>
/// 定义继承 <see cref="AbstractKeyingId"/> 的算法密钥环。
/// </summary>
public class AlgorithmKeyring : AbstractKeyingId
{
    /// <summary>
    /// HMAC HASH 密钥环。
    /// </summary>
    [JsonPropertyOrder(1)]
    public HmacHashKeyring HmacHash { get; set; } = new();

    /// <summary>
    /// DES 密钥。
    /// </summary>
    [JsonPropertyOrder(2)]
    public CommonKeyNonce Des { get; set; } = new();

    /// <summary>
    /// AES 密钥。
    /// </summary>
    [JsonPropertyOrder(3)]
    public CommonKeyNonce Aes { get; set; } = new();

    /// <summary>
    /// AES-CCM 密钥。
    /// </summary>
    [JsonPropertyOrder(4)]
    public CommonKeyNonceTag AesCcm { get; set; } = new();

    /// <summary>
    /// AES-GCM 密钥。
    /// </summary>
    [JsonPropertyOrder(5)]
    public CommonKeyNonceTag AesGcm { get; set; } = new();


    /// <summary>
    /// 生成所有密钥。
    /// </summary>
    public virtual void GenerateAll()
    {
        GenerateId();

        HmacHash.GenerateAll();

        Des.Generate(192, 64); // 192, 64

        Aes.Generate(256, 128); // 256, 128

        AesCcm.Generate(256,
            ComputeSizeInBits(SysAesCcm.NonceByteSizes.MaxSize), // 13
            ComputeSizeInBits(SysAesCcm.TagByteSizes.MaxSize)); // 16

        AesGcm.Generate(256,
            ComputeSizeInBits(SysAesGcm.NonceByteSizes.MaxSize), // 12
            ComputeSizeInBits(SysAesGcm.TagByteSizes.MaxSize)); // 16
    }


    /// <summary>
    /// 填充所有密钥。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="md5Key">给定的 MD5 密钥。</param>
    /// <param name="sha1Key">给定的 SHA1 密钥。</param>
    /// <param name="sha256Key">给定的 SHA256 密钥。</param>
    /// <param name="sha384Key">给定的 SHA384 密钥。</param>
    /// <param name="sha512Key">给定的 SHA512 密钥。</param>
    /// <param name="desKey">给定的 DES 密钥。</param>
    /// <param name="desNonce">给定的 DES 初始向量（IV）。</param>
    /// <param name="aesKey">给定的 AES 密钥。</param>
    /// <param name="aesNonce">给定的 AES 初始向量（IV）。</param>
    /// <param name="aesCcmKey">给定的 AES-CCM 密钥。</param>
    /// <param name="aesCcmNonce">给定的 AES-CCM 初始向量（IV）。</param>
    /// <param name="aesCcmTag">给定的 AES-CCM 验证标记。</param>
    /// <param name="aesGcmKey">给定的 AES-GCM 密钥。</param>
    /// <param name="aesGcmNonce">给定的 AES-GCM 初始向量（IV）。</param>
    /// <param name="aesGcmTag">给定的 AES-GCM 验证标记。</param>
    public virtual void PopulateAll(string id,
        byte[] md5Key, byte[] sha1Key, byte[] sha256Key, byte[] sha384Key, byte[] sha512Key,
        byte[] desKey, byte[] desNonce,
        byte[] aesKey, byte[] aesNonce,
        byte[] aesCcmKey, byte[] aesCcmNonce, byte[] aesCcmTag,
        byte[] aesGcmKey, byte[] aesGcmNonce, byte[] aesGcmTag)
    {
        Id = id;

        HmacHash.PopulateAll(md5Key, sha1Key, sha256Key, sha384Key, sha512Key);

        Des.Populate(desKey, desNonce);

        Aes.Populate(aesKey, aesNonce);
        AesCcm.Populate(aesCcmKey, aesCcmNonce, aesCcmTag);
        AesGcm.Populate(aesGcmKey, aesGcmNonce, aesGcmTag);
    }

    /// <summary>
    /// 填充所有密钥。
    /// </summary>
    /// <param name="keyring">给定的 <see cref="AlgorithmKeyring"/>。</param>
    public virtual void PopulateAll(AlgorithmKeyring keyring)
    {
        Id = keyring.Id;

        HmacHash.PopulateAll(keyring.HmacHash);

        Des.Populate(keyring.Des);

        Aes.Populate(keyring.Aes);
        AesCcm.Populate(keyring.AesCcm);
        AesGcm.Populate(keyring.AesGcm);
    }

}

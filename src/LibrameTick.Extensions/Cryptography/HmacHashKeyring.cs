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
/// 定义继承 <see cref="AbstractKey"/> 的 HMAC HASH 密钥环。
/// </summary>
public sealed class HmacHashKeyring : AbstractKey
{
    /// <summary>
    /// MD5 密钥。
    /// </summary>
    [JsonPropertyOrder(1)]
    public CommonKey Md5 { get; set; } = new();

    /// <summary>
    /// SHA1 密钥。
    /// </summary>
    [JsonPropertyOrder(2)]
    public CommonKey Sha1 { get; set; } = new();

    /// <summary>
    /// SHA256 密钥。
    /// </summary>
    [JsonPropertyOrder(3)]
    public CommonKey Sha256 { get; set; } = new();

    /// <summary>
    /// SHA384 密钥。
    /// </summary>
    [JsonPropertyOrder(4)]
    public CommonKey Sha384 { get; set; } = new();

    /// <summary>
    /// SHA512 密钥。
    /// </summary>
    [JsonPropertyOrder(5)]
    public CommonKey Sha512 { get; set; } = new();


    /// <summary>
    /// 生成所有密钥。
    /// </summary>
    public void GenerateAll()
    {
        Md5.Generate(MD5.HashSizeInBits); // 64
        Sha1.Generate(SHA1.HashSizeInBits); // 64
        Sha256.Generate(SHA256.HashSizeInBits); // 64
        Sha384.Generate(SHA384.HashSizeInBits); // 128
        Sha512.Generate(SHA512.HashSizeInBits); // 128
    }


    /// <summary>
    /// 填充所有密钥。
    /// </summary>
    /// <param name="md5Key">给定的 MD5 密钥。</param>
    /// <param name="sha1Key">给定的 SHA1 密钥。</param>
    /// <param name="sha256Key">给定的 SHA256 密钥。</param>
    /// <param name="sha384Key">给定的 SHA384 密钥。</param>
    /// <param name="sha512Key">给定的 SHA512 密钥。</param>
    public void PopulateAll(byte[] md5Key, byte[] sha1Key,
        byte[] sha256Key, byte[] sha384Key, byte[] sha512Key)
    {
        Md5.Populate(md5Key);
        Sha1.Populate(sha1Key);
        Sha256.Populate(sha256Key);
        Sha384.Populate(sha384Key);
        Sha512.Populate(sha512Key);
    }

    /// <summary>
    /// 填充所有密钥。
    /// </summary>
    /// <param name="keyring">给定的 <see cref="HmacHashKeyring"/>。</param>
    public void PopulateAll(HmacHashKeyring keyring)
    {
        Md5.Populate(keyring.Md5);
        Sha1.Populate(keyring.Sha1);
        Sha256.Populate(keyring.Sha256);
        Sha384.Populate(keyring.Sha384);
        Sha512.Populate(keyring.Sha512);
    }

}

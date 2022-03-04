#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Autokeys;

/// <summary>
/// 定义实现 <see cref="Core.IOptions"/> 的 HMACHASH 选项。
/// </summary>
public class HmacHashOptions : Core.IOptions
{
    /// <summary>
    /// MD5 密钥。
    /// </summary>
    public KeyOptions Md5 { get; set; } = new();

    /// <summary>
    /// SHA1 密钥。
    /// </summary>
    public KeyOptions Sha1 { get; set; } = new();

    /// <summary>
    /// SHA256 密钥。
    /// </summary>
    public KeyOptions Sha256 { get; set; } = new();

    /// <summary>
    /// SHA384 密钥。
    /// </summary>
    public KeyOptions Sha384 { get; set; } = new();

    /// <summary>
    /// SHA512 密钥。
    /// </summary>
    public KeyOptions Sha512 { get; set; } = new();


    /// <summary>
    /// 生成 HMACHASH 所有密钥。
    /// </summary>
    public virtual void GenerateAll()
    {
        Md5.Generate(64);
        Sha1.Generate(64);
        Sha256.Generate(64);
        Sha384.Generate(128);
        Sha512.Generate(128);
    }

    /// <summary>
    /// 填充 HMACHASH 所有密钥。
    /// </summary>
    /// <param name="md5Key">给定的 MD5 密钥。</param>
    /// <param name="sha1Key">给定的 SHA1 密钥。</param>
    /// <param name="sha256Key">给定的 SHA256 密钥。</param>
    /// <param name="sha384Key">给定的 SHA384 密钥。</param>
    /// <param name="sha512Key">给定的 SHA512 密钥。</param>
    public virtual void PopulateAll(byte[] md5Key, byte[] sha1Key,
        byte[] sha256Key, byte[] sha384Key, byte[] sha512Key)
    {
        Md5.Populate(md5Key);
        Sha1.Populate(sha1Key);
        Sha256.Populate(sha256Key);
        Sha384.Populate(sha384Key);
        Sha512.Populate(sha512Key);
    }

    /// <summary>
    /// 填充 HMACHASH 所有密钥。
    /// </summary>
    /// <param name="options">给定的 <see cref="HmacHashOptions"/>。</param>
    public virtual void PopulateAll(HmacHashOptions options)
    {
        Md5.Populate(options.Md5);
        Sha1.Populate(options.Sha1);
        Sha256.Populate(options.Sha256);
        Sha384.Populate(options.Sha384);
        Sha512.Populate(options.Sha512);
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
        => $"[{nameof(Md5)}]{Md5};[{nameof(Sha1)}]{Sha1};[{nameof(Sha256)}]{Sha256};[{nameof(Sha384)}]{Sha384};[{nameof(Sha512)}]{Sha512}";

}

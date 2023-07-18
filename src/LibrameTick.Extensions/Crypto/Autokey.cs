#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Crypto;

/// <summary>
/// 定义一个用于序列化的自动密钥。
/// </summary>
public class Autokey : IEquatable<Autokey>
{
    /// <summary>
    /// 标识。
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// 通用密钥基础设施。
    /// </summary>
    public CkiOptions Cki { get; set; } = new();


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="Autokey"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(Autokey? other)
        => other is not null && Id?.Equals(other.Id, StringComparison.Ordinal) == true;

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as Autokey);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => Id?.GetHashCode() ?? 0;


    /// <summary>
    /// 转为标识字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => Id ?? string.Empty;


    /// <summary>
    /// 生成自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    public static Autokey Generate()
    {
        var autokey = new Autokey();

        autokey.Id = Guid.NewGuid().ToString();

        autokey.Cki.GenerateAll();

        return autokey;
    }

    /// <summary>
    /// 固定自动密钥。
    /// </summary>
    /// <returns>返回 <see cref="Autokey"/>。</returns>
    internal static Autokey Fixed()
    {
        var autokey = new Autokey();

        autokey.Id = "5fb3bff5-ffb8-4996-b3b3-86d44c21ba95";

        autokey.Cki.PopulateAll
        (
            // CKI:HMACHASH
            md5Key: Convert.FromBase64String("R0awifT+w1o="),
            sha1Key: Convert.FromBase64String("hlLZ3ejeJQg="),
            sha256Key: Convert.FromBase64String("BGgb8Vk5WTg="),
            sha384Key: Convert.FromBase64String("w5wS0sSQgvmEabMWVo6dQw=="),
            sha512Key: Convert.FromBase64String("pkJ7vOvf9EUYJ1M+8Gjfrw=="),

            // CKI:AES
            aesKey: Convert.FromBase64String("SnfDrzDLPgl8vI1y8AVCgWrz7Qt+Ne2QHFDpwQUZK+U="),
            aesNonce: Convert.FromBase64String("8xqhovyUczST1DeukSdSoA=="),

            // CKI:AES-CCM
            aesCcmKey: Convert.FromBase64String("tHAG5jLtVK9M9eGGwy5tPoFvrEpEXc5bKNnXrOZv7lw="),
            aesCcmNonce: Convert.FromBase64String("xusbVlSX1zl7tfb91Q=="),
            aesCcmTag: Convert.FromBase64String("UEpsvvcOPTPCBBMRMkrxVA=="),

            // CKI:AES-GCM
            aesGcmKey: Convert.FromBase64String("oZqVCzvzeZNe8VNm3Yzk4xO/4tE6GP8EUzc+kP/704Y="),
            aesGcmNonce: Convert.FromBase64String("ve0+0bo91oia9s7f"),
            aesGcmTag: Convert.FromBase64String("fMfxSChx6P5u8wmdPdH67Q==")
        );

        return autokey;
    }

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Serialization;

namespace Librame.Extensions.Core.Cryptography;

/// <summary>
/// 定义实现 <see cref="IOptionsNotifier"/> 的算法选项。
/// </summary>
public class AlgorithmOptions : AbstractOptionsNotifier
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="AlgorithmOptions"/>（此构造函数适用于独立使用 <see cref="AlgorithmOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public AlgorithmOptions(string sourceAliase)
        : base(sourceAliase)
    {
        HmacHash = new(Notifier);
        Aes = KeyNonceOptions.CreateAesOptions(Notifier, nameof(Aes));
        AesCcm = KeyNonceTagOptions.CreateAesCcmOptions(Notifier, nameof(AesCcm));
        AesGcm = KeyNonceTagOptions.CreateAesGcmOptions(Notifier, nameof(AesGcm));
        Rsa = new(Notifier);
    }

    /// <summary>
    /// 使用给定的父级 <see cref="IPropertyNotifier"/> 构造一个 <see cref="AlgorithmOptions"/>（此构造函数适用于集成在 <see cref="AbstractExtensionOptions"/> 中配合使用，以实现扩展选项整体对属性值变化及时作出响应）。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public AlgorithmOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
        HmacHash = new(Notifier);
        Aes = KeyNonceOptions.CreateAesOptions(Notifier, nameof(Aes));
        AesCcm = KeyNonceTagOptions.CreateAesCcmOptions(Notifier, nameof(AesCcm));
        AesGcm = KeyNonceTagOptions.CreateAesGcmOptions(Notifier, nameof(AesGcm));
        Rsa = new(Notifier);
    }


    /// <summary>
    /// HMAC 哈希选项。
    /// </summary>
    public HmacHashOptions HmacHash { get; init; }

    /// <summary>
    /// AES 选项。
    /// </summary>
    public KeyNonceOptions Aes { get; init; }

    /// <summary>
    /// AES-CCM 选项。
    /// </summary>
    public KeyNonceTagOptions AesCcm { get; init; }

    /// <summary>
    /// AES-GCM 选项。
    /// </summary>
    public KeyNonceTagOptions AesGcm { get; init; }

    /// <summary>
    /// RSA 选项。
    /// </summary>
    public SigningCredentialsOptions Rsa { get; init; }


    /// <summary>
    /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
    /// </summary>
    [JsonConverter(typeof(JsonStringEncodingConverter))]
    public Encoding Encoding
    {
        get => Notifier.GetOrAdd(nameof(Encoding), Encoding.UTF8);
        set => Notifier.AddOrUpdate(nameof(Encoding), value);
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
        => $"{nameof(Encoding)}={Encoding.AsEncodingName()};{nameof(HmacHash)}:{HmacHash};{nameof(Aes)}:{Aes};{nameof(AesCcm)}:{AesCcm};{nameof(AesGcm)}:{AesGcm};{nameof(Rsa)}:{Rsa}";

}

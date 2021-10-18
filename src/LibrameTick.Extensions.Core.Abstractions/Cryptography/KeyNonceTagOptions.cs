#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Cryptography;

/// <summary>
/// 定义实现 <see cref="IOptionsNotifier"/> 的密钥、初始向量（IV）、验证标记选项。
/// </summary>
public class KeyNonceTagOptions : KeyNonceOptions
{
    /// <summary>
    /// 构造一个 <see cref="KeyNonceTagOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名。</param>
    public KeyNonceTagOptions(IPropertyNotifier parentNotifier, string sourceAliase)
        : base(parentNotifier, sourceAliase)
    {
    }


    /// <summary>
    /// 验证标记最大大小。
    /// </summary>
    public int TagMaxSize { get; set; }

    /// <summary>
    /// 验证标记。
    /// </summary>
    public byte[] Tag
    {
        get => Notifier.GetOrAdd(nameof(Tag), Array.Empty<byte>());
        set => Notifier.AddOrUpdate(nameof(Tag), value);
    }


    /// <summary>
    /// 设置验证标记方法。
    /// </summary>
    /// <param name="tagFunc">给定的验证标记方法。</param>
    /// <returns>返回验证标记方法。</returns>
    public Func<byte[]> SetTagFunc(Func<byte[]> tagFunc)
    {
        Notifier.AddOrUpdate(nameof(Tag), tagFunc);
        return tagFunc;
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
        => $"{nameof(TagMaxSize)}={TagMaxSize},{nameof(Tag)}={Tag};{base.ToString()}";


    /// <summary>
    /// 创建 AES-CCM 选项。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名。</param>
    /// <returns>返回 <see cref="KeyNonceTagOptions"/>。</returns>
    public static KeyNonceTagOptions CreateAesCcmOptions(IPropertyNotifier parentNotifier, string sourceAliase)
    {
        var options = new KeyNonceTagOptions(parentNotifier, sourceAliase);

        // 以字节为单位，参数长度可以是 16、24 或 32 字节（128、192 或 256 位）
        options.KeyMaxSize = 32;
        options.NonceMaxSize = AesCcm.NonceByteSizes.MaxSize;
        options.TagMaxSize = AesCcm.TagByteSizes.MaxSize;

        options.Key = RandomExtensions.GenerateByteArray(options.KeyMaxSize);
        options.Nonce = RandomExtensions.GenerateByteArray(options.NonceMaxSize);
        options.Tag = RandomExtensions.GenerateByteArray(options.TagMaxSize);

        return options;
    }

    /// <summary>
    /// 创建 AES-GCM 选项。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名。</param>
    /// <returns>返回 <see cref="KeyNonceTagOptions"/>。</returns>
    public static KeyNonceTagOptions CreateAesGcmOptions(IPropertyNotifier parentNotifier, string sourceAliase)
    {
        var options = new KeyNonceTagOptions(parentNotifier, sourceAliase);

        // 以字节为单位，参数长度可以是 16、24 或 32 字节（128、192 或 256 位）
        options.KeyMaxSize = 32;
        options.NonceMaxSize = AesGcm.NonceByteSizes.MaxSize;
        options.TagMaxSize = AesGcm.TagByteSizes.MaxSize;

        options.Key = RandomExtensions.GenerateByteArray(options.KeyMaxSize);
        options.Nonce = RandomExtensions.GenerateByteArray(options.NonceMaxSize);
        options.Tag = RandomExtensions.GenerateByteArray(options.TagMaxSize);

        return options;
    }

}

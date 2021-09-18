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
/// 定义实现 <see cref="IOptions"/> 的密钥选项。
/// </summary>
public class KeyOptions : AbstractOptions
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="KeyOptions"/>（此构造函数适用于独立使用 <see cref="KeyOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public KeyOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="KeyOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名。</param>
    public KeyOptions(IPropertyNotifier parentNotifier, string sourceAliase)
        : base(parentNotifier, sourceAliase)
    {
    }


    /// <summary>
    /// 密钥最大大小。
    /// </summary>
    public int KeyMaxSize { get; set; }

    /// <summary>
    /// 密钥。
    /// </summary>
    public byte[] Key
    {
        get => Notifier.GetOrAdd(nameof(Key), Array.Empty<byte>());
        set => Notifier.AddOrUpdate(nameof(Key), value);
    }


    /// <summary>
    /// 设置密钥方法。
    /// </summary>
    /// <param name="keyFunc">给定的密钥方法。</param>
    /// <returns>返回密钥方法。</returns>
    public Func<byte[]> SetKeyFunc(Func<byte[]> keyFunc)
    {
        Notifier.AddOrUpdate(nameof(Key), keyFunc);
        return keyFunc;
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
        => $"{nameof(KeyMaxSize)}={KeyMaxSize},{nameof(Key)}={Key}";

}

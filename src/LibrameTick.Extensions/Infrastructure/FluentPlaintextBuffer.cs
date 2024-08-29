#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义实现 <see cref="Fluent{TSelf, TChain}"/> 的流畅明文字节数组缓冲区。
/// </summary>
/// <param name="initialBytes">给定的初始字节数组。</param>
/// <param name="encoding">给定的字符编码。</param>
public class FluentPlaintextBuffer(byte[] initialBytes, Encoding encoding)
    : Fluent<FluentPlaintextBuffer, byte[]>(initialBytes), IEquatable<FluentPlaintextBuffer>
{
    /// <summary>
    /// 构造一个 <see cref="FluentPlaintextBuffer"/>。
    /// </summary>
    /// <param name="initialString">给定的初始字符串。</param>
    /// <param name="encoding">给定的字符编码。</param>
    public FluentPlaintextBuffer(string initialString, Encoding encoding)
        : this(encoding.GetBytes(initialString), encoding)
    {
    }


    /// <summary>
    /// 获取字符编码。
    /// </summary>
    /// <value>
    /// 返回 <see cref="System.Text.Encoding"/>。
    /// </value>
    public Encoding Encoding { get; init; } = encoding;


    /// <summary>
    /// 使用固定时间比较字符串相等，以防范诸如破解密钥的计时攻击。
    /// </summary>
    /// <param name="otherBytes">给定要比较的其他 <see cref="FluentPlaintextBuffer"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool FixedTimeEquals(FluentPlaintextBuffer otherBytes)
        => FixedTimeEquals(otherBytes.CurrentValue);

    /// <summary>
    /// 使用固定时间比较字符串相等，以防范诸如破解密钥的计时攻击。
    /// </summary>
    /// <param name="otherBytes">给定要比较的其他字节数组。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool FixedTimeEquals(byte[] otherBytes)
        => CryptographicOperations.FixedTimeEquals(CurrentValue, otherBytes);


    /// <summary>
    /// 切换当前字节数组。
    /// </summary>
    /// <param name="newBytesFunc">给定新字节数组的方法。</param>
    /// <returns>返回 <see cref="FluentPlaintextBuffer"/>。</returns>
    public virtual FluentPlaintextBuffer Switch(Func<byte[], byte[]> newBytesFunc)
        => base.Switch(fluent => newBytesFunc(fluent.CurrentValue));

    /// <summary>
    /// 复制一个当前流畅字节数组的副本。
    /// </summary>
    /// <returns>返回 <see cref="FluentPlaintextBuffer"/>。</returns>
    public override FluentPlaintextBuffer Copy()
        => new(CurrentValue, Encoding);


    /// <summary>
    /// 将当前 <see cref="FluentPlaintextBuffer"/> 转换为 Base64 字符串形式。
    /// </summary>
    /// <returns>返回 Base64 字符串。</returns>
    public virtual string ToBase64String()
        => Convert.ToBase64String(CurrentValue);


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => CurrentValue.GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回当前路径值字符串。</returns>
    public override string ToString()
        => CurrentValue.ToString() ?? string.Empty;


    /// <summary>
    /// 相等的流畅字节数组。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as FluentPlaintextBuffer);

    /// <summary>
    /// 相等的流畅字节数组。
    /// </summary>
    /// <param name="other">给定要比较的 <see cref="FluentPlaintextBuffer"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(FluentPlaintextBuffer? other)
        => other is not null && CurrentValue.SequenceEqualByReadOnlySpan(other.CurrentValue);


    /// <summary>
    /// 将当前 <see cref="FluentPlaintextBuffer"/> 隐式转换为字节数组形式。
    /// </summary>
    /// <param name="buffer">给定的 <see cref="FluentPlaintextBuffer"/>。</param>
    public static implicit operator byte[](FluentPlaintextBuffer buffer)
        => buffer.CurrentValue;

}

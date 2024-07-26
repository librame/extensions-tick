#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Serialization;

/// <summary>
/// 定义二进制序列化成员数组的自定义特性。
/// </summary>
/// <param name="count">给定要读写的字节数。</param>
/// <param name="index">给定要读写的第一个字节的索引（可选；默认从开始读写）。</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
    Inherited = true, AllowMultiple = false)]
public class BinaryArrayAttribute(int count, int index = 0) : Attribute
{
    /// <summary>
    /// 获取或设置要读写的第一个字节的索引。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int Index { get; set; } = index;

    /// <summary>
    /// 获取或设置要读写的字节数。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int Count { get; set; } = count;
}

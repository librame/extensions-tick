#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies.Cryptography;

/// <summary>
/// 定义抽象密钥。
/// </summary>
public abstract class AbstractKey
{

    /// <summary>
    /// 获取字节数组的位大小。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <param name="bitSize">给定的位大小（可选；默认根据字节数组计算）。</param>
    /// <returns>返回 32 位整数。</returns>
    public virtual int GetByteArrayBitSize(byte[] bytes, int? bitSize = null)
        => bitSize ?? ComputeSizeInBits(bytes.Length);

    /// <summary>
    /// 生成随机数字节数组。
    /// </summary>
    /// <param name="bitSize">给定的位大小。</param>
    /// <returns>返回字节数组。</returns>
    public virtual byte[] GenerateRandomNumberByteArray(int bitSize)
        => RandomNumberGenerator.GetBytes(ComputeSizeInBytes(bitSize));


    /// <summary>
    /// 将字节数组转为字符串。
    /// </summary>
    /// <param name="bytes">给定的字节数组。</param>
    /// <returns>返回字符串。</returns>
    public virtual string ToByteArrayString(byte[] bytes)
        => Convert.ToBase64String(bytes);


    /// <summary>
    /// 计算字节数组的位大小。
    /// </summary>
    /// <param name="byteSize">给定的字节数组大小。</param>
    /// <returns>返回 32 位整数。</returns>
    public static int ComputeSizeInBits(int byteSize)
        => byteSize * 8;

    /// <summary>
    /// 计算位大小的字节数组大小。
    /// </summary>
    /// <param name="bitSize">给定的位大小。</param>
    /// <returns>返回 32 位整数。</returns>
    public static int ComputeSizeInBytes(int bitSize)
        => bitSize / 8;

}

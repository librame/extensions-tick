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
/// 抽象实现 <see cref="IAlgorithmParameterGenerator"/>。
/// </summary>
public abstract class AbstractAlgorithmParameterGenerator : IAlgorithmParameterGenerator
{
    /// <summary>
    /// 生成指定数组长度的密钥。
    /// </summary>
    /// <param name="length">给定的数组长度。</param>
    /// <returns>返回字节数组。</returns>
    public virtual byte[] GenerateKey(int length)
        => RandomExtensions.GenerateByteArray(length);

    /// <summary>
    /// 生成指定数组长度的初始向量。
    /// </summary>
    /// <param name="length">给定的数组长度。</param>
    /// <param name="key">给定可参与运算的密钥（可选）。</param>
    /// <returns>返回字节数组。</returns>
    public virtual byte[] GenerateNonce(int length, byte[]? key = null)
        => RandomExtensions.GenerateByteArray(length);

    /// <summary>
    /// 生成指定数组长度的验证标记。
    /// </summary>
    /// <param name="length">给定的数组长度。</param>
    /// <param name="key">给定可参与运算的密钥（可选）。</param>
    /// <returns>返回字节数组。</returns>
    public virtual byte[] GenerateTag(int length, byte[]? key = null)
        => RandomExtensions.GenerateByteArray(length);

}

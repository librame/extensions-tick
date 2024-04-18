#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies;

/// <summary>
/// 定义算法参数生成器接口。
/// </summary>
public interface IAlgorithmParameterGenerator
{
    /// <summary>
    /// 生成指定数组长度的密钥。
    /// </summary>
    /// <param name="length">给定的数组长度。</param>
    /// <returns>返回字节数组。</returns>
    byte[] GenerateKey(int length);

    /// <summary>
    /// 生成指定数组长度的初始向量。
    /// </summary>
    /// <param name="length">给定的数组长度。</param>
    /// <param name="key">给定可参与运算的密钥（可选）。</param>
    /// <returns>返回字节数组。</returns>
    byte[] GenerateNonce(int length, byte[]? key = null);

    /// <summary>
    /// 生成指定数组长度的验证标记。
    /// </summary>
    /// <param name="length">给定的数组长度。</param>
    /// <param name="key">给定可参与运算的密钥（可选）。</param>
    /// <returns>返回字节数组。</returns>
    byte[] GenerateTag(int length, byte[]? key = null);
}

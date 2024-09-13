#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Cryptography;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义对称算法接口。
/// </summary>
public interface ISymmetricAlgorithm : IAlgorithm
{

    #region AES

    /// <summary>
    /// 加密 AES。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="CommonKeyNonce"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回字节数组。</returns>
    byte[] EncryptAes(byte[] buffer, CommonKeyNonce? options = null);

    /// <summary>
    /// 解密 AES。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="CommonKeyNonce"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回字节数组。</returns>
    byte[] DecryptAes(byte[] buffer, CommonKeyNonce? options = null);

    #endregion


    #region AES-CCM

    /// <summary>
    /// 加密 AES-CCM。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="CommonKeyNonceTag"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    byte[] EncryptAesCcm(byte[] buffer, CommonKeyNonceTag? options = null);

    /// <summary>
    /// 解密 AES-CCM。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="CommonKeyNonceTag"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    byte[] DecryptAesCcm(byte[] buffer, CommonKeyNonceTag? options = null);

    #endregion


    #region AES-GCM

    /// <summary>
    /// 加密 AES-GCM。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="CommonKeyNonceTag"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    byte[] EncryptAesGcm(byte[] buffer, CommonKeyNonceTag? options = null);

    /// <summary>
    /// 解密 AES-GCM。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="CommonKeyNonceTag"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    byte[] DecryptAesGcm(byte[] buffer, CommonKeyNonceTag? options = null);

    #endregion

}

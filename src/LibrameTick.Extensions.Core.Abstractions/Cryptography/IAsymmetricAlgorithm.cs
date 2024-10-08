#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义一个继承 <see cref="IAlgorithm"/> 的非对称算法接口。
/// </summary>
public interface IAsymmetricAlgorithm : IAlgorithm
{

    #region RSA

    /// <summary>
    /// RSA 加密填充。
    /// </summary>
    /// <value>
    /// 返回 <see cref="RSAEncryptionPadding"/>。
    /// </value>
    RSAEncryptionPadding RsaPadding { get; set; }


    /// <summary>
    /// 加密 RSA。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="AlgorithmOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{AlgorithmFactoryOptions}.Default"/>）。</param>
    /// <returns>返回字节数组。</returns>
    byte[] EncryptRsa(byte[] buffer, AlgorithmOptions? options = null);

    /// <summary>
    /// 解密 RSA。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="AlgorithmOptions"/>（可选；默认使用 <see cref="StaticDefaultInitializer{AlgorithmFactoryOptions}.Default"/>）。</param>
    /// <returns>返回字节数组。</returns>
    byte[] DecryptRsa(byte[] buffer, AlgorithmOptions? options = null);

    #endregion

}

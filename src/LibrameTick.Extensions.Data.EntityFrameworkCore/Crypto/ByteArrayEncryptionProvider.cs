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
/// 定义实现 <see cref="IEncryptionProvider{ByteArray}"/> 用于字节数组的加密提供程序。
/// </summary>
public class ByteArrayEncryptionProvider : IEncryptionProvider<byte[]>
{
    private readonly ISymmetricAlgorithm _symmetric;
    private readonly AlgorithmOptions _algorithms;


    /// <summary>
    /// 构造一个 <see cref="ByteArrayEncryptionProvider"/>。
    /// </summary>
    /// <param name="symmetric">给定的 <see cref="ISymmetricAlgorithm"/>。</param>
    /// <param name="algorithms">给定的 <see cref="AlgorithmOptions"/>。</param>
    public ByteArrayEncryptionProvider(ISymmetricAlgorithm symmetric,
        AlgorithmOptions algorithms)
    {
        _symmetric = symmetric;
        _algorithms = algorithms;
    }


    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Algorithm
        => _algorithms;


    /// <summary>
    /// 解密字节数组。
    /// </summary>
    /// <param name="encryptValue">给定的解密字节数组。</param>
    /// <returns>返回加密后的原始字节数组。</returns>
    public byte[] Decrypt(byte[] encryptValue)
        => _symmetric.DecryptAes(encryptValue, _algorithms.Aes);

    /// <summary>
    /// 加密字节数组。
    /// </summary>
    /// <param name="orginalValue">给定的原始字节数组。</param>
    /// <returns>返回加密后的字节数组。</returns>
    public byte[] Encrypt(byte[] orginalValue)
        => _symmetric.EncryptAes(orginalValue, _algorithms.Aes);

}

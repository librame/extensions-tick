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
/// 抽象实现 <see cref="ISymmetricAlgorithm"/>。
/// </summary>
public abstract class AbstractSymmetricAlgorithm : AbstractAlgorithm, ISymmetricAlgorithm
{
    /// <summary>
    /// 构造一个 <see cref="AbstractSymmetricAlgorithm"/>。
    /// </summary>
    /// <param name="parameterGenerator">给定的 <see cref="IAlgorithmParameterGenerator"/>。</param>
    /// <param name="options">给定的 <see cref="IExtensionOptions"/>。</param>
    public AbstractSymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
        IExtensionOptions options)
        : base(parameterGenerator, options)
    {
    }


    /// <summary>
    /// 默认 AES 选项。
    /// </summary>
    /// <returns>返回 <see cref="KeyNonceOptions"/>。</returns>
    protected abstract KeyNonceOptions DefaultAesOptions { get; }

    /// <summary>
    /// 默认 AES-CCM 选项。
    /// </summary>
    /// <returns>返回 <see cref="KeyNonceTagOptions"/>。</returns>
    protected abstract KeyNonceTagOptions DefaultAesCcmOptions { get; }

    /// <summary>
    /// 默认 AES-GCM 选项。
    /// </summary>
    /// <returns>返回 <see cref="KeyNonceTagOptions"/>。</returns>
    protected abstract KeyNonceTagOptions DefaultAesGcmOptions { get; }


    #region AES

    /// <summary>
    /// 加密 AES。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="KeyNonceOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public virtual byte[] EncryptAes(byte[] buffer, KeyNonceOptions? options = null)
    {
        if (options is null)
            options = DefaultAesOptions;
            
        var aes = Aes.Create();
        aes.Key = options.Key;
        aes.IV = options.Nonce;

        var transform = aes.CreateEncryptor();
        return transform.TransformFinalBlock(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// 解密 AES。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="KeyNonceOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public virtual byte[] DecryptAes(byte[] buffer, KeyNonceOptions? options = null)
    {
        if (options is null)
            options = DefaultAesOptions;

        var aes = Aes.Create();
        aes.Key = options.Key;
        aes.IV = options.Nonce;

        var transform = aes.CreateDecryptor();
        return transform.TransformFinalBlock(buffer, 0, buffer.Length);
    }

    #endregion


    #region AES-CCM

    /// <summary>
    /// 加密 AES-CCM。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="KeyNonceTagOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public virtual byte[] EncryptAesCcm(byte[] buffer, KeyNonceTagOptions? options = null)
    {
        if (options is null)
            options = DefaultAesCcmOptions;
            
        var ciphertext = new byte[buffer.Length];

        var aes = new AesCcm(options.Key);
        aes.Encrypt(options.Nonce, buffer, ciphertext, options.Tag);

        return ciphertext;
    }

    /// <summary>
    /// 解密 AES-CCM。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="KeyNonceTagOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public virtual byte[] DecryptAesCcm(byte[] buffer, KeyNonceTagOptions? options = null)
    {
        if (options is null)
            options = DefaultAesCcmOptions;

        var plaintext = new byte[buffer.Length];

        var aes = new AesCcm(options.Key);
        aes.Decrypt(options.Nonce, buffer, options.Tag, plaintext);

        return plaintext;
    }

    #endregion


    #region AES-GCM

    /// <summary>
    /// 加密 AES-GCM。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="KeyNonceTagOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过加密的字节数组。</returns>
    public virtual byte[] EncryptAesGcm(byte[] buffer, KeyNonceTagOptions? options = null)
    {
        if (options is null)
            options = DefaultAesGcmOptions;

        var ciphertext = new byte[buffer.Length];

        var aes = new AesGcm(options.Key);
        aes.Encrypt(options.Nonce, buffer, ciphertext, options.Tag);

        return ciphertext;
    }

    /// <summary>
    /// 解密 AES-GCM。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="KeyNonceTagOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回经过解密的字节数组。</returns>
    public virtual byte[] DecryptAesGcm(byte[] buffer, KeyNonceTagOptions? options = null)
    {
        if (options is null)
            options = DefaultAesGcmOptions;

        var plaintext = new byte[buffer.Length];

        var aes = new AesGcm(options.Key);
        aes.Decrypt(options.Nonce, buffer, options.Tag, plaintext);

        return plaintext;
    }

    #endregion

}

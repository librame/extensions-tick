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
/// 定义算法引用选项。
/// </summary>
public class AlgorithmReferenceOptions : StaticDefaultInitializer<AlgorithmReferenceOptions>, IReferenceEnablement
{
    /// <summary>
    /// 构造一个 <see cref="AlgorithmReferenceOptions"/> 默认实例。
    /// </summary>
    public AlgorithmReferenceOptions()
    {
        AsymmetricStreamDecryptor = (encrypted, decrypted) => encrypted.FromPrivateRsa(decrypted);
        AsymmetricStreamEncryptor = (plaintext, encrypted) => plaintext.AsPublicRsa(encrypted);

        SymmetricStreamDecryptor = (encrypted, decrypted) => encrypted.FromAes(decrypted);
        SymmetricStreamEncryptor = (plaintext, encrypted) => plaintext.AsAes(encrypted);

        DefaultStreamDecryptor = AsymmetricStreamDecryptor;
        DefaultStreamEncryptor = AsymmetricStreamEncryptor;
    }

    /// <summary>
    /// 使用指定的 <see cref="AlgorithmReferenceOptions"/> 构造一个 <see cref="AlgorithmReferenceOptions"/> 实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="AlgorithmReferenceOptions"/>。</param>
    public AlgorithmReferenceOptions(AlgorithmReferenceOptions options)
    {
        AsymmetricStreamDecryptor = options.AsymmetricStreamDecryptor;
        AsymmetricStreamEncryptor = options.AsymmetricStreamEncryptor;

        SymmetricStreamDecryptor = options.SymmetricStreamDecryptor;
        SymmetricStreamEncryptor = options.SymmetricStreamEncryptor;

        DefaultStreamDecryptor = options.DefaultStreamDecryptor;
        DefaultStreamEncryptor = options.DefaultStreamEncryptor;
    }


    /// <summary>
    /// 获取或设置当前是否已启用此功能引用。
    /// </summary>
    /// <remarks>
    /// 主要用于控制外部引用此压缩功能是否已启用。默认不启用。
    /// </remarks>
    public bool IsReferenceEnabled { get; set; }

    /// <summary>
    /// 获取或设置非对称流解密器（传入参数依次为：加密流、解密流）。
    /// </summary>
    /// <remarks>
    /// 默认使用 RSA 解密，详情参见 <see cref="AlgorithmExtensions.FromPrivateRsa(Stream, Stream, RSAEncryptionPadding?, Func{RSA}?)"/>。
    /// </remarks>
    /// <value>
    /// 返回委托动作。
    /// </value>
    public Action<Stream, Stream> AsymmetricStreamDecryptor { get; set; }

    /// <summary>
    /// 获取或设置非对称流加密器（传入参数依次为：明文流、密文流）。
    /// </summary>
    /// <remarks>
    /// 默认使用 RSA 加密，详情参见 <see cref="AlgorithmExtensions.AsPublicRsa(Stream, Stream, RSAEncryptionPadding?, Func{RSA}?)"/>。
    /// </remarks>
    /// <value>
    /// 返回委托动作。
    /// </value>
    public Action<Stream, Stream> AsymmetricStreamEncryptor { get; set; }


    /// <summary>
    /// 获取或设置对称流解密器（传入参数依次为：加密流、解密流）。
    /// </summary>
    /// <remarks>
    /// 默认使用 AES 解密。
    /// </remarks>
    /// <value>
    /// 返回委托动作。
    /// </value>
    public Action<Stream, Stream> SymmetricStreamDecryptor { get; set; }

    /// <summary>
    /// 获取或设置对称流加密器（传入参数依次为：明文流、密文流）。
    /// </summary>
    /// <remarks>
    /// 默认使用 AES 加密。
    /// </remarks>
    /// <value>
    /// 返回委托动作。
    /// </value>
    public Action<Stream, Stream> SymmetricStreamEncryptor { get; set; }


    /// <summary>
    /// 获取或设置默认流解密器（传入参数依次为：加密流、解密流）。
    /// </summary>
    /// <remarks>
    /// 通常用于快速切换当前选项的解密器，默认使用 <see cref="AsymmetricStreamDecryptor"/>。
    /// </remarks>
    /// <value>
    /// 返回委托动作。
    /// </value>
    public Action<Stream, Stream> DefaultStreamDecryptor { get; set; }

    /// <summary>
    /// 获取或设置默认流加密器（传入参数依次为：明文流、密文流）。
    /// </summary>
    /// <remarks>
    /// 通常用于快速切换当前选项的加密器，默认使用 <see cref="AsymmetricStreamEncryptor"/>。
    /// </remarks>
    /// <value>
    /// 返回委托动作。
    /// </value>
    public Action<Stream, Stream> DefaultStreamEncryptor { get; set; }

}

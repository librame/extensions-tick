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
/// 定义抽象实现 <see cref="IAsymmetricAlgorithm"/> 的非对称算法。
/// </summary>
public abstract class AbstractAsymmetricAlgorithm : AbstractAlgorithm, IAsymmetricAlgorithm
{
    /// <summary>
    /// 构造一个 <see cref="AbstractAsymmetricAlgorithm"/>。
    /// </summary>
    /// <param name="parameterGenerator">给定的 <see cref="IAlgorithmParameterGenerator"/>。</param>
    /// <param name="options">给定的 <see cref="AlgorithmOptions"/>。</param>
    protected AbstractAsymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
        AlgorithmOptions options)
        : base(parameterGenerator, options)
    {
    }


    #region RSA

    /// <summary>
    /// RSA 加密填充。
    /// </summary>
    public RSAEncryptionPadding RsaPadding { get; set; }
        = RSAEncryptionPadding.Pkcs1;


    /// <summary>
    /// 加密 RSA。
    /// </summary>
    /// <param name="buffer">给定待加密的字节数组。</param>
    /// <param name="options">给定的 <see cref="SigningCredentialsOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回字节数组。</returns>
    public virtual byte[] EncryptRsa(byte[] buffer, SigningCredentialsOptions? options = null)
    {
        if (options is null)
            options = Options.Rsa;

        if (options.Provider is not IRsaSigningCredentialsProvider rsaProvider)
            throw new NotSupportedException($"{nameof(options)}.{nameof(options.Provider)} unimplemented {nameof(IRsaSigningCredentialsProvider)}.");

        var rsa = rsaProvider.LoadRsa();
        return rsa.Encrypt(buffer, RsaPadding);
    }

    /// <summary>
    /// 解密 RSA。
    /// </summary>
    /// <param name="buffer">给定的字节数组。</param>
    /// <param name="options">给定的 <see cref="SigningCredentialsOptions"/>（可选；默认使用选项配置）。</param>
    /// <returns>返回字节数组。</returns>
    public virtual byte[] DecryptRsa(byte[] buffer, SigningCredentialsOptions? options = null)
    {
        if (options is null)
            options = Options.Rsa;

        if (!(options.Provider is IRsaSigningCredentialsProvider rsaProvider))
            throw new NotSupportedException($"{nameof(options)}.{nameof(options.Provider)} unimplemented {nameof(IRsaSigningCredentialsProvider)}.");

        var rsa = rsaProvider.LoadRsa();
        return rsa.Decrypt(buffer, RsaPadding);
    }

    #endregion

}

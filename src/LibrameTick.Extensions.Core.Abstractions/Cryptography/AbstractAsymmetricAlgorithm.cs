#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Cryptography;

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// 抽象实现 <see cref="IAsymmetricAlgorithm"/>。
    /// </summary>
    public abstract class AbstractAsymmetricAlgorithm : AbstractAlgorithm, IAsymmetricAlgorithm
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractAsymmetricAlgorithm"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parameterGenerator"/> or <paramref name="extensionBuilder"/> 为空。
        /// </exception>
        /// <param name="parameterGenerator">给定的 <see cref="IAlgorithmParameterGenerator"/>。</param>
        /// <param name="extensionBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        public AbstractAsymmetricAlgorithm(IAlgorithmParameterGenerator parameterGenerator,
            IExtensionBuilder extensionBuilder)
            : base(parameterGenerator, extensionBuilder)
        {
        }


        /// <summary>
        /// 默认 RSA 选项。
        /// </summary>
        protected abstract SigningCredentialsOptions DefaultRsaOptions { get; }


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
            if (options == null)
                options = DefaultRsaOptions;

            var rsa = options.Credentials.AsRsa();
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
            if (options == null)
                options = DefaultRsaOptions;

            var rsa = options.Credentials.AsRsa();
            return rsa.Decrypt(buffer, RsaPadding);
        }

        #endregion

    }
}

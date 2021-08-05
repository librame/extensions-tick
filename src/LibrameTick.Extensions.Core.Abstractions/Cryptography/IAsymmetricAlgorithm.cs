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
    /// 定义非对称算法接口。
    /// </summary>
    public interface IAsymmetricAlgorithm : IAlgorithm
    {

        #region RSA

        /// <summary>
        /// RSA 加密填充。
        /// </summary>
        RSAEncryptionPadding RsaPadding { get; set; }


        /// <summary>
        /// 加密 RSA。
        /// </summary>
        /// <param name="buffer">给定待加密的字节数组。</param>
        /// <param name="options">给定的 <see cref="SigningCredentialsOptions"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] EncryptRsa(byte[] buffer, SigningCredentialsOptions? options = null);

        /// <summary>
        /// 解密 RSA。
        /// </summary>
        /// <param name="buffer">给定的字节数组。</param>
        /// <param name="options">给定的 <see cref="SigningCredentialsOptions"/>（可选；默认使用选项配置）。</param>
        /// <returns>返回字节数组。</returns>
        byte[] DecryptRsa(byte[] buffer, SigningCredentialsOptions? options = null);

        #endregion

    }
}

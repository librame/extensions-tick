#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Data.Cryptography
{
    /// <summary>
    /// 定义实现 <see cref="IEncryptionProvider{String}"/> 用于字符串的加密提供程序。
    /// </summary>
    public class StringEncryptionProvider : IEncryptionProvider<string>
    {
        private readonly ByteArrayEncryptionProvider _provider;
        private readonly Encoding _encoding;


        /// <summary>
        /// 构造一个 <see cref="StringEncryptionProvider"/>。
        /// </summary>
        /// <param name="provider">给定的 <see cref="ByteArrayEncryptionProvider"/>。</param>
        public StringEncryptionProvider(ByteArrayEncryptionProvider provider)
        {
            _provider = provider;
            _encoding = provider.Algorithm.Encoding;
        }


        /// <summary>
        /// 解密字符串。
        /// </summary>
        /// <param name="encryptValue">给定的解密字符串。</param>
        /// <returns>返回加密后的原始字符串。</returns>
        public string Decrypt(string encryptValue)
        {
            var buffer = encryptValue.FromBase64String();

            buffer = _provider.Decrypt(buffer);

            return buffer.AsEncodingString(_encoding);
        }

        /// <summary>
        /// 加密字符串。
        /// </summary>
        /// <param name="orginalValue">给定的原始字符串。</param>
        /// <returns>返回加密后的字符串。</returns>
        public string Encrypt(string orginalValue)
        {
            var buffer = orginalValue.FromEncodingString(_encoding);

            buffer = _provider.Encrypt(buffer);

            return buffer.AsBase64String();
        }

    }
}

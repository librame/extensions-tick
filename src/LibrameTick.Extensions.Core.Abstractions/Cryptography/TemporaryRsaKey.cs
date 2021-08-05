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
    /// 定义一个用于文件型的临时 RSA 密钥。
    /// </summary>
    public class TemporaryRsaKey
    {
        /// <summary>
        /// 密钥标识。
        /// </summary>
        public string? KeyId { get; set; }

        /// <summary>
        /// RSA 参数。
        /// </summary>
        public RSAParameters Parameters { get; set; }


        /// <summary>
        /// 生成密钥标识。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public static string GenerateKeyId()
            => RandomExtensions.GenerateByteArray(16).AsBase64String();

        /// <summary>
        /// 从文件中加载 <see cref="TemporaryRsaKey"/>。
        /// </summary>
        /// <param name="keyFile">给定的密钥文件。</param>
        /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
        public static TemporaryRsaKey LoadFile(string keyFile)
        {
            var key = keyFile.ReadJson<TemporaryRsaKey>();
            if (key == null)
            {
                key = new TemporaryRsaKey
                {
                    KeyId = GenerateKeyId(),
                    Parameters = RSA.Create().ExportParameters(true)
                };
                keyFile.WriteJson(key);
            }

            return key;
        }

    }
}

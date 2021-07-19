#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IdentityModel.Tokens;
using System;

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// <see cref="TemporaryRsaKey"/> 静态扩展。
    /// </summary>
    public static class TemporaryRsaKeyExtensions
    {
        /// <summary>
        /// 转换为 <see cref="RsaSecurityKey"/>。
        /// </summary>
        /// <param name="tempKey">给定的 <see cref="TemporaryRsaKey"/>。</param>
        /// <param name="requirePrivateKey">是否需要私钥（可选；默认需要）。</param>
        /// <returns>返回 <see cref="RsaSecurityKey"/>。</returns>
        public static RsaSecurityKey AsRsaKey(this TemporaryRsaKey? tempKey, bool requirePrivateKey = true)
        {
            tempKey.NotNull(nameof(tempKey));

            var rsaKey = new RsaSecurityKey(tempKey.Parameters)
            {
                KeyId = tempKey.KeyId
            };

            if (requirePrivateKey && rsaKey.PrivateKeyStatus == PrivateKeyStatus.DoesNotExist)
                throw new NotSupportedException($"The temporary rsa key does not have a private key.");

            return rsaKey;
        }

    }
}

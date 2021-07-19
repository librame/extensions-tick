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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Librame.Extensions.Core.Cryptography
{
    /// <summary>
    /// <see cref="SigningCredentials"/> 静态扩展。
    /// </summary>
    public static class SigningCredentialsExtensions
    {

        /// <summary>
        /// 验证签名证书。
        /// </summary>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <returns>返回 <see cref="SigningCredentials"/>。</returns>
        public static SigningCredentials Verify(this SigningCredentials credentials)
        {
            if (!(credentials.Key is AsymmetricSecurityKey
                || credentials.Key is JsonWebKey && ((JsonWebKey)credentials.Key).HasPrivateKey))
            {
                throw new InvalidOperationException("Invalid signing key.");
            }

            return credentials;
        }


        /// <summary>
        /// 当作 RSA。
        /// </summary>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <returns>返回 <see cref="RSA"/>。</returns>
        public static RSA AsRsa(this SigningCredentials credentials)
        {
            credentials.NotNull(nameof(credentials));

            if (credentials.Key is X509SecurityKey x509Key)
                return (RSA)x509Key.PrivateKey;

            if (credentials.Key is RsaSecurityKey rsaKey)
            {
                if (rsaKey.Rsa == null)
                {
                    var rsa = RSA.Create();
                    rsa.ImportParameters(rsaKey.Parameters);

                    return rsa;
                }

                return rsaKey.Rsa;
            }

            throw new NotSupportedException($"Not supported signing credentials.");
        }

        /// <summary>
        /// 当作证书。
        /// </summary>
        /// <param name="credentials">给定的 <see cref="SigningCredentials"/>。</param>
        /// <returns>返回 <see cref="X509Certificate2"/>。</returns>
        public static X509Certificate2 AsCertificate(this SigningCredentials credentials)
        {
            credentials.NotNull(nameof(credentials));

            if (credentials.Key is X509SecurityKey x509Key)
                return x509Key.Certificate;

            throw new NotSupportedException($"Not supported signing credentials.");
        }

    }
}

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
    /// <see cref="RSAParametersInfo"/> 静态扩展。
    /// </summary>
    public static class RSAParametersInfoExtensions
    {

        /// <summary>
        /// 转为 RSA 参数集合。
        /// </summary>
        /// <param name="parametersInfo">给定的 <see cref="RSAParametersInfo"/>。</param>
        /// <returns>返回 <see cref="RSAParameters"/>。</returns>
        public static RSAParameters AsParameters(this RSAParametersInfo parametersInfo)
        {
            return new RSAParameters
            {
                D = parametersInfo.D?.FromBase64String(),
                DP = parametersInfo.DP?.FromBase64String(),
                DQ = parametersInfo.DQ?.FromBase64String(),
                Exponent = parametersInfo.Exponent?.FromBase64String(),
                InverseQ = parametersInfo.InverseQ?.FromBase64String(),
                Modulus = parametersInfo.Modulus?.FromBase64String(),
                P = parametersInfo.P?.FromBase64String(),
                Q = parametersInfo.Q?.FromBase64String()
            };
        }

        /// <summary>
        /// 从 RSA 参数集合还原。
        /// </summary>
        /// <param name="parameters">给定的 <see cref="RSAParameters"/>。</param>
        /// <returns>返回 <see cref="RSAParametersInfo"/>。</returns>
        public static RSAParametersInfo FromParameters(this RSAParameters parameters)
        {
            return new RSAParametersInfo
            {
                D = parameters.D?.AsBase64String(),
                DP = parameters.DP?.AsBase64String(),
                DQ = parameters.DQ?.AsBase64String(),
                Exponent = parameters.Exponent?.AsBase64String(),
                InverseQ = parameters.InverseQ?.AsBase64String(),
                Modulus = parameters.Modulus?.AsBase64String(),
                P = parameters.P?.AsBase64String(),
                Q = parameters.Q?.AsBase64String()
            };
        }

    }
}

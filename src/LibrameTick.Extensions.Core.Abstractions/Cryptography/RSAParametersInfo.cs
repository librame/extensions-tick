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
/// 定义一个用于序列化 <see cref="RSAParameters"/> 的信息（直接序列化 <see cref="RSAParameters"/> 会为空）。
/// </summary>
public class RSAParametersInfo
{
    /// <summary>
    /// 对应 <see cref="RSAParameters"/> D 参数。
    /// </summary>
    public string? D { get; set; }

    /// <summary>
    /// 对应 <see cref="RSAParameters"/> DP 参数。
    /// </summary>
    public string? DP { get; set; }

    /// <summary>
    /// 对应 <see cref="RSAParameters"/> DQ 参数。
    /// </summary>
    public string? DQ { get; set; }

    /// <summary>
    /// 对应 <see cref="RSAParameters"/> Exponent 参数。
    /// </summary>
    public string? Exponent { get; set; }

    /// <summary>
    /// 对应 <see cref="RSAParameters"/> InverseQ 参数。
    /// </summary>
    public string? InverseQ { get; set; }

    /// <summary>
    /// 对应 <see cref="RSAParameters"/> Modulus 参数。
    /// </summary>
    public string? Modulus { get; set; }

    /// <summary>
    /// 对应 <see cref="RSAParameters"/> P 参数。
    /// </summary>
    public string? P { get; set; }

    /// <summary>
    /// 对应 <see cref="RSAParameters"/> Q 参数。
    /// </summary>
    public string? Q { get; set; }


    /// <summary>
    /// 转为 RSA 参数集合。
    /// </summary>
    /// <returns>返回 <see cref="RSAParameters"/>。</returns>
    public virtual RSAParameters ToParameters()
    {
        return new RSAParameters
        {
            D = D?.FromBase64String(),
            DP = DP?.FromBase64String(),
            DQ = DQ?.FromBase64String(),
            Exponent = Exponent?.FromBase64String(),
            InverseQ = InverseQ?.FromBase64String(),
            Modulus = Modulus?.FromBase64String(),
            P = P?.FromBase64String(),
            Q = Q?.FromBase64String()
        };
    }

    /// <summary>
    /// 填充指定 RSA 参数集合。
    /// </summary>
    public virtual void Populate(RSAParameters parameters)
    {
        D = parameters.D?.AsBase64String();
        DP = parameters.DP?.AsBase64String();
        DQ = parameters.DQ?.AsBase64String();
        Exponent = parameters.Exponent?.AsBase64String();
        InverseQ = parameters.InverseQ?.AsBase64String();
        Modulus = parameters.Modulus?.AsBase64String();
        P = parameters.P?.AsBase64String();
        Q = parameters.Q?.AsBase64String();
    }

    /// <summary>
    /// 填充指定 RSA 参数集合信息。
    /// </summary>
    public virtual void Populate(RSAParametersInfo parametersInfo)
    {
        D = parametersInfo.D;
        DP = parametersInfo.DP;
        DQ = parametersInfo.DQ;
        Exponent = parametersInfo.Exponent;
        InverseQ = parametersInfo.InverseQ;
        Modulus = parametersInfo.Modulus;
        P = parametersInfo.P;
        Q = parametersInfo.Q;
    }

}

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
/// 定义一个用于反序列化 <see cref="RSAParameters"/> 信息（当 JSON 反序列化参数字节数组会为空）。
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
}

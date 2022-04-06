#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Autokeys;

namespace Librame.Extensions.Core.Cryptography;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的算法选项。
/// </summary>
public class AlgorithmOptions : CkiOptions
{
    /// <summary>
    /// 构造一个 <see cref="AlgorithmOptions"/>。
    /// </summary>
    public AlgorithmOptions()
    {
        // 初始使用默认 CKI 填充所有基础密钥
        AlgorithmExtensions.PopulateDefaultCki(this);
    }


    /// <summary>
    /// RSA 选项。
    /// </summary>
    public SigningCredentialsOptions Rsa { get; set; } = new();


    /// <summary>
    /// 字符编码（默认使用 <see cref="Encoding.UTF8"/>）。
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.UTF8;


    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回 32 位整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();

    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{nameof(Encoding)}={Encoding.AsEncodingName()};{base.ToString()};{nameof(Rsa)}:{Rsa}";

}

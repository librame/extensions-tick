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
/// 定义一个用于序列化的临时 RSA 密钥（兼容 IdentityServer4 生成的临时密钥文件）。
/// </summary>
public class TemporaryRsaKey
{
    /// <summary>
    /// 密钥标识。
    /// </summary>
    public string? KeyId { get; set; }

    /// <summary>
    /// RSA 参数信息。
    /// </summary>
    public RSAParametersInfo? Parameters { get; set; }


    /// <summary>
    /// 转为 RSA 安全密钥。
    /// </summary>
    /// <param name="requiredPrivateKey">验证必须存在私钥（可选；默认启用验证）。</param>
    /// <returns>返回 <see cref="RsaSecurityKey"/>。</returns>
    public virtual RsaSecurityKey ToRsaSecurityKey(bool requiredPrivateKey = true)
    {
        if (Parameters is null)
            throw new ArgumentNullException(nameof(Parameters));

        var rsaKey = new RsaSecurityKey(Parameters.ToParameters())
        {
            KeyId = KeyId
        };

        if (requiredPrivateKey && rsaKey.PrivateKeyStatus is PrivateKeyStatus.DoesNotExist)
            throw new NotSupportedException("The temporary rsa key does not have a private key.");

        return rsaKey;
    }


    /// <summary>
    /// 生成 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    public static TemporaryRsaKey Generate()
    {
        var rsaKey = new TemporaryRsaKey();

        rsaKey.KeyId = RandomExtensions.GenerateByteArray(16).AsBase64String();

        rsaKey.Parameters = new();
        rsaKey.Parameters.Populate(RSA.Create().ExportParameters(true));

        return rsaKey;
    }

}

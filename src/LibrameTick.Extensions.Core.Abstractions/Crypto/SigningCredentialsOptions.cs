#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Crypto;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的签名证书选项。
/// </summary>
public class SigningCredentialsOptions : IOptions
{
    private SigningCredentials? _credentials;


    /// <summary>
    /// 构造一个 <see cref="SigningCredentialsOptions"/>。
    /// </summary>
    /// <param name="provider">给定的 <see cref="ISigningCredentialsProvider"/>（可选；默认使用支持 JSON 文件格式的 <see cref="RsaSigningCredentialsProvider"/>）。</param>
    public SigningCredentialsOptions(ISigningCredentialsProvider? provider = null)
    {
        Provider = provider ?? new RsaSigningCredentialsProvider(new JsonFileRsaKeyProvider());
    }


    /// <summary>
    /// 签名证书提供程序。
    /// </summary>
    public ISigningCredentialsProvider? Provider { get; set; }

    /// <summary>
    /// 签名证书。
    /// </summary>
    [JsonIgnore]
    public SigningCredentials? Credentials
    {
        get
        {
            if (_credentials is null && Provider is not null)
                _credentials = Provider.Load();

            return _credentials;
        }
        set => _credentials = value;
    }


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
        => $"{nameof(SigningCredentials.Key.KeyId)}:{Credentials?.Key.KeyId},{nameof(SigningCredentials.Key.KeySize)}:{Credentials?.Key.KeySize}";

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Cryptography;

/// <summary>
/// 定义继承 <see cref="AbstractFileRsaKeyProvider"/> 的 JSON 文件型 RSA 密钥提供程序。
/// </summary>
public class JsonFileRsaKeyProvider : AbstractFileRsaKeyProvider
{
    /// <summary>
    /// 构造一个 <see cref="JsonFileRsaKeyProvider"/>。
    /// </summary>
    /// <param name="filePath">给定的文件路径（可选；默认使用 IdentityServer4 生成的临时密钥文件）。</param>
    public JsonFileRsaKeyProvider(string? filePath = null)
        : base(filePath)
    {
    }


    /// <summary>
    /// 加载临时 RSA 密钥。
    /// </summary>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    public override TemporaryRsaKey Load()
    {
        var rsaKey = FilePath.DeserializeJsonFile<TemporaryRsaKey>();
        if (rsaKey is null)
            throw new NotSupportedException("Unsupported rsa key file format.");

        return rsaKey;
    }

    /// <summary>
    /// 保存临时 RSA 密钥。
    /// </summary>
    /// <param name="rsaKey">给定的 <see cref="TemporaryRsaKey"/>。</param>
    /// <returns>返回 <see cref="TemporaryRsaKey"/>。</returns>
    public override TemporaryRsaKey Save(TemporaryRsaKey rsaKey)
    {
        FilePath.SerializeJsonFile(rsaKey);

        return rsaKey;
    }

}

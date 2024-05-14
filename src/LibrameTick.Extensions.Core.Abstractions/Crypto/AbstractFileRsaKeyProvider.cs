#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义抽象实现 <see cref="IFileRsaKeyProvider"/> 的文件型 RSA 密钥提供程序。
/// </summary>
public abstract class AbstractFileRsaKeyProvider : AbstractRsaKeyProvider, IFileRsaKeyProvider
{
    /// <summary>
    /// 构造一个 <see cref="AbstractFileRsaKeyProvider"/>。
    /// </summary>
    /// <param name="filePath">给定的文件路径（可选；默认使用 IdentityServer4 生成的临时密钥文件）。</param>
    protected AbstractFileRsaKeyProvider(string? filePath = null)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            // 默认 IdentityServer4 生成的临时密钥文件存放于应用根目录
            filePath = "tempkey.rsa".SetBasePath();
        }

        FilePath = filePath;
    }


    /// <summary>
    /// 文件路径。
    /// </summary>
    public string FilePath { get; init; }


    /// <summary>
    /// 存在临时 RSA 密钥。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public override bool Exist()
        => File.Exists(FilePath);

}

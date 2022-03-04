#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Autokeys;

/// <summary>
/// 定义抽象实现 <see cref="IFileAutokeyProvider"/> 的文件型自动密钥提供程序。
/// </summary>
public abstract class AbstractFileAutokeyProvider : AbstractAutokeyProvider, IFileAutokeyProvider
{
    /// <summary>
    /// 构造一个 <see cref="AbstractFileAutokeyProvider"/>。
    /// </summary>
    /// <param name="filePath">给定的文件路径（可选）。</param>
    protected AbstractFileAutokeyProvider(string? filePath = null)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            // 默认支持格式为“当前项目程序集名.autokey”的文件名
            // 且限定自动密钥文件需存放在与程序集文件相同的目录
            filePath = $"{typeof(Autokey).Assembly.GetName().Name}.autokey"
                .SetBasePath(PathExtensions.CurrentDirectory);
        }

        FilePath = filePath;
    }


    /// <summary>
    /// 文件路径。
    /// </summary>
    public string FilePath { get; init; }


    /// <summary>
    /// 存在自动密钥。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public override bool Exist()
        => File.Exists(FilePath);

}

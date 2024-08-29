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
/// 定义抽象实现 <see cref="IFilePlugin"/> 的文件插件。
/// </summary>
/// <param name="path">给定的 <see cref="FluentFilePath"/>。</param>
public abstract class FilePlugin(FluentFilePath path) : IFilePlugin
{
    /// <summary>
    /// 获取插件要应用的文件路径。
    /// </summary>
    /// <value>
    /// 返回 <see cref="FluentFilePath"/>。
    /// </value>
    public FluentFilePath Path { get; init; } = path;
}

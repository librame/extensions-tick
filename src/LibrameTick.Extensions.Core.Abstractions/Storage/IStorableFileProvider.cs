#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Storage;

/// <summary>
/// 定义继承 <see cref="IFileProvider"/> 的可存储文件提供程序接口。
/// </summary>
public interface IStorableFileProvider : IFileProvider
{
    /// <summary>
    /// 获取文件信息。
    /// </summary>
    /// <param name="subpath">给定的子路径。</param>
    /// <returns>返回 <see cref="IStorableFileInfo"/>。</returns>
    new IStorableFileInfo GetFileInfo(string subpath);

    /// <summary>
    /// 获取目录内容集合。
    /// </summary>
    /// <param name="subpath">给定的子路径。</param>
    /// <returns>返回 <see cref="IStorableDirectoryContents"/>。</returns>
    new IStorableDirectoryContents GetDirectoryContents(string subpath);
}

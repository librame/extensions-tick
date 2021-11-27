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
/// 定义可存储文件管理器接口。
/// </summary>
public interface IStorableFileManager
{
    /// <summary>
    /// 存储进度动作。
    /// </summary>
    Action<StorageProgressDescriptor>? ProgressAction { get; set; }


    /// <summary>
    /// 异步获取文件信息。
    /// </summary>
    /// <param name="subpath">给定的子路径。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="Task{IStorageFileInfo}"/>。</returns>
    Task<IStorableFileInfo> GetFileInfoAsync(string subpath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步获取目录内容集合。
    /// </summary>
    /// <param name="subpath">给定的子路径。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="Task{IStorableDirectoryContents}"/>。</returns>
    Task<IStorableDirectoryContents> GetDirectoryContentsAsync(string subpath,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// 异步读取字符串。
    /// </summary>
    /// <param name="subpath">给定的子路径。</param>
    /// <returns>返回 <see cref="Task{String}"/>。</returns>
    Task<string> ReadStringAsync(string subpath);

    /// <summary>
    /// 异步读取字符串。
    /// </summary>
    /// <param name="fileInfo">给定的 <see cref="IStorableFileInfo"/>。</param>
    /// <returns>返回 <see cref="Task{String}"/>。</returns>
    Task<string> ReadStringAsync(IStorableFileInfo fileInfo);

    /// <summary>
    /// 异步读取。
    /// </summary>
    /// <param name="fileInfo">给定的 <see cref="IStorableFileInfo"/>。</param>
    /// <param name="writeStream">给定的写入 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    Task ReadAsync(IStorableFileInfo fileInfo, Stream writeStream, CancellationToken cancellationToken = default);


    /// <summary>
    /// 异步写入字符串。
    /// </summary>
    /// <param name="fileInfo">给定的 <see cref="IStorableFileInfo"/>。</param>
    /// <param name="content">给定的写入字符串。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    Task WriteStringAsync(IStorableFileInfo fileInfo, string content);

    /// <summary>
    /// 异步写入。
    /// </summary>
    /// <param name="fileInfo">给定的 <see cref="IStorableFileInfo"/>。</param>
    /// <param name="readStream">给定的读取 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    Task WriteAsync(IStorableFileInfo fileInfo, Stream readStream, CancellationToken cancellationToken = default);
}

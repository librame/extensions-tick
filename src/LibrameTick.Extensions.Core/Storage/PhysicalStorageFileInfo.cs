#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Librame.Extensions.Core.Storage
{
    /// <summary>
    /// 定义实现 <see cref="IStorageFileInfo"/> 的物理存储文件信息。
    /// </summary>
    public class PhysicalStorageFileInfo : IStorageFileInfo
    {
        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageFileInfo"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="FileInfo"/>。</param>
        public PhysicalStorageFileInfo(FileInfo info)
        {
            Exists = info.Exists;
            Length = info.Length;
            PhysicalPath = info.FullName;
            Name = info.Name;
            LastModified = info.LastWriteTimeUtc;
            IsDirectory = false;
        }

        /// <summary>
        /// 构造一个 <see cref="PhysicalStorageFileInfo"/>。
        /// </summary>
        /// <param name="info">给定的 <see cref="IFileInfo"/>。</param>
        public PhysicalStorageFileInfo(IFileInfo info)
        {
            Exists = info.Exists;
            Length = info.Length;
            PhysicalPath = info.PhysicalPath;
            Name = info.Name;
            LastModified = info.LastModified;
            IsDirectory = info.IsDirectory;
        }


        /// <summary>
        /// 是否存在。
        /// </summary>
        public bool Exists { get; init; }

        /// <summary>
        /// 大小。
        /// </summary>
        public long Length { get; init; }

        /// <summary>
        /// 物理路径。
        /// </summary>
        public string PhysicalPath { get; init; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// 最后修改时间。
        /// </summary>
        public DateTimeOffset LastModified { get; init; }

        /// <summary>
        /// 是目录。
        /// </summary>
        public bool IsDirectory { get; init; }


        /// <summary>
        /// 创建读取流。
        /// </summary>
        /// <returns>返回 <see cref="Stream"/>。</returns>
        public Stream CreateReadStream()
        {
            // 将缓冲区大小设置为 1，以防止 FileStream 分配它的内部缓冲区 0 导致构造函数抛出异常
            var bufferSize = 1;

            return new FileStream(
                PhysicalPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite,
                bufferSize,
                RequestOptions.Asynchronous | RequestOptions.SequentialScan);
        }

        /// <summary>
        /// 创建写入流。
        /// </summary>
        /// <returns>返回 <see cref="Stream"/>。</returns>
        public Stream CreateWriteStream()
        {
            // 将缓冲区大小设置为 1，以防止 FileStream 分配它的内部缓冲区 0 导致构造函数抛出异常
            var bufferSize = 1;

            return new FileStream(
                PhysicalPath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.ReadWrite,
                bufferSize,
                RequestOptions.Asynchronous | RequestOptions.SequentialScan);
        }

    }
}

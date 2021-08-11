#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Storage
{
    class InternalFileManager : IFileManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CoreExtensionOptions _options;
        private readonly IStorageFileProvider _fileProvider;


        public InternalFileManager(IMemoryCache memoryCache, CoreExtensionBuilder builder)
        {
            _memoryCache = memoryCache;
            _options = builder.Options;

            if (_options.Requests.FileProviders.Count > 0)
                throw new ArgumentException("");
            
            _fileProvider = new CompositeStorageFileProvider(_options.Requests.FileProviders);
        }


        public Action<StorageProgressDescriptor>? ProgressAction { get; set; }


        public Task<IStorageFileInfo> GetFileInfoAsync(string subpath,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => _fileProvider.GetFileInfo(subpath));

        public Task<IStorageDirectoryContents> GetDirectoryContentsAsync(string subpath,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => _fileProvider.GetDirectoryContents(subpath));


        public Task<string> ReadStringAsync(string subpath)
        {
            var fileInfo = _fileProvider.GetFileInfo(subpath);
            return _memoryCache.GetOrCreateAsync(fileInfo.PhysicalPath, entry =>
            {
                entry.AddExpirationToken(_fileProvider.Watch(subpath));
                return ReadStringAsync(fileInfo);
            });
        }

        public Task<string> ReadStringAsync(IStorageFileInfo fileInfo)
        {
            using (var s = fileInfo.CreateReadStream())
            using (var sr = new StreamReader(s))
            {
                return sr.ReadToEndAsync();
            }
        }

        /// <summary>
        /// 异步读取。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="writeStream">给定的写入 <see cref="Stream"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public async Task ReadAsync(IStorageFileInfo fileInfo, Stream writeStream,
            CancellationToken cancellationToken = default)
        {
            var buffer = new byte[_options.Requests.BufferSize];

            if (writeStream.CanSeek)
                writeStream.Seek(0, SeekOrigin.Begin);

            using (var readStream = fileInfo.CreateReadStream())
            {
                var currentCount = 1;
                while (currentCount > 0)
                {
                    // 每次从文件流中读取指定缓冲区的字节数，当读完后退出循环
                    currentCount = await readStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait();

                    // 将读取到的缓冲区字节数写入请求流
                    await writeStream.WriteAsync(buffer, 0, currentCount, cancellationToken).ConfigureAwait();

                    if (ProgressAction != null)
                    {
                        var descriptor = new StorageProgressDescriptor(
                        readStream.Length, readStream.Position,
                        writeStream.Length, writeStream.Position, currentCount);

                        ProgressAction.Invoke(descriptor);
                    }
                }
            }
        }


        /// <summary>
        /// 异步写入字符串。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="content">给定的写入字符串。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public Task WriteStringAsync(IStorageFileInfo fileInfo, string content)
        {
            fileInfo.NotNull(nameof(fileInfo));
            content.NotEmpty(nameof(content));

            using (var readStream = fileInfo.CreateReadStream())
            using (var sw = new StreamWriter(readStream))
            {
                return sw.WriteAsync(content);
            }
        }

        /// <summary>
        /// 异步写入。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="readStream">给定的读取 <see cref="Stream"/>。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public async Task WriteAsync(IStorageFileInfo fileInfo, Stream readStream, CancellationToken cancellationToken = default)
        {
            var buffer = new byte[_options.Requests.BufferSize];

            if (readStream.CanSeek)
                readStream.Seek(0, SeekOrigin.Begin);

            using (var writeStream = fileInfo.CreateWriteStream())
            {
                var currentCount = 1;
                while (currentCount > 0)
                {
                    // 每次从文件流中读取指定缓冲区的字节数，当读完后退出循环
                    currentCount = await readStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait();

                    // 将读取到的缓冲区字节数写入请求流
                    await writeStream.WriteAsync(buffer, 0, currentCount, cancellationToken).ConfigureAwait();

                    if (ProgressAction != null)
                    {
                        var descriptor = new StorageProgressDescriptor(
                        readStream.Length, readStream.Position,
                        writeStream.Length, writeStream.Position, currentCount);

                        ProgressAction.Invoke(descriptor);
                    }
                }
            }
        }

    }
}

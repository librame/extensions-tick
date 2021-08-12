﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;

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

            if (_options.Requests.FileProviders.Count < 0)
                throw new ArgumentException($"The {nameof(CoreExtensionOptions)}.{nameof(_options.Requests)}.{nameof(_options.Requests.FileProviders)} is empty. ex: services.AddLibrame(opts => opts.{nameof(_options.Requests)}.{nameof(_options.Requests.FileProviders)}.Add(new {nameof(PhysicalStorageFileProvider)}()))");
            
            _fileProvider = new InternalCompositeStorageFileProvider(_options.Requests.FileProviders);
        }


        public Action<StorageProgressDescriptor>? ProgressAction { get; set; }


        public Task<IStorageFileInfo> GetFileInfoAsync(string subpath,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => _fileProvider.GetFileInfo(subpath));

        public Task<IStorageDirectoryContents> GetDirectoryContentsAsync(string subpath,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(() => _fileProvider.GetDirectoryContents(subpath));


        #region Read

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
            using (var rs = fileInfo.CreateReadStream())
            using (var sr = new StreamReader(rs))
            {
                return sr.ReadToEndAsync();
            }
        }


        public async Task ReadAsync(IStorageFileInfo fileInfo, Stream writeStream,
            CancellationToken cancellationToken = default)
        {
            if (writeStream.CanSeek)
                writeStream.Seek(0, SeekOrigin.Begin);

            using (var rs = fileInfo.CreateReadStream())
            {
                var processingSize = 0L;
                var processingSpeed = 0L;
                var beginSecond = DateTime.Now.Second;

                var readLength = 0;
                var buffer = new byte[_options.Requests.BufferSize];

                while ((readLength = await rs.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait()) > 0)
                {
                    await writeStream.WriteAsync(buffer, 0, readLength, cancellationToken).ConfigureAwait();

                    processingSize += readLength;
                    processingSpeed += readLength;

                    if (ProgressAction != null)
                    {
                        var endSecond = DateTime.Now.Second;

                        if (beginSecond != endSecond)
                            processingSpeed = processingSpeed / (endSecond - beginSecond);

                        ProgressAction.Invoke(new StorageProgressDescriptor
                        {
                            ContentLength = fileInfo.Length,
                            StartPosition = 0,
                            ProcessingSize = processingSize,
                            ProcessingSpeed = processingSpeed,
                            ProcessingPercent = Math.Max((int)(processingSize * 100 / fileInfo.Length), 1)
                        });

                        if (beginSecond != endSecond)
                        {
                            beginSecond = DateTime.Now.Second;
                            processingSpeed = 0;
                        }
                    }
                }
            }
        }

        #endregion


        #region Write

        /// <summary>
        /// 异步写入字符串。
        /// </summary>
        /// <param name="fileInfo">给定的 <see cref="IStorageFileInfo"/>。</param>
        /// <param name="content">给定的写入字符串。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public Task WriteStringAsync(IStorageFileInfo fileInfo, string content)
        {
            using (var rs = fileInfo.CreateReadStream())
            using (var sw = new StreamWriter(rs))
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
            if (readStream.CanSeek)
                readStream.Seek(0, SeekOrigin.Begin);

            using (var writeStream = fileInfo.CreateWriteStream())
            {
                var processingSize = 0L;
                var processingSpeed = 0L;
                var beginSecond = DateTime.Now.Second;

                var readLength = 0;
                var buffer = new byte[_options.Requests.BufferSize];

                while ((readLength = await readStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait()) > 0)
                {
                    await writeStream.WriteAsync(buffer, 0, readLength, cancellationToken).ConfigureAwait();

                    processingSize += readLength;
                    processingSpeed += readLength;

                    if (ProgressAction != null)
                    {
                        var endSecond = DateTime.Now.Second;

                        if (beginSecond != endSecond)
                            processingSpeed = processingSpeed / (endSecond - beginSecond);

                        ProgressAction.Invoke(new StorageProgressDescriptor
                        {
                            ContentLength = fileInfo.Length,
                            StartPosition = 0,
                            ProcessingSize = processingSize,
                            ProcessingSpeed = processingSpeed,
                            ProcessingPercent = Math.Max((int)(processingSize * 100 / fileInfo.Length), 1)
                        });

                        if (beginSecond != endSecond)
                        {
                            beginSecond = DateTime.Now.Second;
                            processingSpeed = 0;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
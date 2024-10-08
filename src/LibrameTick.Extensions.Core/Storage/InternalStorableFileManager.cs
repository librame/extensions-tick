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

namespace Librame.Extensions.Storage;

internal sealed class InternalStorableFileManager : IStorableFileManager
{
    private readonly IMemoryCache _memoryCache;
    private readonly IOptionsMonitor<CoreExtensionOptions> _optionsMonitor;
    private readonly IStorableFileProvider _fileProvider;


    public InternalStorableFileManager(IMemoryCache memoryCache,
        IOptionsMonitor<CoreExtensionOptions> optionsMonitor)
    {
        _memoryCache = memoryCache;
        _optionsMonitor = optionsMonitor;

        var fileProviders = optionsMonitor.CurrentValue.WebFile.FileProviders;

        if (fileProviders.Count < 0)
            throw new ArgumentException($"The {nameof(fileProviders)} is empty. ex: services.AddLibrame(opts => opts.{nameof(Options.WebFile)}.{nameof(Options.WebFile.FileProviders)}.Add(new {nameof(PhysicalStorableFileProvider)}()))");
            
        _fileProvider = new InternalCompositeStorableFileProvider(fileProviders);
    }


    public CoreExtensionOptions Options
        => _optionsMonitor.CurrentValue;

    public int BufferSize
        => Options.WebFile.BufferSize;


    public Action<StorageProgressDescriptor>? ProgressAction { get; set; }


    public Task<IStorableFileInfo> GetFileInfoAsync(string subpath,
        CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTaskResult(() => _fileProvider.GetFileInfo(subpath));

    public Task<IStorableDirectoryContents> GetDirectoryContentsAsync(string subpath,
        CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTaskResult(() => _fileProvider.GetDirectoryContents(subpath));


    #region Read

    public async Task<string> ReadStringAsync(string subpath)
    {
        var fileInfo = _fileProvider.GetFileInfo(subpath);

        ArgumentNullException.ThrowIfNull(fileInfo.PhysicalPath);

        return (await _memoryCache.GetOrCreateAsync(fileInfo.PhysicalPath, entry =>
        {
            entry.AddExpirationToken(_fileProvider.Watch(subpath));

            return ReadStringAsync(fileInfo);
        }))
        ?? string.Empty;
    }

    public Task<string> ReadStringAsync(IStorableFileInfo fileInfo)
    {
        using (var rs = fileInfo.CreateReadStream())
        using (var sr = new StreamReader(rs))
        {
            return sr.ReadToEndAsync();
        }
    }


    public async Task ReadAsync(IStorableFileInfo fileInfo, Stream writeStream,
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
            var buffer = BufferSize.RentByteArray();

            while ((readLength = await rs.ReadAsync(buffer, 0, buffer.Length, cancellationToken).AvoidCapturedContext()) > 0)
            {
                await writeStream.WriteAsync(buffer, 0, readLength, cancellationToken).AvoidCapturedContext();

                processingSize += readLength;
                processingSpeed += readLength;

                if (ProgressAction is not null)
                {
                    var endSecond = DateTime.Now.Second;

                    if (beginSecond != endSecond)
                        processingSpeed = processingSpeed / (endSecond - beginSecond);

                    ProgressAction(new StorageProgressDescriptor
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

            buffer.ReturnArray();
        }
    }

    #endregion


    #region Write

    /// <summary>
    /// 异步写入字符串。
    /// </summary>
    /// <param name="fileInfo">给定的 <see cref="IStorableFileInfo"/>。</param>
    /// <param name="content">给定的写入字符串。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    public Task WriteStringAsync(IStorableFileInfo fileInfo, string content)
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
    /// <param name="fileInfo">给定的 <see cref="IStorableFileInfo"/>。</param>
    /// <param name="readStream">给定的读取 <see cref="Stream"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    public async Task WriteAsync(IStorableFileInfo fileInfo, Stream readStream, CancellationToken cancellationToken = default)
    {
        if (readStream.CanSeek)
            readStream.Seek(0, SeekOrigin.Begin);

        using (var writeStream = fileInfo.CreateWriteStream())
        {
            var processingSize = 0L;
            var processingSpeed = 0L;
            var beginSecond = DateTime.Now.Second;

            var readLength = 0;
            var buffer = BufferSize.RentByteArray();

            while ((readLength = await readStream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).AvoidCapturedContext()) > 0)
            {
                await writeStream.WriteAsync(buffer, 0, readLength, cancellationToken).AvoidCapturedContext();

                processingSize += readLength;
                processingSpeed += readLength;

                if (ProgressAction is not null)
                {
                    var endSecond = DateTime.Now.Second;

                    if (beginSecond != endSecond)
                        processingSpeed = processingSpeed / (endSecond - beginSecond);

                    ProgressAction(new StorageProgressDescriptor
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

            buffer.ReturnArray();
        }
    }

    #endregion

}

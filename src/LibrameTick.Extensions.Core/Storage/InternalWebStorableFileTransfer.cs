#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Network;

namespace Librame.Extensions.Core.Storage;

class InternalWebStorableFileTransfer : IWebStorableFileTransfer
{
    private readonly IWebFilePermission _permission;
    private readonly IHttpClientInvokerFactory _factory;
    private readonly CoreExtensionOptions _options;


    public InternalWebStorableFileTransfer(IWebFilePermission permission,
        IHttpClientInvokerFactory factory, IOptionsMonitor<CoreExtensionOptions> options)
    {
        _permission = permission;
        _factory = factory;
        _options = options.CurrentValue;
        
        Encoding = _options.Algorithm.Encoding;
        BufferSize = _options.WebFile.BufferSize;
    }


    public Encoding Encoding { get; }

    public int BufferSize { get; set; }


    public bool UseAccessToken { get; set; }

    public bool UseBasicAuthentication { get; set; }

    public bool UseBearerAuthentication { get; set; }

    public bool UseCookieValue { get; set; }

    public bool UseBreakpointResume { get; set; }
        = true;


    public Action<StorageProgressDescriptor>? ProgressAction { get; set; }


    #region Download

    public async Task<string> DownloadFileAsync(string downloadUri, string savePath,
        CancellationToken cancellationToken = default)
    {
        // 获取本地文件最后一次写入的结束位置
        var startPosition = await GetFileLastEndPositionAsync(savePath, cancellationToken).ConfigureAwait();

        var client = await CreateClientAsync(cancellationToken).ConfigureAwait();
        var message = await client.GetAsync(downloadUri, HttpCompletionOption.ResponseHeadersRead,
            cancellationToken).ConfigureAwait();
            
        var contentLength = message.Content.Headers.ContentLength;
        if (contentLength is null || contentLength.HasValue && contentLength < 1)
            return string.Empty;

        using (var rs = await message.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait())
        {
            if (rs.CanSeek)
                rs.Position = startPosition;

            using (var fs = new FileStream(savePath, FileMode.Append, FileAccess.Write))
            {
                var processingSize = 0L;
                var processingSpeed = 0L;
                var beginSecond = DateTime.Now.Second;

                var readLength = 0;
                var buffer = new byte[BufferSize];

                while ((readLength = await rs.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait()) > 0)
                {
                    await fs.WriteAsync(buffer, 0, readLength).ConfigureAwait();

                    processingSize += readLength;
                    processingSpeed += readLength;

                    if (ProgressAction is not null)
                    {
                        var endSecond = DateTime.Now.Second;

                        if (beginSecond != endSecond)
                            processingSpeed = processingSpeed / (endSecond - beginSecond);

                        ProgressAction(new StorageProgressDescriptor
                        {
                            ContentLength = contentLength.Value,
                            StartPosition = startPosition,
                            ProcessingSize = processingSize,
                            ProcessingSpeed = processingSpeed,
                            ProcessingPercent = Math.Max((int)(processingSize * 100 / contentLength), 1)
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

        return savePath;
    }

    private async Task<long> GetFileLastEndPositionAsync(string savePath,
        CancellationToken cancellationToken = default)
    {
        if (File.Exists(savePath) && UseBreakpointResume)
        {
            using (var fs = File.OpenRead(savePath))
            {
                var buffer = new byte[BufferSize];

                while (await fs.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait() > 0) ;

                return fs.Position;
            }
        }

        return 0L;
    }

    #endregion


    #region Upload

    public async Task<string> UploadFileAsync(string uploadUri, string filePath,
        string? saveFileNameStar = null, string? saveFileName = null,
        CancellationToken cancellationToken = default)
    {
        var client = await CreateClientAsync(cancellationToken: cancellationToken).ConfigureAwait();

        using (var content = new MultipartFormDataContent())
        {
            // 上传暂不支持续传
            var fileBytes = await GetFileByteArrayAsync(filePath, startPosition: 0,
                cancellationToken).ConfigureAwait(false);

            var bytesContent = new ByteArrayContent(fileBytes);

            // 设置上传后保存的前缀路径和文件名称（文件名称如果重复会特殊处理）
            bytesContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            // 保存文件名前缀
            if (!string.IsNullOrEmpty(saveFileNameStar))
                bytesContent.Headers.ContentDisposition.FileNameStar = saveFileNameStar;

            // 保存文件名
            bytesContent.Headers.ContentDisposition.FileName = string.IsNullOrEmpty(saveFileName)
                ? Path.GetFileName(filePath) : saveFileName;

            content.Add(bytesContent);

            var message = await client.PostAsync(uploadUri, content).ConfigureAwait();

            return await message.Content.ReadAsStringAsync().ConfigureAwait();
        }
    }

    private async Task<byte[]> GetFileByteArrayAsync(string filePath,
        long? startPosition = null, CancellationToken cancellationToken = default)
    {
        if (File.Exists(filePath))
        {
            byte[] allBytes;

            using (var fs = File.OpenRead(filePath))
            {
                if (UseBreakpointResume && fs.CanSeek &&
                    startPosition is not null && startPosition.Value > 0)
                {
                    fs.Position = startPosition.Value;
                    allBytes = new byte[fs.Length - startPosition.Value];
                }
                else
                {
                    allBytes = new byte[fs.Length];
                }

                var readLength = 0;
                var buffer = new byte[BufferSize];

                while ((readLength = await fs.ReadAsync(buffer, 0, buffer.Length,
                    cancellationToken).ConfigureAwait()) > 0)
                {
                    Array.Copy(buffer, allBytes, readLength);
                }
            }

            return allBytes;
        }

        return Array.Empty<byte>();
    }

    #endregion


    private async Task<HttpClient> CreateClientAsync(CancellationToken cancellationToken = default)
    {
        var client = _factory.CreateClient(_options.WebFile.HttpClientInvoking);
        
        // Authentication: Basic
        if (UseBasicAuthentication)
        {
            var parameter = await _permission.GetBasicCodeAsync(cancellationToken).ConfigureAwait();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", parameter);
        }

        // Authentication: Bearer
        if (UseBearerAuthentication)
        {
            var parameter = await _permission.GetBearerTokenAsync(cancellationToken).ConfigureAwait();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parameter);
        }

        // AccessToken
        if (UseAccessToken)
        {
            var accessToken = await _permission.GetAccessTokenAsync(cancellationToken).ConfigureAwait();
            client.DefaultRequestHeaders.Add("access_token", accessToken);
        }

        // Cookie
        if (UseCookieValue)
        {
            var cookieValue = await _permission.GetCookieValueAsync(cancellationToken).ConfigureAwait();
            client.DefaultRequestHeaders.Add(nameof(HttpRequestHeader.Cookie), cookieValue);
        }

        return client;
    }

}

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
/// 定义文件传输接口。
/// </summary>
public interface IFileTransmission
{
    /// <summary>
    /// 字符编码。
    /// </summary>
    Encoding Encoding { get; set; }

    /// <summary>
    /// 缓冲区大小。
    /// </summary>
    int BufferSize { get; set; }

    /// <summary>
    /// 连接超时。
    /// </summary>
    TimeSpan Timeout { get; set; }


    /// <summary>
    /// 使用访问令牌。
    /// </summary>
    bool UseAccessToken { get; set; }

    /// <summary>
    /// 使用基础认证（安全程度低，多用于路由器和嵌入式设备，而且往往不会使用 HTTPS）。
    /// </summary>
    bool UseBasicAuthentication { get; set; }

    /// <summary>
    /// 使用 JWT 认证访问令牌。
    /// </summary>
    bool UseBearerAuthentication { get; set; }

    /// <summary>
    /// 使用 Cookie 值。
    /// </summary>
    bool UseCookieValue { get; set; }

    /// <summary>
    /// 使用断点续传（默认使用）。
    /// </summary>
    bool UseBreakpointResume { get; set; }


    /// <summary>
    /// 进度动作。
    /// </summary>
    Action<StorageProgressDescriptor>? ProgressAction { get; set; }


    /// <summary>
    /// 异步下载文件。
    /// </summary>
    /// <param name="downloadUri">给定用于下载文件的远程 URL。</param>
    /// <param name="savePath">给定的保存路径。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含保存路径字符串的异步操作。</returns>
    Task<string> DownloadFileAsync(string downloadUri, string savePath,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步上传文件。
    /// </summary>
    /// <param name="uploadUri">给定用于接收上传文件的远程 URI。</param>
    /// <param name="filePath">给定的文件路径。</param>
    /// <param name="saveFileNameStar">给定要保存的文件名前缀。</param>
    /// <param name="saveFileName">给定要保存的文件名。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含远程响应字符串的异步操作。</returns>
    Task<string> UploadFileAsync(string uploadUri, string filePath,
        string? saveFileNameStar = null, string? saveFileName = null,
        CancellationToken cancellationToken = default);
}

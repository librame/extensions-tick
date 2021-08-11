#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Storage
{
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
        /// 使用基础认证。
        /// </summary>
        bool UseBasic { get; set; }

        /// <summary>
        /// 使用访问令牌。
        /// </summary>
        bool UseAccessToken { get; set; }

        /// <summary>
        /// 使用授权码。
        /// </summary>
        bool UseAuthorizationCode { get; set; }

        /// <summary>
        /// 使用 Cookie 值。
        /// </summary>
        bool UseCookieValue { get; set; }

        /// <summary>
        /// 使用断点续传（默认使用）。
        /// </summary>
        bool UseBreakpointResume { get; set; }


        /// <summary>
        /// 进度动作（传入参数依次为总长度、当前位置）。
        /// </summary>
        Action<long, long>? ProgressAction { get; set; }


        /// <summary>
        /// 异步下载文件。
        /// </summary>
        /// <param name="downloadUrl">给定用于下载文件的远程 URL。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含保存路径字符串的异步操作。</returns>
        Task<string> DownloadFileAsync(string downloadUrl, string savePath,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步上传文件。
        /// </summary>
        /// <param name="uploadUrl">给定用于接收上传文件的远程 URL。</param>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <returns>返回一个包含远程响应字符串数组的异步操作。</returns>
        Task<string> UploadFileAsync(string uploadUrl, string filePath,
            CancellationToken cancellationToken = default);
    }
}

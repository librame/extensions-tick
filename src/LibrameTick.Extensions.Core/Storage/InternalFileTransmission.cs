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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Storage
{
    class InternalFileTransmission : IFileTransmission
    {
        private readonly IFilePermission _permission;
        private readonly IHttpClientFactory _factory;
        private readonly CoreExtensionOptions _options;


        public InternalFileTransmission(IFilePermission permission,
            IHttpClientFactory factory, CoreExtensionBuilder builder)
        {
            _permission = permission;
            _factory = factory;
            _options = builder.Options;

            Encoding = _options.Encoding;
            BufferSize = _options.Requests.BufferSize;
            Timeout = _options.Requests.Timeout;
        }


        public Encoding Encoding { get; set; }

        public int BufferSize { get; set; }

        public TimeSpan Timeout { get; set; }


        public bool UseAccessToken { get; set; }

        public bool UseAuthorizationCode { get; set; }

        public bool UseCookieValue { get; set; }

        public bool UseBreakpointResume { get; set; }
            = true;


        public Action<long, long>? ProgressAction { get; set; }


        public async Task<string> DownloadFileAsync(string downloadUrl, string savePath,
            CancellationToken cancellationToken = default)
        {
            var hwr = await CreateRequestAsync(downloadUrl, "GET", cancellationToken).ConfigureAwait();

            var buffer = new byte[BufferSize];
            var range = 0L;

            if (File.Exists(savePath) && UseBreakpointResume)
            {
                using (var fs = File.OpenRead(savePath))
                {
                    var readCount = 1;
                    while (readCount > 0)
                    {
                        // 每次从流中读取指定缓冲区的字节数，当读完后退出循环
                        readCount = fs.Read(buffer, 0, buffer.Length);
                    }

                    range = fs.Position;
                }

                hwr.AddRange(range);
            }

            using (var wr = hwr.GetResponse())
            {
                // Accept-Ranges: bytes or none.
                var acceptRanges = wr.Headers[HttpResponseHeader.AcceptRanges];
                var supportRanges = acceptRanges?.Contains("bytes");

                using (var s = wr.GetResponseStream())
                {
                    var writeMode = FileMode.Create;

                    // 如果需要且服务端支持 Ranges，才启用续传
                    if (range > 0 && supportRanges.HasValue && supportRanges.Value && s.CanSeek)
                    {
                        s.Seek(range, SeekOrigin.Begin);
                        writeMode = FileMode.Append;
                    }

                    using (var fs = File.Open(savePath, writeMode))
                    {
                        var readCount = 1;
                        while (readCount > 0)
                        {
                            // 每次从流中读取指定缓冲区的字节数，当读完后退出循环
                            readCount = s.Read(buffer, 0, buffer.Length);

                            // 将读取到的缓冲区字节数写入文件流
                            fs.Write(buffer, 0, readCount);

                            ProgressAction?.Invoke(s.Length, fs.Position);
                        }
                    }
                }
            }

            return savePath;
        }

        public async Task<string> UploadFileAsync(string uploadUrl, string filePath,
            CancellationToken cancellationToken = default)
        {
            string response = string.Empty;

            var hwr = await CreateRequestAsync(uploadUrl, cancellationToken: cancellationToken).ConfigureAwait();

            using (var s = hwr.GetRequestStream())
            {
                var buffer = new byte[BufferSize];

                using (var fs = File.OpenRead(filePath))
                {
                    var readCount = 1;
                    while (readCount > 0)
                    {
                        // 每次从文件流中读取指定缓冲区的字节数，当读完后退出循环
                        readCount = fs.Read(buffer, 0, buffer.Length);

                        // 将读取到的缓冲区字节数写入请求流
                        s.Write(buffer, 0, readCount);

                        ProgressAction?.Invoke(fs.Length, s.Position);
                    }
                }
            }

            using (var s = hwr.GetResponse().GetResponseStream())
            {
                using (var sr = new StreamReader(s, Encoding))
                {
                    response = await sr.ReadToEndAsync().ConfigureAwait();
                }
            }

            return response;
        }

        private async Task<HttpWebRequest> CreateRequestAsync(string url,
            CancellationToken cancellationToken = default)
        {
            var client = _factory.CreateClient();

            //var authentication = new AuthenticationHeaderValue(
            //    "Basic",
            //    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{password}")
            //    ));

            //client.DefaultRequestHeaders.Authorization = authentication;

            client.Timeout = Timeout;

            client.DefaultRequestHeaders.UserAgent.TryParseAdd(_options.Requests.UserAgent);

            //if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
            //    hwr.ContentType = $"application/x-www-form-urlencoded; charset={Encoding.AsEncodingName()}";

            //if (UseAccessToken)
            //{
            //    var accessToken = await _permission.GetAccessTokenAsync(cancellationToken).ConfigureAwait();
            //    hwr.Headers.Add("access_token", accessToken);
            //}

            //if (UseAuthorizationCode)
            //{
            //    var authorizationCode = await _permission.GetAuthorizationCodeAsync(cancellationToken).ConfigureAwait();
            //    hwr.Headers.Add(HttpRequestHeader.Authorization, authorizationCode);
            //}

            //if (UseCookieValue)
            //{
            //    var cookieValue = await _permission.GetCookieValueAsync(cancellationToken).ConfigureAwait();
            //    hwr.Headers.Add(HttpRequestHeader.Cookie, cookieValue);
            //}

            return hwr;
        }

    }
}

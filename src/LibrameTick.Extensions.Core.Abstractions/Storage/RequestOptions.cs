#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Serialization;
using System.Text.Json.Serialization;

namespace Librame.Extensions.Core.Storage
{
    /// <summary>
    /// 定义实现 <see cref="IOptions"/> 的请求选项。
    /// </summary>
    public class RequestOptions : AbstractOptions
    {
        /// <summary>
        /// 使用给定的 <see cref="IPropertyNotifier"/> 构造一个 <see cref="RequestOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        /// <param name="sourceAliase">给定的源别名（可选）。</param>
        public RequestOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
            : base(parentNotifier, sourceAliase)
        {
        }


        /// <summary>
        /// 认证用户。
        /// </summary>
        public string AuthUsername
        {
            get => Notifier.GetOrAdd(nameof(AuthUsername), nameof(AuthUsername));
            set => Notifier.AddOrUpdate(nameof(AuthUsername), value);
        }

        /// <summary>
        /// 认证密码。
        /// </summary>
        [JsonConverter(typeof(JsonStringEncryptionConverter))]
        public string AuthPassword
        {
            get => Notifier.GetOrAdd(nameof(AuthPassword), nameof(AuthPassword));
            set => Notifier.AddOrUpdate(nameof(AuthPassword), value);
        }

        /// <summary>
        /// 认证 JWT 令牌。
        /// </summary>
        public string AuthJwtToken
        {
            get => Notifier.GetOrAdd(nameof(AuthJwtToken), nameof(AuthJwtToken));
            set => Notifier.AddOrUpdate(nameof(AuthJwtToken), value);
        }

        /// <summary>
        /// 认证访问令牌。
        /// </summary>
        public string AuthAccessToken
        {
            get => Notifier.GetOrAdd(nameof(AuthAccessToken), nameof(AuthAccessToken));
            set => Notifier.AddOrUpdate(nameof(AuthAccessToken), value);
        }

        /// <summary>
        /// 缓冲区大小（默认 512 字节）。
        /// </summary>
        public int BufferSize
        {
            get => Notifier.GetOrAdd(nameof(BufferSize), 512);
            set => Notifier.AddOrUpdate(nameof(BufferSize), value);
        }

        /// <summary>
        /// 超时（默认 5 秒）。
        /// </summary>
        public TimeSpan Timeout
        {
            get => Notifier.GetOrAdd(nameof(Timeout), TimeSpan.FromSeconds(5));
            set => Notifier.AddOrUpdate(nameof(Timeout), value);
        }

        /// <summary>
        /// 浏览器代理。
        /// </summary>
        public string UserAgent
        {
            get => Notifier.GetOrAdd(nameof(UserAgent), "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36 Edg/92.0.902.67");
            set => Notifier.AddOrUpdate(nameof(UserAgent), value);
        }

        /// <summary>
        /// 文件提供程序列表。
        /// </summary>
        public List<IStorageFileProvider> FileProviders { get; init; }
            = new List<IStorageFileProvider>();

    }
}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.JsonConverters;
using Librame.Extensions.Microparts;

namespace Librame.Extensions.Core.Storage;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的 Web 文件选项。
/// </summary>
public class WebFileOptions : IOptions
{
    /// <summary>
    /// HTTP 客户端选项。
    /// </summary>
    public HttpClientOptions HttpClient { get; set; } = new();


    /// <summary>
    /// 认证用户。
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// 认证密码。
    /// </summary>
    [JsonConverter(typeof(JsonStringEncryptionConverter))]
    public string? Password { get; set; }

    /// <summary>
    /// 认证 JWT 令牌。
    /// </summary>
    public string? JwtToken { get; set; }

    /// <summary>
    /// 认证访问令牌。
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// 认证 Cookie 名称。
    /// </summary>
    public string? CookieName { get; set; }

    /// <summary>
    /// 缓冲区大小（默认 512 字节）。
    /// </summary>
    public int BufferSize { get; set; } = 512;

    /// <summary>
    /// 文件提供程序列表。
    /// </summary>
    [JsonIgnore]
    public List<IStorableFileProvider> FileProviders { get; set; } = new();
}

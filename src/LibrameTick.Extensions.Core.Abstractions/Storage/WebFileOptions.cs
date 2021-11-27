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

namespace Librame.Extensions.Core.Storage;

/// <summary>
/// 定义实现 <see cref="IOptionsNotifier"/> 的 Web 文件选项。
/// </summary>
public class WebFileOptions : AbstractOptionsNotifier
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="WebFileOptions"/>（此构造函数适用于独立使用 <see cref="WebFileOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public WebFileOptions(string sourceAliase)
        : base(sourceAliase)
    {
        HttpClientInvoking = new(Notifier);
    }

    /// <summary>
    /// 使用给定的 <see cref="IPropertyNotifier"/> 构造一个 <see cref="WebFileOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public WebFileOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
        HttpClientInvoking = new(Notifier);
    }


    /// <summary>
    /// HTTP 客户端调用选项。
    /// </summary>
    public HttpClientInvokingOptions HttpClientInvoking { get; init; }


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
    /// 文件提供程序列表。
    /// </summary>
    [JsonIgnore]
    public List<IStorableFileProvider> FileProviders { get; init; } = new();
}

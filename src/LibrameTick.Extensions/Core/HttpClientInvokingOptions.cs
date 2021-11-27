#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义实现 <see cref="IOptionsNotifier"/> 的 HTTP 客户端调用选项。
/// </summary>
public class HttpClientInvokingOptions : AbstractOptionsNotifier
{
    /// <summary>
    /// 构造一个默认 <see cref="HttpClientInvokingOptions"/>。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名。</param>
    public HttpClientInvokingOptions(string sourceAliase)
        : base(sourceAliase)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="HttpClientInvokingOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public HttpClientInvokingOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
    }


    /// <summary>
    /// 使用 HTTP 消息处理程序调用（默认为 <see cref="HttpClientHandler"/> 并启用 <see cref="HttpClientHandler.UseCookies"/>）。
    /// </summary>
    [JsonIgnore]
    public HttpMessageHandler? MessageHandler { get; set; }
        = new HttpClientHandler { UseCookies = true };

    /// <summary>
    /// 通过工厂模式调用 HTTP 客户端的逻辑名称（默认为 <see cref="string.Empty"/> 表示使用默认构造函数新建实例）。
    /// </summary>
    public string ClientName
    {
        get => Notifier.GetOrAdd(nameof(ClientName), string.Empty);
        set => Notifier.AddOrUpdate(nameof(ClientName), value);
    }

    /// <summary>
    /// 请求超时（支持 <see cref="TimeSpan"/> 格式字符串，默认 30 秒。参数格式可参见“https://docs.microsoft.com/zh-cn/dotnet/api/system.timespan.parse?view=net-6.0”）。
    /// </summary>
    public string Timeout
    {
        get => Notifier.GetOrAdd(nameof(Timeout), "00:00:30");
        set => Notifier.AddOrUpdate(nameof(Timeout), value);
    }

    /// <summary>
    /// 浏览器代理。
    /// </summary>
    public string UserAgent
    {
        get => Notifier.GetOrAdd(nameof(UserAgent), "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/95.0.4638.69 Safari/537.36 Edg/95.0.1020.53");
        set => Notifier.AddOrUpdate(nameof(UserAgent), value);
    }

}

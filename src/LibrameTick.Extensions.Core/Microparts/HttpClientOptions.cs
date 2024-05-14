#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Microparts;

///// <summary>
///// 定义实现 <see cref="IOptions"/> 的 <see cref="HttpClient"/> 选项。
///// </summary>
//public class HttpClientOptions : IOptions
//{
//    /// <summary>
//    /// 使用 HTTP 消息处理程序调用（默认为 <see cref="HttpClientHandler"/> 并启用 <see cref="HttpClientHandler.UseCookies"/>）。
//    /// </summary>
//    [JsonIgnore]
//    public HttpMessageHandler? MessageHandler { get; set; }
//        = new HttpClientHandler { UseCookies = true };

//    /// <summary>
//    /// 通过工厂模式调用 HTTP 客户端的逻辑名称（默认为 <see cref="string.Empty"/> 表示使用默认构造函数新建实例）。
//    /// </summary>
//    public string? ClientName { get; set; }

//    /// <summary>
//    /// 请求超时（支持 <see cref="TimeSpan"/> 格式字符串，默认 10 秒。参数格式可参见“https://docs.microsoft.com/zh-cn/dotnet/api/system.timespan.parse?view=net-6.0”）。
//    /// </summary>
//    public TimeSpan Timeout { get; set; }
//        = TimeSpan.FromSeconds(10);

//    /// <summary>
//    /// 浏览器代理。
//    /// </summary>
//    public string? UserAgent { get; set; }
//        = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.102 Safari/537.36 Edg/98.0.1108.56";

//}

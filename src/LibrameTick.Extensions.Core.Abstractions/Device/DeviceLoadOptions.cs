#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Microparts;

namespace Librame.Extensions.Device;

/// <summary>
/// 定义一个实现 <see cref="IOptions"/> 的设备负载选项。
/// </summary>
public class DeviceLoadOptions : IOptions
{
    /// <summary>
    /// 本机监控选项。
    /// </summary>
    public DeviceMonitoringOptions Monitoring { get; set; } = new();

    /// <summary>
    /// HTTP 客户端选项。
    /// </summary>
    public HttpClientOptions HttpClient { get; set; } = new();


    /// <summary>
    /// 本机（默认为 Localhost）。
    /// </summary>
    public string Localhost { get; set; } = nameof(Localhost);

    /// <summary>
    /// 要获取设备负载的主机集合（默认获取本机，支持 URL 链接）。
    /// </summary>
    public List<string> Hosts => new List<string> { Localhost };

    /// <summary>
    /// 获取的时间间隔，即采集频率，需大于监控的最大采集时间间隔（默认为 3 秒）。
    /// </summary>
    public TimeSpan Interval { get; set; }
        = TimeSpan.FromSeconds(3);

    /// <summary>
    /// 首次执行完成延迟。
    /// </summary>
    public long FirstCompletedDelayTicks { get; set; }

    /// <summary>
    /// 首次执行完成延迟超时，超过此设置则不继续等待，直接返回默认，防止无限等待（默认 3 倍时间间隔）。
    /// </summary>
    public long FirstCompletionDelayTimeoutTicks
        => Interval.Ticks * 3;

    /// <summary>
    /// 间隔经过的动作。
    /// </summary>
    [JsonIgnore]
    public Action<IDeviceLoader>? ElapsingAction { get; set; }

    /// <summary>
    /// 获取主机利用率的动作。
    /// </summary>
    [JsonIgnore]
    public Action<IDeviceLoader, string>? GettingUsageAction { get; set; }

    /// <summary>
    /// 成功获取主机利用率的动作。
    /// </summary>
    [JsonIgnore]
    public Action<IDeviceLoader, DeviceUsageDescriptor>? GotUsageAction { get; set; }
}

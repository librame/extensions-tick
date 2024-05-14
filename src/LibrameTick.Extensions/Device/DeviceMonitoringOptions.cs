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
using Librame.Extensions.Infrastructure.Dependency;

namespace Librame.Extensions.Device;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的设备监视选项。
/// </summary>
public class DeviceMonitoringOptions : IOptions
{
    /// <summary>
    /// 提升准确性的处理器重复采集次数（默认 3 次）。
    /// </summary>
    public int ProcessorCollectCount { get; set; } = 3;

    /// <summary>
    /// 处理器单次采集间隔（默认 300 毫秒）。
    /// </summary>
    public TimeSpan ProcessorCollectInterval { get; set; }
        = TimeSpan.FromMilliseconds(300);

    /// <summary>
    /// 提升准确性的网卡重复采集次数（网络设备太多，默认 1 次）。
    /// </summary>
    public int NetworkCollectCount { get; set; } = 1;

    /// <summary>
    /// 网卡单次采集间隔（默认 300 毫秒）。
    /// </summary>
    public TimeSpan NetworkCollectInterval { get; set; }
        = TimeSpan.FromMilliseconds(300);

    /// <summary>
    /// 创建时间方法（默认使用本地当前时间）。
    /// </summary>
    public Func<DateTimeOffset> CreateTimeFunc { get; set; }
        = DependencyRegistration.CurrentContext.Clocks.GetNow;

    /// <summary>
    /// 含有网络接口方法（默认不包含 VMware 虚拟网卡）。
    /// </summary>
    [JsonIgnore]
    public Func<NetworkInterface, bool>? HasInterfaceFunc { get; set; }
        = static face => face.NetworkInterfaceType == NetworkInterfaceType.Ethernet
            && !face.Name.StartsWith("VMware");


    /// <summary>
    /// 获取最大采集间隔。
    /// </summary>
    /// <returns>返回 <see cref="TimeSpan"/>。</returns>
    public TimeSpan GetMaxInterval()
        => new(ProcessorCollectCount * ProcessorCollectInterval.Ticks
            + NetworkCollectCount * NetworkCollectInterval.Ticks);

}

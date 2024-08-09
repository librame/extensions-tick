#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Device;

/// <summary>
/// 定义一个本地设备信息。
/// </summary>
public sealed class LocalDeviceInfo : StaticDefaultInitializer<LocalDeviceInfo>
{
    /// <summary>
    /// 获取或设置处理器信息。
    /// </summary>
    /// <value>
    /// 返回 <see cref="ProcessorDeviceInfo"/>。
    /// </value>
    public ProcessorDeviceInfo Processor { get; set; } = ProcessorDeviceInfo.Default;

    /// <summary>
    /// 获取或设置内存信息。
    /// </summary>
    /// <value>
    /// 返回 <see cref="MemoryDeviceInfo"/>。
    /// </value>
    public MemoryDeviceInfo Memory { get; set; } = MemoryDeviceInfo.Default;

    /// <summary>
    /// 获取或设置复合磁盘信息。
    /// </summary>
    /// <value>
    /// 返回 <see cref="CompositeDiskDeviceInfo"/>。
    /// </value>
    public CompositeDiskDeviceInfo Disks { get; set; } = CompositeDiskDeviceInfo.Default;

    /// <summary>
    /// 获取或设置复合网络信息。
    /// </summary>
    /// <value>
    /// 返回 <see cref="CompositeNetworkDeviceInfo"/>。
    /// </value>
    public CompositeNetworkDeviceInfo Networks { get; set; } = CompositeNetworkDeviceInfo.Default;
}

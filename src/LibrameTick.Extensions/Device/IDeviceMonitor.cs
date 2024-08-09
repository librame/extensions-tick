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
/// 定义一个实现 <see cref="IDisposable"/> 的设备监视器接口。
/// </summary>
public interface IDeviceMonitor : IDisposable
{
    /// <summary>
    /// 设备监视器选项。
    /// </summary>
    DeviceMonitoringOptions Options { get; }


    /// <summary>
    /// 发送 PING。
    /// </summary>
    /// <param name="hostNameOrAddress">给定的主机名或 IP 地址。</param>
    /// <param name="timeout">给定的超时毫秒数（可选）。</param>
    /// <returns>返回 <see cref="PingReply"/>。</returns>
    PingReply SendPing(string hostNameOrAddress, int? timeout = null);

    /// <summary>
    /// 发送 PING。
    /// </summary>
    /// <param name="hostNameOrAddress">给定的主机名或 IP 地址。</param>
    /// <param name="timeout">给定的超时毫秒数（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="PingReply"/> 的异步操作。</returns>
    Task<PingReply> SendPingAsync(string hostNameOrAddress, int? timeout = null,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// 获取支持的所有设备信息。
    /// </summary>
    /// <returns>返回 <see cref="LocalDeviceInfo"/>。</returns>
    LocalDeviceInfo GetAll();

    /// <summary>
    /// 异步获取支持的所有设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="LocalDeviceInfo"/> 的异步操作。</returns>
    Task<LocalDeviceInfo> GetAllAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 获取处理器设备信息。
    /// </summary>
    /// <returns>返回 <see cref="IProcessorDeviceInfo"/>。</returns>
    ProcessorDeviceInfo GetProcessor();

    /// <summary>
    /// 异步获取处理器设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IProcessorDeviceInfo"/> 的异步操作。</returns>
    Task<ProcessorDeviceInfo> GetProcessorAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 获取内存设备信息。
    /// </summary>
    /// <returns>返回 <see cref="IMemoryDeviceInfo"/>。</returns>
    MemoryDeviceInfo GetMemory();

    /// <summary>
    /// 异步获取内存设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IMemoryDeviceInfo"/> 的异步操作。</returns>
    Task<MemoryDeviceInfo> GetMemoryAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 获取复合磁盘设备信息。
    /// </summary>
    /// <returns>返回 <see cref="CompositeDiskDeviceInfo"/>。</returns>
    CompositeDiskDeviceInfo GetDisks();

    /// <summary>
    /// 异步获取复合磁盘设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="CompositeDiskDeviceInfo"/> 的异步操作。</returns>
    Task<CompositeDiskDeviceInfo> GetDisksAsync(CancellationToken cancellationToken = default);


    /// <summary>
    /// 获取复合网络设备信息。
    /// </summary>
    /// <returns>返回 <see cref="CompositeNetworkDeviceInfo"/>。</returns>
    CompositeNetworkDeviceInfo GetNetworks();

    /// <summary>
    /// 异步获取复合网络设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="CompositeNetworkDeviceInfo"/> 的异步操作。</returns>
    Task<CompositeNetworkDeviceInfo> GetNetworksAsync(CancellationToken cancellationToken = default);
}

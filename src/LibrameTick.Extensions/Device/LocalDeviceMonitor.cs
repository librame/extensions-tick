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

namespace Librame.Extensions.Device;

/// <summary>
/// 定义实现 <see cref="IDeviceMonitor"/> 的本机设备监视器。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="LocalDeviceMonitor"/>。
/// </remarks>
/// <param name="options">给定的 <see cref="DeviceMonitoringOptions"/>。</param>
public sealed class LocalDeviceMonitor(DeviceMonitoringOptions options) : AbstractDisposable, IDeviceMonitor
{
    private Ping? _ping;


    /// <summary>
    /// 设备监视器选项。
    /// </summary>
    public DeviceMonitoringOptions Options { get; init; } = options;


    /// <summary>
    /// 发送 PING。
    /// </summary>
    /// <param name="hostNameOrAddress">给定的主机名或 IP 地址。</param>
    /// <param name="timeout">给定的超时毫秒数（可选）。</param>
    /// <returns>返回 <see cref="PingReply"/>。</returns>
    public PingReply SendPing(string hostNameOrAddress, int? timeout = null)
    {
        _ping ??= new Ping();

        return timeout is null
            ? _ping.Send(hostNameOrAddress)
            : _ping.Send(hostNameOrAddress, timeout.Value);
    }

    /// <summary>
    /// 发送 PING。
    /// </summary>
    /// <param name="hostNameOrAddress">给定的主机名或 IP 地址。</param>
    /// <param name="timeout">给定的超时毫秒数（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="PingReply"/> 的异步操作。</returns>
    public Task<PingReply> SendPingAsync(string hostNameOrAddress, int? timeout = null,
        CancellationToken cancellationToken = default)
    {
        _ping ??= new Ping();

        return timeout is null
            ? _ping.SendPingAsync(hostNameOrAddress)
            : _ping.SendPingAsync(hostNameOrAddress, timeout.Value);
    }


    /// <summary>
    /// 获取支持的所有设备信息。
    /// </summary>
    /// <returns>返回 <see cref="LocalDeviceInfo"/>。</returns>
    public LocalDeviceInfo GetAll()
    {
        return new()
        {
            Processor = GetProcessor(),
            Memory = GetMemory(),
            Disks = GetDisks(),
            Networks = GetNetworks()
        };
    }

    /// <summary>
    /// 异步获取支持的所有设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="LocalDeviceInfo"/> 的异步操作。</returns>
    public async Task<LocalDeviceInfo> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var processor = await GetProcessorAsync(cancellationToken).ConfigureAwait(false);
        var memory = await GetMemoryAsync(cancellationToken).ConfigureAwait(false);
        var disks = await GetDisksAsync(cancellationToken).ConfigureAwait(false);
        var networks = await GetNetworksAsync(cancellationToken).ConfigureAwait(false);

        return new()
        {
            Processor = processor,
            Memory = memory,
            Disks = disks,
            Networks = networks
        };
    }


    /// <summary>
    /// 获取处理器设备信息。
    /// </summary>
    /// <returns>返回 <see cref="ProcessorDeviceInfo"/>。</returns>
    public ProcessorDeviceInfo GetProcessor()
        => ProcessorDevice.GetInfo(Options.ProcessorCollectCount, Options.ProcessorCollectInterval);

    /// <summary>
    /// 异步获取处理器设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="ProcessorDeviceInfo"/> 的异步操作。</returns>
    public Task<ProcessorDeviceInfo> GetProcessorAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetProcessor);


    /// <summary>
    /// 获取内存设备信息。
    /// </summary>
    /// <returns>返回 <see cref="MemoryDeviceInfo"/>。</returns>
    public MemoryDeviceInfo GetMemory()
        => MemoryDevice.GetInfo();

    /// <summary>
    /// 异步获取内存设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="MemoryDeviceInfo"/> 的异步操作。</returns>
    public Task<MemoryDeviceInfo> GetMemoryAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetMemory);


    /// <summary>
    /// 获取复合磁盘设备信息。
    /// </summary>
    /// <returns>返回 <see cref="CompositeDiskDeviceInfo"/>。</returns>
    public CompositeDiskDeviceInfo GetDisks()
        => DiskDevice.GetInfos();

    /// <summary>
    /// 异步获取复合磁盘设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="CompositeDiskDeviceInfo"/> 的异步操作。</returns>
    public Task<CompositeDiskDeviceInfo> GetDisksAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetDisks);


    /// <summary>
    /// 获取复合网络设备信息。
    /// </summary>
    /// <returns>返回 <see cref="CompositeNetworkDeviceInfo"/>。</returns>
    public CompositeNetworkDeviceInfo GetNetworks()
        => NetworkDevice.GetInfos(Options);

    /// <summary>
    /// 异步获取复合网络设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="CompositeNetworkDeviceInfo"/> 的异步操作。</returns>
    public Task<CompositeNetworkDeviceInfo> GetNetworksAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetNetworks);


    /// <summary>
    /// 释放托管对象。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    protected override bool ReleaseManaged()
    {
        _ping?.Dispose();

        return true;
    }

}

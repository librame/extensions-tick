#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Device;

/// <summary>
/// 定义实现 <see cref="IDeviceMonitor"/> 的本机设备监视器。
/// </summary>
public class LocalDeviceMonitor : AbstractDisposable, IDeviceMonitor
{
    private Ping? _ping;


    /// <summary>
    /// 构造一个 <see cref="LocalDeviceMonitor"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DeviceMonitoringOptions"/>。</param>
    public LocalDeviceMonitor(DeviceMonitoringOptions options)
    {
        Options = options;
    }


    /// <summary>
    /// 设备监视器选项。
    /// </summary>
    public DeviceMonitoringOptions Options { get; init; }


    /// <summary>
    /// 发送 PING。
    /// </summary>
    /// <param name="hostNameOrAddress">给定的主机名或 IP 地址。</param>
    /// <param name="timeout">给定的超时毫秒数（可选）。</param>
    /// <returns>返回 <see cref="PingReply"/>。</returns>
    public PingReply SendPing(string hostNameOrAddress, int? timeout = null)
    {
        if (_ping is null)
            _ping = new Ping();

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
        if (_ping is null)
            _ping = new Ping();

        return timeout is null
            ? _ping.SendPingAsync(hostNameOrAddress)
            : _ping.SendPingAsync(hostNameOrAddress, timeout.Value);
    }


    /// <summary>
    /// 获取处理器设备信息。
    /// </summary>
    /// <returns>返回 <see cref="IProcessorDeviceInfo"/>。</returns>
    public IProcessorDeviceInfo GetProcessor()
        => ProcessorDevice.GetInfo(Options.ProcessorCollectCount, Options.ProcessorCollectInterval);

    /// <summary>
    /// 异步获取处理器设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IProcessorDeviceInfo"/> 的异步操作。</returns>
    public Task<IProcessorDeviceInfo> GetProcessorAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetProcessor);


    /// <summary>
    /// 获取网络设备信息。
    /// </summary>
    /// <returns>返回 <see cref="INetworkDeviceInfo"/>。</returns>
    public INetworkDeviceInfo GetNetwork()
        => NetworkDevice.GetInfo(Options.NetworkCollectCount, Options.NetworkCollectInterval, Options.HasInterfaceFunc);

    /// <summary>
    /// 异步获取网络设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="INetworkDeviceInfo"/> 的异步操作。</returns>
    public Task<INetworkDeviceInfo> GetNetworkAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetNetwork);


    /// <summary>
    /// 获取内存设备信息。
    /// </summary>
    /// <returns>返回 <see cref="IMemoryDeviceInfo"/>。</returns>
    public IMemoryDeviceInfo GetMemory()
        => MemoryDevice.GetInfo();

    /// <summary>
    /// 异步获取内存设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IMemoryDeviceInfo"/> 的异步操作。</returns>
    public Task<IMemoryDeviceInfo> GetMemoryAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetMemory);


    /// <summary>
    /// 获取磁盘设备信息。
    /// </summary>
    /// <returns>返回 <see cref="IDiskDeviceInfo"/>。</returns>
    public IDiskDeviceInfo GetDisk()
        => DiskDevice.GetInfo();

    /// <summary>
    /// 异步获取磁盘设备信息。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IDiskDeviceInfo"/> 的异步操作。</returns>
    public Task<IDiskDeviceInfo> GetDiskAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(GetDisk);


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

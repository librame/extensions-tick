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
/// 定义一个实现 <see cref="INetworkDeviceInfo"/> 的网络设备信息。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/whuanle/CZGL.SystemInfo.git"/>
/// </remarks>
public readonly struct NetworkDeviceInfo : INetworkDeviceInfo
{
    private readonly NetworkInterface _info;


    /// <summary>
    /// 构造一个 <see cref="NetworkDeviceInfo"/>。
    /// </summary>
    /// <param name="info">给定的 <see cref="NetworkInterface"/>。</param>
    /// <param name="traffic">给定的 <see cref="NetworkTraffic"/>。</param>
    public NetworkDeviceInfo(NetworkInterface info, NetworkTraffic traffic)
    {
        _info = info;
        Traffic = traffic;
    }


    /// <summary>
    /// 网络流量。
    /// </summary>
    public NetworkTraffic Traffic { get; init; }

    /// <summary>
    /// 标识符。
    /// </summary>
    /// <remarks>ex：{92D3E528-5363-43C7-82E8-D143DC6617ED}</remarks>
    public string Id => _info.Id;

    /// <summary>
    /// 名称。
    /// </summary>
    /// <remarks>ex：以太网，WLAN</remarks>
    public string Name => _info.Name;

    /// <summary>
    /// MAC 地址。
    /// </summary>
    /// <remarks>ex：1C997AF108E3</remarks>
    public string Mac => _info.GetPhysicalAddress().ToString();

    /// <summary>
    /// 描述（在 Windows 上，它通常描述接口供应商、类型 (例如，以太网) 、品牌和型号）。
    /// </summary>
    /// <remarks>ex：Realtek PCIe GbE Family Controller、Realtek 8822CE Wireless LAN 802.11ac PCI-E NIC</remarks>
    public string Description => _info.Description;

    /// <summary>
    /// 操作状态。
    /// </summary>
    public OperationalStatus Status => _info.OperationalStatus;

    /// <summary>
    /// 接口类型。
    /// </summary>
    public NetworkInterfaceType NetworkType => _info.NetworkInterfaceType;

    /// <summary>
    /// 链接速度（每字节/秒为单位）。
    /// </summary>
    /// <remarks>如果是-1，则说明无法获取此网卡的链接速度；例如 270_000_000 表示是 270MB 的链接速度</remarks>
    public long Speed => _info.Speed;

    /// <summary>
    /// 是否支持 IPv4。
    /// </summary>
    public bool IsSupportIPv4 => _info.Supports(NetworkInterfaceComponent.IPv4);

    /// <summary>
    /// 是否支持 IPv6。
    /// </summary>
    public bool IsSupportIPv6 => _info.Supports(NetworkInterfaceComponent.IPv6);

    /// <summary>
    /// 是否能够与其他计算机通讯（公网或内网），如果任何网络接口标记为 "up" 且不是环回或隧道接口，则认为网络连接可用。
    /// </summary>
    public bool IsAvailable => NetworkInterface.GetIsNetworkAvailable();

    /// <summary>
    /// 分配的多播地址。
    /// </summary>
    /// <remarks>ex：192.168.3.38</remarks>
    public IReadOnlyCollection<IPAddress> MulticastAddresses
        => _info.GetIPProperties().MulticastAddresses.Select(static x => x.Address).AsReadOnlyCollection();

    /// <summary>
    /// 分配的单播地址。
    /// </summary>
    /// <remarks>ex：192.168.3.38</remarks>
    public IReadOnlyCollection<IPAddress> UnicastAddresses
        => _info.GetIPProperties().UnicastAddresses.Select(static x => x.Address).AsReadOnlyCollection();

    /// <summary>
    /// 网关地址（依次为IPv4、IPv6）。
    /// </summary>
    /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
    public IReadOnlyCollection<IPAddress> GatewayAddresses
        => _info.GetIPProperties().GatewayAddresses.Select(static x => x.Address).AsReadOnlyCollection();

    /// <summary>
    /// 域名系统 (DNS) 服务器地址（依次为IPv4、IPv6）。
    /// </summary>
    /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
    public IReadOnlyCollection<IPAddress> DnsAddresses
        => _info.GetIPProperties().DnsAddresses.AsReadOnlyCollection();

    /// <summary>
    /// 网络利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate
        => Traffic.CalculateUsageRate(Speed);
}

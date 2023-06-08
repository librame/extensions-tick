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
/// 定义一个实现 <see cref="INetworkDeviceInfo"/> 的复合网络设备信息。
/// </summary>
public readonly struct CompositeNetworkDeviceInfo : INetworkDeviceInfo, IComposable<INetworkDeviceInfo>
{
    private readonly IEnumerable<INetworkDeviceInfo> _infos;
    private readonly NetworkInterfaceType[] _networkTypes;
    private readonly OperationalStatus[] _operationalStatuses;


    /// <summary>
    /// 构造一个 <see cref="CompositeDiskDeviceInfo"/>。
    /// </summary>
    /// <param name="infos">给定的 <see cref="IEnumerable{INetworkDeviceInfo}"/>。</param>
    public CompositeNetworkDeviceInfo(IEnumerable<INetworkDeviceInfo> infos)
    {
        _infos = infos;

        _networkTypes = _infos.Select(static s => s.NetworkType).Distinct().ToArray();
        _operationalStatuses = _infos.Select(static s => s.Status).Distinct().ToArray();

        Traffic = new(infos.Select(static s => s.Traffic.CreateTime).Max(),
            infos.Select(static s => s.Traffic.ReceivedLength).Sum(),
            infos.Select(static s => s.Traffic.SendLength).Sum(),
            infos.Select(static s => s.Traffic.ReceivedRate).Average(),
            infos.Select(static s => s.Traffic.SendRate).Average());

        UsageRate = Traffic.CalculateUsageRate(Speed);
    }


    /// <summary>
    /// 网络流量。
    /// </summary>
    public NetworkTraffic Traffic { get; init; }

    /// <summary>
    /// 标识符。
    /// </summary>
    /// <remarks>ex：{92D3E528-5363-43C7-82E8-D143DC6617ED}</remarks>
    public string Id => string.Join(',', _infos.Select(static s => s.Id));

    /// <summary>
    /// 名称。
    /// </summary>
    /// <remarks>ex：以太网，WLAN</remarks>
    public string Name => string.Join(',', _infos.Select(static s => s.Name));

    /// <summary>
    /// MAC 地址。
    /// </summary>
    /// <remarks>ex：1C997AF108E3</remarks>
    public string Mac => string.Join(',', _infos.Select(static s => s.Mac));

    /// <summary>
    /// 描述（在 Windows 上，它通常描述接口供应商、类型 (例如，以太网) 、品牌和型号）。
    /// </summary>
    /// <remarks>ex：Realtek PCIe GbE Family Controller、Realtek 8822CE Wireless LAN 802.11ac PCI-E NIC</remarks>
    public string Description => string.Join(',', _infos.Select(static s => s.Description));

    /// <summary>
    /// 操作状态。
    /// </summary>
    public OperationalStatus Status
        => _operationalStatuses.Length == 1 ? _operationalStatuses.First() : OperationalStatus.Unknown;

    /// <summary>
    /// 接口类型。
    /// </summary>
    public NetworkInterfaceType NetworkType
        => _networkTypes.Length == 1 ? _networkTypes.First() : NetworkInterfaceType.Unknown;

    /// <summary>
    /// 链接速度（每字节/秒为单位）。
    /// </summary>
    /// <remarks>如果是-1，则说明无法获取此网卡的链接速度；例如 270_000_000 表示是 270MB 的链接速度</remarks>
    public long Speed => (long)_infos.Select(static s => s.Speed).Average();

    /// <summary>
    /// 是否支持 IPv4。
    /// </summary>
    public bool IsSupportIPv4 => _infos.Any(static p => p.IsSupportIPv4);

    /// <summary>
    /// 是否支持 IPv6。
    /// </summary>
    public bool IsSupportIPv6 => _infos.Any(static p => p.IsSupportIPv6);

    /// <summary>
    /// 是否能够与其他计算机通讯（公网或内网），如果任何网络接口标记为 "up" 且不是环回或隧道接口，则认为网络连接可用。
    /// </summary>
    public bool IsAvailable => _infos.Any(static p => p.IsAvailable);

    /// <summary>
    /// 分配的多播地址。
    /// </summary>
    /// <remarks>ex：192.168.3.38</remarks>
    public IReadOnlyCollection<IPAddress> MulticastAddresses
        => _infos.SelectMany(static s => s.MulticastAddresses).Distinct().AsReadOnlyCollection();

    /// <summary>
    /// 分配的单播地址。
    /// </summary>
    /// <remarks>ex：192.168.3.38</remarks>
    public IReadOnlyCollection<IPAddress> UnicastAddresses
        => _infos.SelectMany(static s => s.UnicastAddresses).Distinct().AsReadOnlyCollection();

    /// <summary>
    /// 网关地址（依次为IPv4、IPv6）。
    /// </summary>
    /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
    public IReadOnlyCollection<IPAddress> GatewayAddresses
        => _infos.SelectMany(static s => s.GatewayAddresses).Distinct().AsReadOnlyCollection();

    /// <summary>
    /// 域名系统 (DNS) 服务器地址（依次为IPv4、IPv6）。
    /// </summary>
    /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
    public IReadOnlyCollection<IPAddress> DnsAddresses
        => _infos.SelectMany(static s => s.DnsAddresses).Distinct().AsReadOnlyCollection();

    /// <summary>
    /// 网络利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; init; }


    /// <summary>
    /// 复合的网络设备数。
    /// </summary>
    public int Count => _infos.Count();


    /// <summary>
    /// 获取复合的网络设备枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{INetworkDeviceInfo}"/>。</returns>
    public IEnumerator<INetworkDeviceInfo> GetEnumerator()
        => _infos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}

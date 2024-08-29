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
using Librame.Extensions.Serialization;

namespace Librame.Extensions.Device;

/// <summary>
/// 定义一个实现 <see cref="INetworkDeviceInfo"/> 的复合网络设备信息。
/// </summary>
public sealed class CompositeNetworkDeviceInfo : INetworkDeviceInfo, IComposable<INetworkDeviceInfo>
{
    /// <summary>
    /// 网络设备信息数组。
    /// </summary>
    /// <value>
    /// 返回 <see cref="INetworkDeviceInfo"/> 数组。
    /// </value>
    [BinaryMapping]
    public List<NetworkDeviceInfo> Infos { get; set; } = [];

    /// <summary>
    /// 网络接口类型数组。
    /// </summary>
    /// <value>
    /// 返回 <see cref="NetworkInterfaceType"/> 数组。
    /// </value>
    public List<NetworkInterfaceType> InterfaceTypes { get; set; } = [];

    /// <summary>
    /// 网络操作状态数组。
    /// </summary>
    /// <value>
    /// 返回 <see cref="OperationalStatus"/> 数组。
    /// </value>
    public List<OperationalStatus> OperationalStatuses { get; set; } = [];

    /// <summary>
    /// 复合的网络设备数量。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int Count { get; set; }


    /// <summary>
    /// 网络流量。
    /// </summary>
    public NetworkTraffic Traffic { get; set; } = new();

    /// <summary>
    /// 标识符。
    /// </summary>
    /// <remarks>ex：{92D3E528-5363-43C7-82E8-D143DC6617ED}</remarks>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// 名称。
    /// </summary>
    /// <remarks>ex：以太网，WLAN</remarks>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// MAC 地址。
    /// </summary>
    /// <remarks>ex：1C997AF108E3</remarks>
    public string Mac { get; set; } = string.Empty;

    /// <summary>
    /// 描述（在 Windows 上，它通常描述接口供应商、类型 (例如，以太网) 、品牌和型号）。
    /// </summary>
    /// <remarks>ex：Realtek PCIe GbE Family Controller、Realtek 8822CE Wireless LAN 802.11ac PCI-E NIC</remarks>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 操作状态。
    /// </summary>
    public OperationalStatus Status { get; set; } = OperationalStatus.Unknown;

    /// <summary>
    /// 接口类型。
    /// </summary>
    public NetworkInterfaceType InterfaceType { get; set; } = NetworkInterfaceType.Unknown;

    /// <summary>
    /// 链接速度（每字节/秒为单位）。
    /// </summary>
    /// <remarks>如果是-1，则说明无法获取此网卡的链接速度；例如 270_000_000 表示是 270MB 的链接速度</remarks>
    public long Speed { get; set; }

    /// <summary>
    /// 是否支持 IPv4。
    /// </summary>
    public bool IsSupportIPv4 { get; set; }

    /// <summary>
    /// 是否支持 IPv6。
    /// </summary>
    public bool IsSupportIPv6 { get; set; }

    /// <summary>
    /// 是否能够与其他计算机通讯（公网或内网），如果任何网络接口标记为 "up" 且不是环回或隧道接口，则认为网络连接可用。
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// 分配的多播地址。
    /// </summary>
    /// <remarks>ex：192.168.3.38</remarks>
    public List<IPAddress> MulticastAddresses { get; set; } = [];

    /// <summary>
    /// 分配的单播地址。
    /// </summary>
    /// <remarks>ex：192.168.3.38</remarks>
    public List<IPAddress> UnicastAddresses { get; set; } = [];

    /// <summary>
    /// 网关地址（依次为IPv4、IPv6）。
    /// </summary>
    /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
    public List<IPAddress> GatewayAddresses { get; set; } = [];

    /// <summary>
    /// 域名系统 (DNS) 服务器地址（依次为IPv4、IPv6）。
    /// </summary>
    /// <remarks>ex：fe80::1677:40ff:fef9:bf95%5、192.168.3.1</remarks>
    public List<IPAddress> DnsAddresses { get; set; } = [];

    /// <summary>
    /// 网络利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; set; }


    /// <summary>
    /// 获取复合的网络设备枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{INetworkDeviceInfo}"/>。</returns>
    public IEnumerator<INetworkDeviceInfo> GetEnumerator()
        => Infos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    /// <summary>
    /// 创建复合网络设备信息。
    /// </summary>
    /// <param name="infos">给定的 <see cref="INetworkDeviceInfo"/> 数组。</param>
    /// <returns>返回 <see cref="CompositeNetworkDeviceInfo"/>。</returns>
    public static CompositeNetworkDeviceInfo Create(params NetworkDeviceInfo[] infos)
        => Create((IEnumerable<NetworkDeviceInfo>)infos);

    /// <summary>
    /// 创建复合网络设备信息。
    /// </summary>
    /// <param name="infos">给定的 <see cref="IEnumerable{INetworkDeviceInfo}"/>。</param>
    /// <returns>返回 <see cref="CompositeNetworkDeviceInfo"/>。</returns>
    public static CompositeNetworkDeviceInfo Create(IEnumerable<NetworkDeviceInfo> infos)
    {
        var interfaceTypes = infos.Select(static s => s.InterfaceType).Distinct().ToList();
        var operationalStatuses = infos.Select(static s => s.Status).Distinct().ToList();

        var traffic = NetworkTraffic.Create(infos.Select(static s => s.Traffic!.CreateTime).Max(),
            infos.Select(static s => s.Traffic!.ReceivedLength).Sum(),
            infos.Select(static s => s.Traffic!.SendLength).Sum(),
            infos.Select(static s => s.Traffic!.ReceivedRate).Average(),
            infos.Select(static s => s.Traffic!.SendRate).Average());

        var speed = (long)infos.Select(static s => s.Speed).Average();

        return new()
        {
            Infos = infos.ToList(),
            InterfaceTypes = interfaceTypes,
            OperationalStatuses = operationalStatuses,
            Count = infos.Count(),
            Traffic = traffic,
            Id = string.Join(',', infos.Select(static s => s.Id)),
            Name = string.Join(',', infos.Select(static s => s.Name)),
            Mac = string.Join(',', infos.Select(static s => s.Mac)),
            Description = string.Join(',', infos.Select(static s => s.Description)),
            Status = operationalStatuses.Count == 1 ? operationalStatuses.First() : OperationalStatus.Unknown,
            InterfaceType = interfaceTypes.Count == 1 ? interfaceTypes.First() : NetworkInterfaceType.Unknown,
            Speed = speed,
            IsSupportIPv4 = infos.Any(static p => p.IsSupportIPv4),
            IsSupportIPv6 = infos.Any(static p => p.IsSupportIPv6),
            IsAvailable = infos.Any(static p => p.IsAvailable),
            MulticastAddresses = infos.SelectMany(static s => s.MulticastAddresses).Distinct().ToList(),
            UnicastAddresses = infos.SelectMany(static s => s.UnicastAddresses).Distinct().ToList(),
            GatewayAddresses = infos.SelectMany(static s => s.GatewayAddresses).Distinct().ToList(),
            DnsAddresses = infos.SelectMany(static s => s.DnsAddresses).Distinct().ToList(),
            UsageRate = traffic.CalcUsageRate(speed)
        };
    }

}

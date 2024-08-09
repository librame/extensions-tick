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
/// 定义一个实现 <see cref="IDeviceUsage{Single}"/> 的网络设备信息接口。
/// </summary>
public interface INetworkDeviceInfo : IDeviceUsage<float>
{
    /// <summary>
    /// 网络流量。
    /// </summary>
    NetworkTraffic Traffic { get; }

    /// <summary>
    /// 标识符。
    /// </summary>
    string Id { get; }

    /// <summary>
    /// 名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    /// MAC 地址。
    /// </summary>
    string Mac { get; }

    /// <summary>
    /// 描述。
    /// </summary>
    string Description { get; }

    /// <summary>
    /// 操作状态。
    /// </summary>
    OperationalStatus Status { get; }

    /// <summary>
    /// 接口类型。
    /// </summary>
    NetworkInterfaceType InterfaceType { get; }

    /// <summary>
    /// 链接速度（每字节/秒为单位）。
    /// </summary>
    long Speed { get; }

    /// <summary>
    /// 是否支持 IPv4。
    /// </summary>
    bool IsSupportIPv4 { get; }

    /// <summary>
    /// 是否支持 IPv6。
    /// </summary>
    bool IsSupportIPv6 { get; }

    /// <summary>
    /// 是否能够与其他计算机通讯（公网或内网）。
    /// </summary>
    bool IsAvailable { get; }

    /// <summary>
    /// 分配的多播地址。
    /// </summary>
    List<IPAddress> MulticastAddresses { get; }

    /// <summary>
    /// 分配的单播地址。
    /// </summary>
    List<IPAddress> UnicastAddresses { get; }

    /// <summary>
    /// 网关地址。
    /// </summary>
    List<IPAddress> GatewayAddresses { get; }

    /// <summary>
    /// 域名系统 (DNS) 服务器地址。
    /// </summary>
    List<IPAddress> DnsAddresses { get; }
}

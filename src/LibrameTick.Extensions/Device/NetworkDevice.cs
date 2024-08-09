﻿#region License

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
/// 定义网络设备。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/whuanle/CZGL.SystemInfo.git"/>
/// </remarks>
public static class NetworkDevice
{
    private readonly static Func<IPAddress, bool> _isValidAddressFunc = p
        => p.AddressFamily == AddressFamily.InterNetwork
        || p.AddressFamily == AddressFamily.InterNetworkV6;

    private static IEnumerable<NetworkInterface>? _hostInterfaces = null;


    /// <summary>
    /// 获取复合网络设备信息。
    /// </summary>
    /// <param name="options">给定的 <see cref="DeviceMonitoringOptions"/>。</param>
    /// <returns>返回 <see cref="CompositeNetworkDeviceInfo"/>。</returns>
    public static CompositeNetworkDeviceInfo GetInfos(DeviceMonitoringOptions options)
        => GetInfos(options.NetworkCollectCount, options.NetworkCollectInterval, options.CreateTimeFunc,
            options.HasInterfaceFunc);

    /// <summary>
    /// 获取复合网络设备信息。
    /// </summary>
    /// <param name="collectCount">给定用于提升准确性的重复采集次数。</param>
    /// <param name="collectInterval">给定的单次采集间隔。</param>
    /// <param name="createTimeFunc">给定的产生时间方法。</param>
    /// <param name="hasInterfaceFunc">给定含有网络接口的方法。</param>
    /// <returns>返回 <see cref="CompositeNetworkDeviceInfo"/>。</returns>
    public static CompositeNetworkDeviceInfo GetInfos(int collectCount, TimeSpan collectInterval,
        Func<DateTimeOffset> createTimeFunc, Func<NetworkInterface, bool>? hasInterfaceFunc)
    {
        if (collectCount < 1)
            collectCount = 1; // 至少重复采集一次（除开首次）

        if (collectInterval == TimeSpan.Zero)
            collectInterval = TimeSpan.FromMilliseconds(300);

        _hostInterfaces ??= GetHostInterfaces(hasInterfaceFunc);

        var infos = GetInfos(createTimeFunc, collectCount, collectInterval).ToArray();

        return CompositeNetworkDeviceInfo.Create(infos);
    }

    private static IEnumerable<NetworkDeviceInfo> GetInfos(Func<DateTimeOffset> createTimeFunc,
        int collectCount, TimeSpan collectInterval)
    {
        var lastTraffics = _hostInterfaces!.Select(s => GetTraffic(createTimeFunc, s)).ToArray();

        // 需要多次计算以提高网络利用率准确性
        var receivedRates = new Dictionary<string, float>(lastTraffics.Length);
        var sendRates = new Dictionary<string, float>(lastTraffics.Length);

        for (var i = 0; i < collectCount; i++)
        {
            Thread.Sleep(collectInterval);

            for (var j = 0; j < lastTraffics.Length; j++)
            {
                var info = _hostInterfaces!.ElementAt(j);
                var curTraffic = GetTraffic(createTimeFunc, info, lastTraffics[j]);

                if (!receivedRates.ContainsKey(info.Id))
                    receivedRates.Add(info.Id, curTraffic.ReceivedRate);
                else
                    receivedRates[info.Id] += curTraffic.ReceivedRate;

                if (!sendRates.ContainsKey(info.Id))
                    sendRates.Add(info.Id, curTraffic.SendRate);
                else
                    sendRates[info.Id] += curTraffic.SendRate;

                lastTraffics[j] = curTraffic;
            }
        }

        for (var i = 0; i < lastTraffics.Length; i++)
        {
            var info = _hostInterfaces!.ElementAt(i);

            // 计算多次平均利用率
            var avgReceivedRate = receivedRates[info.Id] / collectCount;
            var avgSendRate = sendRates[info.Id] / collectCount;

            var realTraffic = lastTraffics[i].WithRate(avgReceivedRate, avgSendRate);

            yield return NetworkDeviceInfo.Create(info, realTraffic);
        }
    }

    private static NetworkTraffic GetTraffic(Func<DateTimeOffset> createTimeFunc, NetworkInterface info,
        NetworkTraffic? lastTraffic = null)
    {
        var statistics = info.GetIPStatistics();

        return lastTraffic is null
            ? NetworkTraffic.Create(createTimeFunc(), statistics.BytesReceived, statistics.BytesSent)
            : NetworkTraffic.Create(createTimeFunc(), statistics.BytesReceived, statistics.BytesSent, lastTraffic);
    }

    private static IEnumerable<NetworkInterface> GetHostInterfaces(Func<NetworkInterface, bool>? hasInterfaceFunc)
    {
        var hostAddresses = Dns.GetHostAddresses(Dns.GetHostName())
            .Where(_isValidAddressFunc)
            .ToArray();

        var interfaces = NetworkInterface.GetAllNetworkInterfaces();
        if (hasInterfaceFunc is not null)
        {
            interfaces = interfaces.Where(hasInterfaceFunc).ToArray();
        }

        foreach (var item in interfaces)
        {
            var itemAddresses = item.GetIPProperties().UnicastAddresses.Select(s => s.Address);
            if (itemAddresses.Intersect(hostAddresses).Any())
            {
                yield return item;
            }
        }
    }

}

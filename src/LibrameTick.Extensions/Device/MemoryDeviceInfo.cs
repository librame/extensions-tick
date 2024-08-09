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
/// 定义一个实现 <see cref="IMemoryDeviceInfo"/> 的内存设备信息。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/whuanle/CZGL.SystemInfo.git"/>
/// </remarks>
public sealed class MemoryDeviceInfo : StaticDefaultInitializer<MemoryDeviceInfo>, IMemoryDeviceInfo
{
    /// <summary>
    /// 总物理内存字节数。
    /// </summary>
    public ulong TotalPhysicalMemory { get; set; }

    /// <summary>
    /// 可用物理内存字节数。
    /// </summary>
    public ulong AvailablePhysicalMemory { get; set; }

    /// <summary>
    /// 已用物理内存字节数。
    /// </summary>
    public ulong UsedPhysicalMemory { get; set; }

    /// <summary>
    /// 总虚拟内存字节数。
    /// </summary>
    public ulong TotalVirtualMemory { get; set; }

    /// <summary>
    /// 可用虚拟内存字节数。
    /// </summary>
    public ulong AvailableVirtualMemory { get; set; }

    /// <summary>
    /// 已用虚拟内存字节数。
    /// </summary>
    public ulong UsedVirtualMemory { get; set; }

    /// <summary>
    /// 物理内存利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; set; }


    /// <summary>
    /// 创建内存设备信息。
    /// </summary>
    /// <param name="sysinfo">给定的 <see cref="DllInterop.LinuxSysinfo"/>。</param>
    /// <returns>返回 <see cref="MemoryDeviceInfo"/>。</returns>
    internal static MemoryDeviceInfo Create(DllInterop.LinuxSysinfo sysinfo)
    {
        return new()
        {
            TotalPhysicalMemory = sysinfo.totalram,
            AvailablePhysicalMemory = sysinfo.freeram,
            UsedPhysicalMemory = sysinfo.totalram - sysinfo.freeram,
            TotalVirtualMemory = sysinfo.totalswap,
            AvailableVirtualMemory = sysinfo.freeswap,
            UsedVirtualMemory = sysinfo.totalswap - sysinfo.freeswap,
            UsageRate = ((float)sysinfo.totalram - sysinfo.freeram) / sysinfo.totalram * 100
        };
    }

    /// <summary>
    /// 创建内存设备信息。
    /// </summary>
    /// <param name="status">给定的 <see cref="DllInterop.MemoryStatusExE"/>。</param>
    /// <returns>返回 <see cref="MemoryDeviceInfo"/>。</returns>
    internal static MemoryDeviceInfo Create(DllInterop.MemoryStatusExE status)
    {
        return new()
        {
            TotalPhysicalMemory = status.ullTotalPhys,
            AvailablePhysicalMemory = status.ullAvailPhys,
            UsedPhysicalMemory = status.ullTotalPhys - status.ullAvailPhys,
            TotalVirtualMemory = status.ullTotalVirtual,
            AvailableVirtualMemory = status.ullAvailVirtual,
            UsedVirtualMemory = status.ullTotalVirtual - status.ullAvailVirtual,
            UsageRate = status.dwMemoryLoad
        };
    }

}

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
/// 定义一个实现 <see cref="IMemoryDeviceInfo"/> 的内存设备信息结构体。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/whuanle/CZGL.SystemInfo.git"/>
/// </remarks>
public readonly struct MemoryDeviceInfo : IMemoryDeviceInfo
{
    /// <summary>
    /// 使用 <see cref="DllInterop.MemoryStatusExE"/> 构造一个 <see cref="MemoryDeviceInfo"/>。
    /// </summary>
    /// <param name="status">给定的 <see cref="DllInterop.MemoryStatusExE"/>。</param>
    internal MemoryDeviceInfo(DllInterop.MemoryStatusExE status)
        : this(status.ullTotalPhys, status.ullAvailPhys,
                status.dwMemoryLoad, status.ullTotalVirtual, status.ullAvailVirtual)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="MemoryDeviceInfo"/>。
    /// </summary>
    /// <param name="totalPhysicalMemory">给定的总物理内存字节数。</param>
    /// <param name="availablePhysicalMemory">给定的可用物理内存字节数。</param>
    /// <param name="usageRate">给定的物理内存利用率。</param>
    /// <param name="totalVirtualMemory">给定的总虚拟内存字节数。</param>
    /// <param name="availableVirtualMemory">给定的可用虚拟内存字节数。</param>
    public MemoryDeviceInfo(ulong totalPhysicalMemory, ulong availablePhysicalMemory,
        float usageRate, ulong totalVirtualMemory, ulong availableVirtualMemory)
    {
        TotalPhysicalMemory = totalPhysicalMemory;
        AvailablePhysicalMemory = availablePhysicalMemory;
        UsageRate = usageRate;
        TotalVirtualMemory = totalVirtualMemory;
        AvailableVirtualMemory = availableVirtualMemory;
    }


    /// <summary>
    /// 总物理内存字节数。
    /// </summary>
    public ulong TotalPhysicalMemory { get; init; }

    /// <summary>
    /// 可用物理内存字节数。
    /// </summary>
    public ulong AvailablePhysicalMemory { get; init; }

    /// <summary>
    /// 已用物理内存字节数。
    /// </summary>
    public ulong UsedPhysicalMemory
        => TotalPhysicalMemory - AvailablePhysicalMemory;

    /// <summary>
    /// 总虚拟内存字节数。
    /// </summary>
    public ulong TotalVirtualMemory { get; init; }

    /// <summary>
    /// 可用虚拟内存字节数。
    /// </summary>
    public ulong AvailableVirtualMemory { get; init; }

    /// <summary>
    /// 已用虚拟内存字节数。
    /// </summary>
    public ulong UsedVirtualMemory
        => TotalVirtualMemory - AvailableVirtualMemory;

    /// <summary>
    /// 物理内存利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; init; }

}

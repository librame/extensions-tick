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
/// 定义一个实现 <see cref="IDeviceUsage{Single}"/> 的内存设备信息接口。
/// </summary>
public interface IMemoryDeviceInfo : IDeviceUsage<float>
{
    /// <summary>
    /// 总物理内存字节数。
    /// </summary>
    ulong TotalPhysicalMemory { get; }

    /// <summary>
    /// 可用物理内存字节数。
    /// </summary>
    ulong AvailablePhysicalMemory { get; }

    /// <summary>
    /// 已用物理内存字节数。
    /// </summary>
    ulong UsedPhysicalMemory { get; }

    /// <summary>
    /// 总虚拟内存字节数。
    /// </summary>
    ulong TotalVirtualMemory { get; }

    /// <summary>
    /// 可用虚拟内存字节数。
    /// </summary>
    ulong AvailableVirtualMemory { get; }

    /// <summary>
    /// 已用虚拟内存字节数。
    /// </summary>
    ulong UsedVirtualMemory { get; }
}

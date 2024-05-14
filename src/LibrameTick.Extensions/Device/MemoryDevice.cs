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
/// 定义内存设备。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/whuanle/CZGL.SystemInfo.git"/>
/// </remarks>
public static class MemoryDevice
{

    /// <summary>
    /// 获取内存设备信息。
    /// </summary>
    /// <returns>返回 <see cref="IMemoryDeviceInfo"/>。</returns>
    public static IMemoryDeviceInfo GetInfo()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxInfo();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return GetWindowsInfo();

        return default(MemoryDeviceInfo);
    }

    private static MemoryDeviceInfo GetLinuxInfo()
    {
        var info = new DllInterop.LinuxSysinfo();

        if (DllInterop.sysinfo(ref info) != 0)
            return default;

        var usedPercentage = ((float)info.totalram - info.freeram) / info.totalram * 100;

        return new MemoryDeviceInfo(info.totalram, info.freeram, usedPercentage, info.totalswap, info.freeswap);
    }

    private static MemoryDeviceInfo GetWindowsInfo()
    {
        // 检查 Windows 内核版本，是否为旧系统（https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions）
        if (Environment.OSVersion.Version.Major < 5)
            return default;

        var status = new DllInterop.MemoryStatusExE();
        status.Init();

        if (!DllInterop.GlobalMemoryStatusEx(ref status))
            return default;

        return new MemoryDeviceInfo(status);
    }

}

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
    /// <returns>返回 <see cref="MemoryDeviceInfo"/>。</returns>
    /// <exception cref="NotSupportedException">
    /// 不支持的操作系统的异常。
    /// </exception>
    public static MemoryDeviceInfo GetInfo()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxInfo();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return GetWindowsInfo();

        throw new NotSupportedException("Unsupported OS.");
    }

    private static MemoryDeviceInfo GetLinuxInfo()
    {
        var sysinfo = new DllInterop.LinuxSysinfo();
        if (DllInterop.sysinfo(ref sysinfo) != 0)
        {
            throw new Exception("Failed to get system information.");
        }

        return MemoryDeviceInfo.Create(sysinfo);
    }

    private static MemoryDeviceInfo GetWindowsInfo()
    {
        // 检查 Windows 内核版本，是否为旧系统（https://en.wikipedia.org/wiki/List_of_Microsoft_Windows_versions）
        if (Environment.OSVersion.Version.Major < 5)
        {
            throw new NotSupportedException("Unsupported Windows OS kernel version.");
        }

        var status = new DllInterop.MemoryStatusExE();
        status.Init();

        if (!DllInterop.GlobalMemoryStatusEx(ref status))
        {
            throw new Exception("Failed to get system information.");
        }

        return MemoryDeviceInfo.Create(status);
    }

}

﻿#region License

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
/// 定义磁盘设备。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/whuanle/CZGL.SystemInfo.git"/>
/// </remarks>
public static class DiskDevice
{
    private readonly static Func<DriveInfo, bool> _isValidDiskFunc = p => p.DriveType == DriveType.Fixed
        && p.TotalSize > 0
        && p.DriveFormat != "overlay";


    /// <summary>
    /// 获取复合磁盘设备信息。
    /// </summary>
    /// <returns>返回 <see cref="CompositeDiskDeviceInfo"/>。</returns>
    public static CompositeDiskDeviceInfo GetInfos()
    {
        var infos = DriveInfo.GetDrives()
            .Where(_isValidDiskFunc)
            .Select(static s => DiskDeviceInfo.Create(s))
            .DistinctBy(static ks => ks.Name)
            .ToArray();

        return CompositeDiskDeviceInfo.Create(infos);
    }

}

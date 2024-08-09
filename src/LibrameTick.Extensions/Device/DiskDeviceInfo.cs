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
/// 定义一个实现 <see cref="IDiskDeviceInfo"/> 的磁盘设备信息。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/whuanle/CZGL.SystemInfo.git"/>
/// </remarks>
/// <remarks>
/// 构造一个 <see cref="DiskDeviceInfo"/>。
/// </remarks>
public sealed class DiskDeviceInfo : StaticDefaultInitializer<DiskDeviceInfo>, IDiskDeviceInfo
{
    /// <summary>
    /// 磁盘名称。
    /// </summary>
    /// <remarks>
    /// Windows: C:\<br />
    /// Linux: /dev
    /// </remarks>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 磁盘根目录。
    /// </summary>
    public string RootPath { get; set; } = string.Empty;

    /// <summary>
    /// 驱动器类型。
    /// </summary>
    /// <remarks>获取驱动器类型，如 CD-ROM、可移动、网络或固定。</remarks>
    public DriveType DriveType { get; set; } = DriveType.Unknown;

    /// <summary>
    /// 文件系统。
    /// </summary>
    /// <remarks>
    /// ex:<br />
    /// Windows: NTFS、CDFS...<br />
    /// Linux: rootfs、tmpfs、binfmt_misc...
    /// </remarks>
    public string FileSystem { get; set; } = string.Empty;

    /// <summary>
    /// 磁盘剩余容量（以字节为单位）。
    /// </summary>
    public long FreeSpace { get; set; }

    /// <summary>
    /// 磁盘总容量（以字节为单位）。
    /// </summary>
    public long TotalSize { get; set; }

    /// <summary>
    /// 磁盘已用容量（以字节为单位）。
    /// </summary>
    public long UsedSize { get; set; }

    /// <summary>
    /// 磁盘利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; set; }


    /// <summary>
    /// 创建磁盘设备信息。
    /// </summary>
    /// <param name="info">给定的 <see cref="DriveInfo"/>。</param>
    /// <returns>返回 <see cref="DiskDeviceInfo"/>。</returns>
    public static DiskDeviceInfo Create(DriveInfo info)
    {
        var usageRate = info.TotalSize == 0
            ? 0f
            : (float)(info.TotalSize - info.AvailableFreeSpace) / info.TotalSize * 100;

        return new()
        {
            Name = info.Name,
            RootPath = info.RootDirectory.FullName,
            DriveType = info.DriveType,
            FileSystem = info.DriveFormat,
            FreeSpace = info.AvailableFreeSpace,
            TotalSize = info.TotalSize,
            UsedSize = info.TotalSize - info.AvailableFreeSpace,
            UsageRate = usageRate
        };
    }

}

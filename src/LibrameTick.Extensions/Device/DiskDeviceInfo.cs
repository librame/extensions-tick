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
public readonly struct DiskDeviceInfo : IDiskDeviceInfo
{
    private readonly DriveInfo _info;


    /// <summary>
    /// 构造一个 <see cref="DiskDeviceInfo"/>。
    /// </summary>
    /// <param name="info">给定的 <see cref="DriveInfo"/>。</param>
    public DiskDeviceInfo(DriveInfo info)
    {
        _info = info;
    }


    /// <summary>
    /// 磁盘名称。
    /// </summary>
    /// <remarks>
    /// ex:<br />
    /// Windows: C:\<br />
    /// Linux: /dev
    /// </remarks>
    public string Name => _info.Name;

    /// <summary>
    /// 磁盘根目录。
    /// </summary>
    public string RootPath => _info.RootDirectory.FullName;

    /// <summary>
    /// 驱动器类型。
    /// </summary>
    /// <remarks>获取驱动器类型，如 CD-ROM、可移动、网络或固定。</remarks>
    public DriveType DriveType => _info.DriveType;

    /// <summary>
    /// 文件系统。
    /// </summary>
    /// <remarks>
    /// ex:<br />
    /// Windows: NTFS、CDFS...<br />
    /// Linux: rootfs、tmpfs、binfmt_misc...
    /// </remarks>
    public string FileSystem => _info.DriveFormat;

    /// <summary>
    /// 磁盘剩余容量（以字节为单位）。
    /// </summary>
    public long FreeSpace => _info.AvailableFreeSpace;

    /// <summary>
    /// 磁盘总容量（以字节为单位）。
    /// </summary>
    public long TotalSize => _info.TotalSize;

    /// <summary>
    /// 磁盘已用容量（以字节为单位）。
    /// </summary>
    public long UsedSize => TotalSize - FreeSpace;

    /// <summary>
    /// 磁盘利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate => TotalSize == 0 ? 0f : (float)UsedSize / TotalSize * 100;
}

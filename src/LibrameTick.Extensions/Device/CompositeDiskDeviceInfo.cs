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
/// 定义一个实现 <see cref="IDiskDeviceInfo"/> 的复合磁盘设备信息。
/// </summary>
public readonly struct CompositeDiskDeviceInfo : IDiskDeviceInfo, IComposable<IDiskDeviceInfo>
{
    private readonly IEnumerable<IDiskDeviceInfo> _infos;
    private readonly DriveType[] _driveTypes;


    /// <summary>
    /// 构造一个 <see cref="CompositeDiskDeviceInfo"/>。
    /// </summary>
    /// <param name="infos">给定的 <see cref="IEnumerable{IDiskDeviceInfo}"/>。</param>
    public CompositeDiskDeviceInfo(IEnumerable<IDiskDeviceInfo> infos)
    {
        _infos = infos;
        _driveTypes = _infos.Select(s => s.DriveType).Distinct().ToArray();
    }


    /// <summary>
    /// 磁盘名称。
    /// </summary>
    public string Name => string.Join(',', _infos.Select(s => s.Name));

    /// <summary>
    /// 磁盘根目录。
    /// </summary>
    public string RootPath => string.Join(',', _infos.Select(s => s.RootPath));

    /// <summary>
    /// 驱动器类型（多个驱动器类型相同则返回同类型，反之返回未知）。
    /// </summary>
    public DriveType DriveType => _driveTypes.Length == 1 ? _driveTypes.First() : DriveType.Unknown;

    /// <summary>
    /// 文件系统。
    /// </summary>
    public string FileSystem => string.Join(',', _infos.Select(s => s.FileSystem));

    /// <summary>
    /// 磁盘剩余容量（以字节为单位）。
    /// </summary>
    public long FreeSpace => _infos.Select(s => s.FreeSpace).Sum();

    /// <summary>
    /// 磁盘总容量（以字节为单位）。
    /// </summary>
    public long TotalSize => _infos.Select(s => s.TotalSize).Sum();

    /// <summary>
    /// 磁盘已用容量（以字节为单位）。
    /// </summary>
    public long UsedSize => TotalSize - FreeSpace;

    /// <summary>
    /// 磁盘利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate => TotalSize == 0 ? 0f : (float)UsedSize / TotalSize * 100;


    /// <summary>
    /// 复合的磁盘设备数。
    /// </summary>
    public int Count => _infos.Count();


    /// <summary>
    /// 获取复合的磁盘设备枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{IDiskDeviceInfo}"/>。</returns>
    public IEnumerator<IDiskDeviceInfo> GetEnumerator()
        => _infos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}

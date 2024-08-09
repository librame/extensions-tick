#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Serialization;

namespace Librame.Extensions.Device;

/// <summary>
/// 定义一个实现 <see cref="IDiskDeviceInfo"/> 的复合磁盘设备信息。
/// </summary>
public sealed class CompositeDiskDeviceInfo : StaticDefaultInitializer<CompositeDiskDeviceInfo>,
    IDiskDeviceInfo, IComposable<IDiskDeviceInfo>
{
    /// <summary>
    /// 磁盘设备信息数组。
    /// </summary>
    /// <value>
    /// 返回 <see cref="List{DiskDeviceInfo}"/>。
    /// </value>
    [BinaryExpressionMapping]
    public List<DiskDeviceInfo> Infos { get; set; } = [];

    /// <summary>
    /// 磁盘类型数组。
    /// </summary>
    /// <value>
    /// 返回 <see cref="List{DriveType}"/>。
    /// </value>
    public List<DriveType> DriveTypes { get; set; } = [];

    /// <summary>
    /// 复合磁盘设备数量。
    /// </summary>
    /// <value>
    /// 返回整数。
    /// </value>
    public int Count { get; set; }


    /// <summary>
    /// 磁盘名称。
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 磁盘根目录。
    /// </summary>
    public string RootPath { get; set; } = string.Empty;

    /// <summary>
    /// 驱动器类型（多个驱动器类型相同则返回同类型，反之返回未知）。
    /// </summary>
    public DriveType DriveType { get; set; } = DriveType.Unknown;

    /// <summary>
    /// 文件系统。
    /// </summary>
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
    /// 获取复合的磁盘设备枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{IDiskDeviceInfo}"/>。</returns>
    public IEnumerator<IDiskDeviceInfo> GetEnumerator()
        => Infos.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    /// <summary>
    /// 创建复合磁盘设备信息。
    /// </summary>
    /// <param name="infos">给定的 <see cref="IDiskDeviceInfo"/> 数组。</param>
    /// <returns>返回 <see cref="CompositeDiskDeviceInfo"/>。</returns>
    public static CompositeDiskDeviceInfo Create(params DiskDeviceInfo[] infos)
        => Create((IEnumerable<DiskDeviceInfo>)infos);

    /// <summary>
    /// 创建复合磁盘设备信息。
    /// </summary>
    /// <param name="infos">给定的 <see cref="IEnumerable{DiskDeviceInfo}"/>。</param>
    /// <returns>返回 <see cref="CompositeDiskDeviceInfo"/>。</returns>
    public static CompositeDiskDeviceInfo Create(IEnumerable<DiskDeviceInfo> infos)
    {
        var driveTypes = infos.Select(static s => s.DriveType).Distinct().ToList();
        var freeSpaces = infos.Select(static s => s.FreeSpace).Sum();
        var totalSizes = infos.Select(static s => s.TotalSize).Sum();

        return new()
        {
            Infos = infos.ToList(),
            DriveTypes = driveTypes,
            Count = infos.Count(),

            // Composite properties
            Name = string.Join(',', infos.Select(static s => s.Name)),
            RootPath = string.Join(',', infos.Select(static s => s.RootPath)),
            DriveType = driveTypes.Count == 1 ? driveTypes.First() : DriveType.Unknown,
            FileSystem = string.Join(',', infos.Select(static s => s.FileSystem)),
            FreeSpace = freeSpaces,
            TotalSize = totalSizes,
            UsedSize = totalSizes - freeSpaces,
            UsageRate = totalSizes == 0 ? 0f : (float)(totalSizes - freeSpaces) / totalSizes * 100
        };
    }

}

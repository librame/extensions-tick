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
/// 定义一个实现 <see cref="IDeviceUsage{Single}"/> 的磁盘设备信息接口。
/// </summary>
public interface IDiskDeviceInfo : IDeviceUsage<float>
{
    /// <summary>
    /// 磁盘名称。
    /// </summary>
    string Name { get; }

    /// <summary>
    /// 磁盘根目录。
    /// </summary>
    string RootPath { get; }

    /// <summary>
    /// 驱动器类型。
    /// </summary>
    DriveType DriveType { get; }

    /// <summary>
    /// 文件系统。
    /// </summary>
    string FileSystem { get; }

    /// <summary>
    /// 磁盘剩余容量（以字节为单位）。
    /// </summary>
    long FreeSpace { get; }

    /// <summary>
    /// 磁盘总容量（以字节为单位）。
    /// </summary>
    long TotalSize { get; }

    /// <summary>
    /// 磁盘已用容量（以字节为单位）。
    /// </summary>
    long UsedSize { get; }
}

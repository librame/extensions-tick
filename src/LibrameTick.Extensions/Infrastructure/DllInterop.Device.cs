#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义动态链接库互操作静态类。
/// </summary>
public static partial class DllInterop
{

    /// <summary>
    /// 检索系统定时信息（支持多处理器）。
    /// </summary>
    /// <remarks>
    /// 参考：<see href="https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getsystemtimes"/>
    /// </remarks>
    /// <param name="idleTime">指向 FILETIME 结构的指针，该结构接收系统空闲的时间量。</param>
    /// <param name="kernelTime">指向 FILETIME 结构的指针，该结构接收系统在内核模式下执行的时间量（包括所有处理器上的所有进程的线程）；此时间值还包括系统空闲的时间。</param>
    /// <param name="userTime">指向 FILETIME 结构的指针，该结构接收系统在 User 模式下执行的时间量（包括所有处理器上的所有进程的线程）。</param>
    /// <returns>返回是否成功的布尔值。</returns>
    [LibraryImport(Kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GetSystemTimes(out FileTime idleTime, out FileTime kernelTime, out FileTime userTime);


    /// <summary>
    /// 检索有关系统当前使用物理和虚拟内存的信息。
    /// </summary>
    /// <remarks>
    /// 参考：<see href="https://docs.microsoft.com/zh-cn/windows/win32/api/sysinfoapi/nf-sysinfoapi-globalmemorystatusex"/>
    /// </remarks>
    /// <param name="buffer">给定的 <see cref="MemoryStatusExE"/>。</param>
    /// <returns>返回是否成功的布尔值。</returns>
    [LibraryImport(Kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool GlobalMemoryStatusEx(ref MemoryStatusExE buffer);


    /// <summary>
    /// 检索系统统计信息。
    /// </summary>
    /// <remarks>
    /// <code>
    /// int sysinfo(struct sysinfo *info);
    /// </code>
    /// 参考：<see href="https://linux.die.net/man/2/sysinfo"/>
    /// </remarks>
    /// <param name="info">给定的 <see cref="LinuxSysinfo"/>。</param>
    /// <returns>返回是否成功的整数值。</returns>
    [LibraryImport("libc.so.6", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.I4)]
    internal static partial int sysinfo(ref LinuxSysinfo info);


    /// <summary>
    /// Linux 内存信息。
    /// </summary>
    internal struct LinuxSysinfo
    {
        /// <summary>
        /// Seconds since boot。
        /// </summary>
        public long uptime;

        /// <summary>
        /// 获取 1，5，15 分钟内存的平均使用量，数组大小为 3。
        /// </summary>
        unsafe public fixed ulong loads[3];

        /// <summary>
        /// 总物理内存。
        /// </summary>
        public ulong totalram;

        /// <summary>
        /// 可用内存。
        /// </summary>
        public ulong freeram;

        /// <summary>
        /// 共享内存。
        /// </summary>
        public ulong sharedram;

        /// <summary>
        /// Memory used by buffers。
        /// </summary>
        public ulong bufferram;

        /// <summary>
        /// Total swap space size。
        /// </summary>
        public ulong totalswap;

        /// <summary>
        /// swap space still available。
        /// </summary>
        public ulong freeswap;

        /// <summary>
        /// Number of current processes。
        /// </summary>
        public ushort procs;

        /// <summary>
        /// Total high memory size。
        /// </summary>
        public ulong totalhigh;

        /// <summary>
        /// Available high memory size。
        /// </summary>
        public ulong freehigh;

        /// <summary>
        /// Memory unit size in bytes。
        /// </summary>
        public uint mem_unit;

        /// <summary>
        /// Padding to 64 bytes。
        /// </summary>
        unsafe public fixed byte _f[64];
    }


    /// <summary>
    /// 包含有关物理内存和虚拟内存（包括扩展内存）的当前状态的信息。该 GlobalMemoryStatusEx 在这个构造函数存储信息。
    /// </summary>
    /// <remarks>
    /// 参考：<see href="https://docs.microsoft.com/en-us/windows/win32/api/sysinfoapi/ns-sysinfoapi-memorystatusex"/>
    /// </remarks>
    internal struct MemoryStatusExE
    {
        /// <summary>
        /// 结构的大小，以字节为单位，必须在调用 GlobalMemoryStatusEx 之前设置此成员，可以用 Init 方法提前处理。
        /// </summary>
        /// <remarks>应当使用本对象提供的 Init ，而不是使用构造函数！</remarks>
        public uint dwLength;

        /// <summary>
        /// 一个介于 0 和 100 之间的数字，用于指定正在使用的物理内存的大致百分比（0 表示没有内存使用，100 表示内存已满）。
        /// </summary>
        public uint dwMemoryLoad;

        /// <summary>
        /// 实际物理内存量，以字节为单位。
        /// </summary>
        public ulong ullTotalPhys;

        /// <summary>
        /// 当前可用的物理内存量，以字节为单位。这是可以立即重用而无需先将其内容写入磁盘的物理内存量。它是备用列表、空闲列表和零列表的大小之和。
        /// </summary>
        public ulong ullAvailPhys;

        /// <summary>
        /// 系统或当前进程的当前已提交内存限制，以字节为单位，以较小者为准。要获得系统范围的承诺内存限制，请调用 GetPerformanceInfo。
        /// </summary>
        public ulong ullTotalPageFile;

        /// <summary>
        /// 当前进程可以提交的最大内存量，以字节为单位。该值等于或小于系统范围的可用提交值。要计算整个系统的可承诺值，调用 GetPerformanceInfo 核减价值 CommitTotal 从价值 CommitLimit。
        /// </summary>
        public ulong ullAvailPageFile;

        /// <summary>
        /// 调用进程的虚拟地址空间的用户模式部分的大小，以字节为单位。该值取决于进程类型、处理器类型和操作系统的配置。例如，对于 x86 处理器上的大多数 32 位进程，此值约为 2 GB，对于在启用4 GB 调整的系统上运行的具有大地址感知能力的 32 位进程约为 3 GB 。
        /// </summary>
        public ulong ullTotalVirtual;

        /// <summary>
        /// 当前在调用进程的虚拟地址空间的用户模式部分中未保留和未提交的内存量，以字节为单位。
        /// </summary>
        public ulong ullAvailVirtual;


        /// <summary>
        /// 预订的。该值始终为 0。
        /// </summary>
        public ulong ullAvailExtendedVirtual;


        /// <summary>
        /// 初始化结构大小。
        /// </summary>
        public void Init()
        {
            dwLength = checked((uint)Marshal.SizeOf(typeof(MemoryStatusExE)));
        }
    }


    /// <summary>
    /// 定义一个系统文件时间结构体。
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct FileTime
    {
        /// <summary>
        /// 低位时间部分。
        /// </summary>
        public uint LowTime;

        /// <summary>
        /// 高位时间部分。
        /// </summary>
        public uint HighTime;
    }

}

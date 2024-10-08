﻿#region License

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
public static class ProcessorDevice
{
    private static readonly string _linuxSystemInfoPath = "/proc/stat";


    /// <summary>
    /// 获取处理器设备信息。
    /// </summary>
    /// <param name="collectCount">给定用于提升准确性的重复采集次数。</param>
    /// <param name="collectInterval">给定的单次采集间隔。</param>
    /// <returns>返回 <see cref="ProcessorDeviceInfo"/>。</returns>
    public static ProcessorDeviceInfo GetInfo(int collectCount, TimeSpan collectInterval)
    {
        if (collectCount < 1)
            collectCount = 1; // 至少重复采集一次（除开首次）

        if (collectInterval == TimeSpan.Zero)
            collectInterval = TimeSpan.FromMilliseconds(300);

        // 需要多次计算以提高处理器利用率准确性
        var values = new float[collectCount];

        // 初次因缺少上次时间对比，利用率为默认值
        var lastTimes = GetMoment();

        for (var i = 0; i < collectCount; i++)
        {
            Thread.Sleep(collectInterval);

            var curTimes = GetMoment(lastTimes);
            values[i] = curTimes.UsageRate;

            lastTimes = curTimes;
        }

        // 计算多次平均利用率
        var avgUsageRate = values.Sum() / collectCount;

        var realTimes = lastTimes.WithUsageRate(avgUsageRate);

        // 逻辑处理器数
        var count = Environment.ProcessorCount;

        return ProcessorDeviceInfo.Create(realTimes, count);
    }

    private static ProcessorMoment GetMoment(ProcessorMoment? lastTimes = null)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxMoment(lastTimes);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return GetWindowsMoment(lastTimes);

        throw new NotSupportedException("Unsupported OS.");
    }

    private static ProcessorMoment GetLinuxMoment(ProcessorMoment? lastMoment)
    {
        ulong ulIdleTime = 0;
        ulong ulLoadTime = 0;

        try
        {
            var allLines = File.ReadAllLines(_linuxSystemInfoPath);

            foreach (var line in allLines)
            {
                if (!line.StartsWith("cpu"))
                    continue;

                var values = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();
                ulLoadTime += (ulong)values[1..].Select(decimal.Parse).Sum();

                ulIdleTime += ulong.Parse(values[4]);
            }
        }
        catch (Exception ex)
        {
            throw ex.WriteDebugAs<PlatformNotSupportedException>($"{RuntimeInformation.OSArchitecture} {Environment.OSVersion.Platform} {Environment.OSVersion}");
        }

        return lastMoment is null
            ? ProcessorMoment.Create(ulIdleTime, ulLoadTime)
            : ProcessorMoment.Create(ulIdleTime, ulLoadTime, lastMoment);
    }

    private static ProcessorMoment GetWindowsMoment(ProcessorMoment? lastMoment)
    {
        if (!DllInterop.GetSystemTimes(out DllInterop.FileTime idleTime,
            out DllInterop.FileTime kernelTime, out DllInterop.FileTime userTime))
        {
            throw new Exception("Failed to get system information.");
        }

        return GetWindowsMoment(idleTime, kernelTime, userTime, lastMoment);
    }

    private static ProcessorMoment GetWindowsMoment(DllInterop.FileTime idleTime, DllInterop.FileTime kernelTime,
        DllInterop.FileTime userTime, ProcessorMoment? lastMoment)
    {
        var ulIdleTime = ((ulong)idleTime.HighTime << 32) | idleTime.LowTime;
        var ulKernelTime = ((ulong)kernelTime.HighTime << 32) | kernelTime.LowTime;
        var ulUserTime = ((ulong)userTime.HighTime << 32) | userTime.LowTime;

        return lastMoment is null
            ? ProcessorMoment.Create(ulIdleTime, ulKernelTime + ulUserTime)
            : ProcessorMoment.Create(ulIdleTime, ulKernelTime + ulUserTime, lastMoment);
    }

}

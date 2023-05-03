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
/// 定义一个实现 <see cref="IDeviceUsage{Single}"/> 的处理器时间结构体。
/// </summary>
public readonly struct ProcessorTimes : IDeviceUsage<float>
{
    /// <summary>
    /// 使用上一次处理器时间构造一个 <see cref="ProcessorTimes"/>。
    /// </summary>
    /// <param name="idleTime">给定的空闲时间。</param>
    /// <param name="loadTime">给定的负载时间。</param>
    /// <param name="lastTimes">上一次 <see cref="ProcessorTimes"/>。</param>
    public ProcessorTimes(ulong idleTime, ulong loadTime, ProcessorTimes lastTimes)
    {
        IdleTime = idleTime;
        LoadTime = loadTime;
        UsageRate = CalculateUsageRate(lastTimes);
    }

    /// <summary>
    /// 构造一个 <see cref="ProcessorTimes"/>。
    /// </summary>
    /// <param name="idleTime">给定的空闲时间。</param>
    /// <param name="loadTime">给定的负载时间。</param>
    /// <param name="usageRate">给定的利用率，通常以百分比数值表示（默认为 -1）。</param>
    public ProcessorTimes(ulong idleTime, ulong loadTime, float? usageRate = null)
    {
        IdleTime = idleTime;
        LoadTime = loadTime;
        UsageRate = usageRate ?? -1;
    }


    /// <summary>
    /// 空闲时间。
    /// </summary>
    public ulong IdleTime { get; init; }

    /// <summary>
    /// 负载时间。
    /// </summary>
    public ulong LoadTime { get; init; }

    /// <summary>
    /// 利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; init; }


    private float CalculateUsageRate(ProcessorTimes lastTimes)
    {
        var totalTicks = LoadTime - lastTimes.LoadTime;
        var idleTicks = IdleTime - lastTimes.IdleTime;

        return (1.0f - (totalTicks > 0 ? ((float)idleTicks) / totalTicks : 0f)) * 100;
    }


    /// <summary>
    /// 使用新利用率创建一个实例副本。
    /// </summary>
    /// <param name="newUsageRate">给定的新利用率。</param>
    /// <returns>返回 <see cref="ProcessorTimes"/>。</returns>
    public ProcessorTimes WithUsageRate(float newUsageRate)
        => new ProcessorTimes(IdleTime, LoadTime, newUsageRate);

}

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
/// 定义一个实现 <see cref="IDeviceUsage{Single}"/> 的处理器时刻。
/// </summary>
[Serializable]
public sealed class ProcessorMoment : StaticDefaultInitializer<ProcessorMoment>, IDeviceUsage<float>
{
    /// <summary>
    /// 空闲时间。
    /// </summary>
    public ulong IdleTime { get; set; }

    /// <summary>
    /// 负载时间。
    /// </summary>
    public ulong LoadTime { get; set; }
    
    /// <summary>
    /// 利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; set; }


    /// <summary>
    /// 使用新利用率创建一个实例副本。
    /// </summary>
    /// <param name="newUsageRate">给定的新利用率。</param>
    /// <returns>返回 <see cref="ProcessorMoment"/>。</returns>
    public ProcessorMoment WithUsageRate(float newUsageRate)
        => Create(IdleTime, LoadTime, newUsageRate);


    /// <summary>
    /// 创建处理器时刻。
    /// </summary>
    /// <param name="idleTime">给定的空闲时间。</param>
    /// <param name="loadTime">给定的负载时间。</param>
    /// <param name="lastMoment">给定的上次 <see cref="ProcessorMoment"/>。</param>
    /// <returns>返回 <see cref="ProcessorMoment"/>。</returns>
    public static ProcessorMoment Create(ulong idleTime, ulong loadTime, ProcessorMoment lastMoment)
    {
        var totalTicks = loadTime - lastMoment.LoadTime;
        var idleTicks = idleTime - lastMoment.IdleTime;

        var usageRate = (1.0f - (totalTicks > 0 ? ((float)idleTicks) / totalTicks : 0f)) * 100;

        return Create(idleTime, loadTime, usageRate);
    }

    /// <summary>
    /// 创建处理器时刻。
    /// </summary>
    /// <param name="idleTime">给定的空闲时间。</param>
    /// <param name="loadTime">给定的负载时间。</param>
    /// <param name="usageRate">给定的利用率（可选；默认为 0）。</param>
    /// <returns>返回 <see cref="ProcessorMoment"/>。</returns>
    public static ProcessorMoment Create(ulong idleTime, ulong loadTime, float? usageRate = null)
    {
        return new()
        {
            IdleTime = idleTime,
            LoadTime = loadTime,
            UsageRate = usageRate ?? 0
        };
    }

}

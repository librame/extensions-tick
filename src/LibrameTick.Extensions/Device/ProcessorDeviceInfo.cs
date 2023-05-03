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
/// 定义一个实现 <see cref="IProcessorDeviceInfo"/> 的处理器设备信息结构体。
/// </summary>
public readonly struct ProcessorDeviceInfo : IProcessorDeviceInfo
{
    /// <summary>
    /// 构造一个 <see cref="ProcessorDeviceInfo"/>。
    /// </summary>
    /// <param name="times">给定的 <see cref="ProcessorTimes"/>。</param>
    /// <param name="count">给定的处理器数。</param>
    public ProcessorDeviceInfo(ProcessorTimes times, int count)
    {
        Times = times;
        UsageRate = times.UsageRate;
        Count = count;
    }


    /// <summary>
    /// 处理器时间。
    /// </summary>
    public ProcessorTimes Times { get; init; }

    /// <summary>
    /// 处理器数。
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// 处理器利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; init; }


    /// <summary>
    /// 使用新处理器时间创建一个实例副本。
    /// </summary>
    /// <param name="newTimes">给定的新 <see cref="ProcessorTimes"/>。</param>
    /// <returns>返回 <see cref="IProcessorDeviceInfo"/>。</returns>
    public IProcessorDeviceInfo WithTimes(ProcessorTimes newTimes)
        => new ProcessorDeviceInfo(newTimes, Count);

}

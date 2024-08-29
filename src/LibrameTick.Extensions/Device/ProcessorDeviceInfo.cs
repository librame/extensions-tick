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
/// 定义一个实现 <see cref="IProcessorDeviceInfo"/> 的处理器设备信息。
/// </summary>
[Serializable]
public sealed class ProcessorDeviceInfo : IProcessorDeviceInfo
{
    /// <summary>
    /// 处理器时刻。
    /// </summary>
    public ProcessorMoment Moment { get; set; } = new();

    /// <summary>
    /// 处理器数。
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// 处理器利用率（通常以百分比数值表示）。
    /// </summary>
    public float UsageRate { get; set; }


    /// <summary>
    /// 创建处理器设备信息。
    /// </summary>
    /// <param name="moment">给定的 <see cref="ProcessorMoment"/>。</param>
    /// <param name="count">给定的处理器数。</param>
    /// <returns>返回 <see cref="ProcessorDeviceInfo"/>。</returns>
    public static ProcessorDeviceInfo Create(ProcessorMoment moment, int count)
    {
        return new()
        {
            Moment = moment,
            Count = count,
            UsageRate = moment.UsageRate
        };
    }

}

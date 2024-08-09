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
/// 定义设备利用率描述符。
/// </summary>
public class DeviceUsageDescriptor
{
    /// <summary>
    /// 主机。
    /// </summary>
    public string? Host { get; set; }

    /// <summary>
    /// 处理器利用率（通常以百分比数值表示）。
    /// </summary>
    public float Processor { get; set; }

    /// <summary>
    /// 内存利用率（通常以百分比数值表示）。
    /// </summary>
    public float Memory { get; set; }

    /// <summary>
    /// 网络利用率（通常以百分比数值表示）。
    /// </summary>
    public float Network { get; set; }

    /// <summary>
    /// 磁盘利用率（通常以百分比数值表示）。
    /// </summary>
    public float Disk { get; set; }


    /// <summary>
    /// 计算主机负载（格式为排序优先级形式）。
    /// </summary>
    /// <param name="smallerIsBetter">负载越小越好（可选；默认启用）</param>
    /// <returns>返回浮点数。</returns>
    public float CalculateLoad(bool smallerIsBetter = true)
    {
        if (smallerIsBetter)
        {
            return ToPriority(Network) * 1
                + ToPriority(Processor) * 2
                + ToPriority(Memory) * 3;
        }

        return ToPriority(Network) * 3
                + ToPriority(Processor) * 2
                + ToPriority(Memory) * 1;
    }

    private static float ToPriority(float usage)
        => Convert.ToSingle(Math.Round(usage / 100, 2));

}

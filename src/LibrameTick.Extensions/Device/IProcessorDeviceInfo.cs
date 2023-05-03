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
/// 定义一个实现 <see cref="IDeviceUsage{Single}"/> 的处理器设备信息接口。
/// </summary>
public interface IProcessorDeviceInfo : IDeviceUsage<float>
{
    /// <summary>
    /// 处理器时间。
    /// </summary>
    ProcessorTimes Times { get; init; }

    /// <summary>
    /// 处理器数。
    /// </summary>
    public int Count { get; init; }


    /// <summary>
    /// 使用新处理器时间创建一个实例副本。
    /// </summary>
    /// <param name="newTimes">给定的新 <see cref="ProcessorTimes"/>。</param>
    /// <returns>返回 <see cref="IProcessorDeviceInfo"/>。</returns>
    IProcessorDeviceInfo WithTimes(ProcessorTimes newTimes);
}

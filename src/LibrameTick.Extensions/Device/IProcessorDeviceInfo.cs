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
    ProcessorMoment? Moment { get; }

    /// <summary>
    /// 处理器数。
    /// </summary>
    public int Count { get; }
}

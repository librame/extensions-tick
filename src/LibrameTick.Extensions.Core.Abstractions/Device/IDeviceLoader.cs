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
/// 定义一个实现 <see cref="IDisposable"/> 的设备负载器接口。
/// </summary>
public interface IDeviceLoader : IDisposable
{
    /// <summary>
    /// 获取设备利用率集合。
    /// </summary>
    /// <param name="realtimeForEverytime">每次需要实时计算（如果不启用，当首次等待计算后，下次先返回上次计算值，利用率将在后台计算后更新，以提升响应速度）。</param>
    /// <returns>返回 <see cref="DeviceUsageDescriptor"/> 数组。</returns>
    DeviceUsageDescriptor[] GetUsages(bool realtimeForEverytime);
}

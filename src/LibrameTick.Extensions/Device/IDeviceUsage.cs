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
/// 定义设备利用率接口。
/// </summary>
public interface IDeviceUsage<TValue>
{
    /// <summary>
    /// 设备利用率（通常以百分比数值表示）。
    /// </summary>
    TValue UsageRate { get; }
}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace Librame.Extensions.Device;

/// <summary>
/// 定义实现 <see cref="AbstractDeviceLoader"/> 的存取器设备负载器。
/// </summary>
public class AccessorDeviceLoader : AbstractDeviceLoader
{
    /// <summary>
    /// 构造一个 <see cref="AccessorDeviceLoader"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DataExtensionOptions"/>。</param>
    /// <param name="hosts">给定的主机集合。</param>
    public AccessorDeviceLoader(DataExtensionOptions options, IEnumerable<string> hosts)
        : base(ChangeHosts(options, hosts))
    {
    }


    private static DeviceLoadOptions ChangeHosts(DataExtensionOptions options, IEnumerable<string> hosts)
    {
        options.DeviceLoad.Hosts.Clear();

        options.DeviceLoad.Hosts.AddRange(hosts);

        return options.DeviceLoad;
    }

}

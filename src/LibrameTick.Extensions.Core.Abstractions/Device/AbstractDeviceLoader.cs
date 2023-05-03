#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Microparts;
using System.Diagnostics;

namespace Librame.Extensions.Device;

/// <summary>
/// 定义抽象实现 <see cref="IDeviceLoader"/> 的设备负载器。
/// </summary>
public abstract class AbstractDeviceLoader : AbstractDisposable, IDeviceLoader
{
    private Timer? _timer;

    private IDeviceMonitor? _localhost;
    private HttpClient? _client;
    private DeviceUsageDescriptor[]? _usages;
    private DateTimeOffset? _firstCompletedTime;


    /// <summary>
    /// 构造一个 <see cref="AbstractDeviceLoader"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DeviceLoadOptions"/>。</param>
    protected AbstractDeviceLoader(DeviceLoadOptions options)
    {
        Options = options;

        Initialize();
    }


    /// <summary>
    /// 负责选项。
    /// </summary>
    public DeviceLoadOptions Options { get; init; }


    private void Initialize()
    {
        // 如果主机集合包含本机，则初始化本机监控器
        if (Options.Hosts.Any(host => host == Options.Localhost))
            _localhost = new LocalDeviceMonitor(Options.Monitoring);

        // 如果主机集合包含 URL，则使用 HttpClient 远程调用
        if (Options.Hosts.Any(host => host != Options.Localhost))
            _client = new HttpClientMicropart(Options.HttpClient).Unwrap();

        // 首次初始化利用率数组实例
        if (_usages is null)
            _usages = new DeviceUsageDescriptor[Options.Hosts.Count];

        _timer = new Timer(new TimerCallback(GetUsagesAsync), this, 0,
            (long)Options.Interval.TotalMilliseconds);
    }


    /// <summary>
    /// 获取设备利用率集合。
    /// </summary>
    /// <param name="realtimeForEverytime">每次需要实时计算（如果不启用，当首次等待计算后，下次先返回上次计算值，利用率将在后台计算后更新，以提升响应速度）。</param>
    /// <returns>返回 <see cref="DeviceUsageDescriptor"/> 数组。</returns>
    public virtual DeviceUsageDescriptor[] GetUsages(bool realtimeForEverytime)
    {
        // 首次获取需同步等待延迟响应
        if (_firstCompletedTime is null)
        {
            var keepWaitingInterval = 500;

            Thread.Sleep(keepWaitingInterval);
            Options.FirstCompletedDelayTicks += keepWaitingInterval;

            // 支持限时的继续等待延迟响应
            if (Options.FirstCompletedDelayTicks < Options.FirstCompletionDelayTimeoutTicks)
                return GetUsages(realtimeForEverytime);
        }

        // 清空首次未成功执行的等待延迟
        if (_usages is null)
            Options.FirstCompletedDelayTicks = 0;

        if (realtimeForEverytime)
            _firstCompletedTime = null;

        return _usages ?? Array.Empty<DeviceUsageDescriptor>();
    }


    private async void GetUsagesAsync(object? state)
    {
        try
        {
            Options.ElapsingAction?.Invoke(this);

            for (var i = 0; i < Options.Hosts.Count; i++)
            {
                DeviceUsageDescriptor? usage;

                var host = Options.Hosts[i];

                Options.GettingUsageAction?.Invoke(this, host);

                if (host == Options.Localhost)
                    usage = await GetLocalhostUsageAsync(host);
                else
                    usage = await GetUrlUsageAsync(host);

                Options.GotUsageAction?.Invoke(this, usage);

                _usages![i] = usage;
            }

            if (_firstCompletedTime is null)
                _firstCompletedTime = DateTimeOffset.UtcNow;
        }
        catch (Exception ex)
        {
            ex.WriteDebug();
        }
    }

    private async Task<DeviceUsageDescriptor> GetLocalhostUsageAsync(string host)
    {
        var processor = await _localhost!.GetProcessorAsync();
        var memory = await _localhost.GetMemoryAsync();
        var network = await _localhost.GetNetworkAsync();
        var disk = await _localhost.GetDiskAsync();

        return new DeviceUsageDescriptor
        {
            Host = host,
            Processor = processor.UsageRate,
            Memory = memory.UsageRate,
            Network = network.UsageRate,
            Disk = disk.UsageRate
        };
    }

    private async Task<DeviceUsageDescriptor> GetUrlUsageAsync(string url)
    {
        var json = await _client!.GetStringAsync(url);

        var usage = json.FromJson<DeviceUsageDescriptor>() ?? new();
        usage.Host = url;

        return usage;
    }


    /// <summary>
    /// 释放托管对象。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    protected override bool ReleaseManaged()
    {
        _localhost?.Dispose();
        _client?.Dispose();
        _timer?.Dispose();

        return true;
    }

}

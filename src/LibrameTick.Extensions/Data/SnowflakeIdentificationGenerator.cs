﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义雪花 64 位整型标识生成器（可生成长度 18 位的长整数标识）。
/// </summary>
public class SnowflakeIdentificationGenerator : AbstractIdentificationGenerator<long>
{
    private readonly IClockBootstrap _clock;
    private readonly ILockerBootstrap _locker;
    private readonly SnowflakeIdentificationParameters _parameters;
    private readonly long _machineId;
    private readonly long _dataCenterId;

    private long _sequence;
    private long _lastTimestamp = -1L;


    /// <summary>
    /// 构造一个 <see cref="SnowflakeIdentificationGenerator"/>。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（如使用本地时钟可参考 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <param name="locker">给定的 <see cref="ILockerBootstrap"/>（如使用本地锁定器可参考 <see cref="Bootstrapper.GetLocker()"/>）。</param>
    /// <param name="options">给定的 <see cref="IdentificationGenerationOptions"/>。</param>
    /// <param name="parameters">给定的 <see cref="SnowflakeIdentificationParameters"/>（可选）。</param>
    public SnowflakeIdentificationGenerator(IClockBootstrap clock, ILockerBootstrap locker,
        IdentificationGenerationOptions options, SnowflakeIdentificationParameters? parameters = null)
    {
        _clock = clock;
        _locker = locker;
        _parameters = parameters ?? new SnowflakeIdentificationParameters();

        if (options.MachineId >= 0)
            _machineId = options.MachineId.NotGreater(_parameters.GetMaxMachineId(), nameof(options.MachineId));
        else
            _machineId = _parameters.InitialMachineId;

        if (options.DataCenterId >= 0)
            _dataCenterId = options.DataCenterId.NotGreater(_parameters.GetMaxDataCenterId(), nameof(options.DataCenterId));
        else
            _dataCenterId = _parameters.InitialDataCenterId;
    }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回长整数。</returns>
    public override long GenerateId()
    {
        var timestamp = GetTimestamp();

        if (_lastTimestamp == timestamp)
        {
            // 同一微妙中生成ID
            _sequence = _parameters.GetSequenceMask(_sequence);
            if (_sequence is 0)
            {
                timestamp = GetNextTimestamp();
            }
        }
        else
        {
            // 不同微秒生成ID
            // 计数清0
            _sequence = 0;
        }

        return GenerateCore(timestamp);
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含长整数的异步操作。</returns>
    public override async Task<long> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var timestamp = await GetTimestampAsync(cancellationToken).ConfigureAwait();

        if (_lastTimestamp == timestamp)
        {
            // 同一微妙中生成ID
            _sequence = _parameters.GetSequenceMask(_sequence);
            if (_sequence is 0)
            {
                timestamp = await GetNextTimestampAsync(cancellationToken).ConfigureAwait();
            }
        }
        else
        {
            // 不同微秒生成ID
            // 计数清0
            _sequence = 0;
        }

        return GenerateCore(timestamp);
    }

    private long GenerateCore(long timestamp)
    {
        if (timestamp < _lastTimestamp)
        {
            // 如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
            throw new Exception($"Clock moved backwards. Refusing to generate id for {_lastTimestamp - timestamp} milliseconds");
        }

        return _locker.SpinLock(() =>
        {
            var nextId = (timestamp - _parameters.Twepoch << _parameters.GetTimestampLeftShift())
                | (_dataCenterId << _parameters.GetDataCenterIdShift())
                | (_machineId << _parameters.GetMachineIdShift())
                | _sequence;

            _lastTimestamp = timestamp;

            return nextId;
        });
    }


    private long GetNextTimestamp()
    {
        var timestamp = GetTimestamp();

        while (timestamp <= _lastTimestamp)
        {
            timestamp = GetTimestamp();
        }

        return timestamp;
    }

    private async Task<long> GetNextTimestampAsync(CancellationToken cancellationToken = default)
    {
        var timestamp = await GetTimestampAsync(cancellationToken).ConfigureAwait();

        while (timestamp <= _lastTimestamp)
        {
            timestamp = await GetTimestampAsync(cancellationToken).ConfigureAwait();
        }

        return timestamp;
    }


    private long GetTimestamp()
    {
        var offsetNow = _clock.GetUtcNow();

        var offsetBaseTime = new DateTimeOffset(_parameters.Twepoch, offsetNow.Offset);
        return (long)(offsetNow - offsetBaseTime).TotalMilliseconds;
    }

    private async Task<long> GetTimestampAsync(CancellationToken cancellationToken = default)
    {
        var offsetNow = await _clock.GetUtcNowAsync(cancellationToken: cancellationToken).ConfigureAwait();

        var offsetBaseTime = new DateTimeOffset(_parameters.Twepoch, offsetNow.Offset);
        return (long)(offsetNow - offsetBaseTime).TotalMilliseconds;
    }

}

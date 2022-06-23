#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 定义雪花 64 位整型标识生成器（可生成长度 19 位的长整数标识）。
/// </summary>
public class SnowflakeIdGenerator : AbstractClockIdGenerator<long>
{
    private readonly long _machineId;
    private readonly long _dataCenterId;

    private long _sequence;
    private long _sequenceAsync;

    private long _lastTicks = -1L;
    private long _lastTicksAsync = -1L;


    /// <summary>
    /// 使用内置的 <see cref="Bootstrapper.GetClock()"/> 构造一个 <see cref="SnowflakeIdGenerator"/>。
    /// </summary>
    /// <param name="snowflakes">给定的 <see cref="SnowflakeIdOptions"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    public SnowflakeIdGenerator(SnowflakeIdOptions snowflakes, IdGenerationOptions options)
        : this(snowflakes, options, Bootstrapper.GetClock())
    {
    }

    /// <summary>
    /// 构造一个 <see cref="SnowflakeIdGenerator"/>。
    /// </summary>
    /// <param name="snowflakes">给定的 <see cref="SnowflakeIdOptions"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>。</param>
    public SnowflakeIdGenerator(SnowflakeIdOptions snowflakes, IdGenerationOptions options,
        IClockBootstrap clock)
        : base(options, clock)
    {
        Snowflakes = snowflakes;

        if (options.MachineId >= 0)
            _machineId = options.MachineId.NotGreater(Snowflakes.GetMaxMachineId());
        else
            _machineId = Snowflakes.MachineBits;

        if (options.DataCenterId >= 0)
            _dataCenterId = options.DataCenterId.NotGreater(Snowflakes.GetMaxDataCenterId());
        else
            _dataCenterId = Snowflakes.DataCenterBits;
    }


    /// <summary>
    /// 雪花标识选项。
    /// </summary>
    public SnowflakeIdOptions Snowflakes { get; init; }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回长整数。</returns>
    public override long GenerateId()
    {
        var ticks = GetNowTicks();

        if (_lastTicks == ticks)
        {
            // 同一微妙中生成ID
            _sequence = Snowflakes.GetSequenceMask(_sequence);
            if (_sequence is 0)
            {
                ticks = GetNowTicks();
            }
        }
        else
        {
            // 不同微秒生成ID，计数清0
            _sequence = 0;
        }

        if (ticks < _lastTicks)
        {
            // 如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
            throw new ArgumentException($"Clock moved backwards. Refusing to generate id for {_lastTicks - ticks} milliseconds.");
        }

        var newId = CreateId(ticks, _sequence);
        _lastTicks = ticks;

        return newId;
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含长整数的异步操作。</returns>
    public override async Task<long> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var ticksAsync = await GetNowTicksAsync(cancellationToken).DisableAwaitContext();

        if (_lastTicksAsync == ticksAsync)
        {
            // 同一微妙中生成ID
            _sequenceAsync = Snowflakes.GetSequenceMask(_sequenceAsync);
            if (_sequenceAsync is 0)
            {
                ticksAsync = await GetNowTicksAsync(cancellationToken).DisableAwaitContext();
            }
        }
        else
        {
            // 不同微秒生成ID，计数清0
            _sequenceAsync = 0;
        }

        if (ticksAsync < _lastTicksAsync)
        {
            // 如果当前时间戳比上一次生成ID时时间戳还小，抛出异常，因为不能保证现在生成的ID之前没有生成过
            throw new ArgumentException($"Clock moved backwards. Refusing to generate id for {_lastTicksAsync - ticksAsync} milliseconds.");
        }

        var newId = CreateId(ticksAsync, _sequenceAsync);
        _lastTicksAsync = ticksAsync;

        return newId;
    }


    /// <summary>
    /// 创建雪花标识。
    /// </summary>
    /// <param name="ticks">给定的时间刻度。</param>
    /// <param name="sequence">给定的记数器。</param>
    /// <returns>返回长整数。</returns>
    protected virtual long CreateId(long ticks, long sequence)
    {
        var newId = (ticks << Snowflakes.GetTicksLeftShift())
            | (_dataCenterId << Snowflakes.GetDataCenterIdShift())
            | (_machineId << Snowflakes.GetMachineIdShift())
            | sequence;

        return newId;
    }

}

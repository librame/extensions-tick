#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;
using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.IdGeneration;

/// <summary>
/// 定义雪花 64 位整型标识生成器。
/// </summary>
public class SnowflakeIdGenerator : AbstractClockIdGenerator<long>
{
    private readonly long _machineId;
    private readonly long _dataCenterId;

    private long _sequence;
    private long _sequenceAsync;

    private long _baseTicks = -1L;
    private long _lastTicks = -1L;
    private long _lastTicksAsync = -1L;


    /// <summary>
    /// 构造一个 <see cref="SnowflakeIdGenerator"/>。
    /// </summary>
    /// <param name="snowflakes">给定的 <see cref="SnowflakeIdOptions"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockDependency"/>。</param>
    public SnowflakeIdGenerator(SnowflakeIdOptions snowflakes, IdGenerationOptions options,
        IClockDependency clock)
        : base(options, clock)
    {
        _machineId = options.MachineId.NotGreater(snowflakes.MaxMachineId);
        _dataCenterId = options.DataCenterId.NotGreater(snowflakes.MaxDataCenterId);

        _baseTicks = base.GetBaseTicks();

        Snowflakes = snowflakes;
    }


    /// <summary>
    /// 雪花标识选项。
    /// </summary>
    public SnowflakeIdOptions Snowflakes { get; init; }


    /// <summary>
    /// 转换时钟周期数精度。
    /// </summary>
    /// <param name="ticks">给定的时钟周期数。</param>
    /// <returns>返回长整数。</returns>
    protected override long ConvertTicksPrecision(long ticks)
        => ticks / 1000_0; // 转为毫秒


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回长整数。</returns>
    public override long GenerateId()
    {
        var nowTicks = GetNowTicks();

        if (nowTicks < _lastTicks)
        {
            // 时钟回拨
            nowTicks = GetNowTicks(_lastTicks);
        }
        else if (nowTicks == _lastTicks)
        {
            // 对序列+1并计算该周期内产生的序列号是否已经到达上限
            _sequence = (_sequence + 1) & Snowflakes.SequenceMask;
            if (_sequence is 0)
                nowTicks = GetNowTicks(_lastTicks);
        }
        else
        {
            // 不同序列生成，序列清0
            _sequence = 0;
        }

        _lastTicks = nowTicks;
        Options.GeneratingAction?.Invoke(new(nowTicks, _baseTicks, TimePrecision.Millisecond));
        
        return CreateId(nowTicks - _baseTicks, _sequence);
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含长整数的异步操作。</returns>
    public override async ValueTask<long> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var nowTicksAsync = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);

        if (nowTicksAsync < _lastTicksAsync)
        {
            // 时钟回拨
            nowTicksAsync = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);
        }
        else if (nowTicksAsync == _lastTicksAsync)
        {
            // 对序列+1并计算该周期内产生的序列号是否已经到达上限
            _sequenceAsync = (_sequenceAsync + 1) & Snowflakes.SequenceMask;
            if (_sequenceAsync is 0)
                nowTicksAsync = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);
        }
        else
        {
            // 不同微秒生成ID，计数清0
            _sequenceAsync = 0;
        }

        _lastTicksAsync = nowTicksAsync;
        Options.GeneratingAction?.Invoke(new(nowTicksAsync, _baseTicks, TimePrecision.Millisecond));
        
        return CreateId(nowTicksAsync - _baseTicks, _sequenceAsync);
    }


    /// <summary>
    /// 创建雪花标识。
    /// </summary>
    /// <param name="deltaTicks">给定的时钟周期数变数。</param>
    /// <param name="sequence">给定的记数器。</param>
    /// <returns>返回长整数。</returns>
    protected virtual long CreateId(long deltaTicks, long sequence)
    {
        // 时钟周期数变数超出最大值
        if (deltaTicks > Snowflakes.MaxTicks)
        {
            throw new InvalidOperationException("delta ticks bits is exhausted. Refusing ID generate. Now: " + deltaTicks);
        }

        return (deltaTicks << Snowflakes.TicksLeftShift)
            | (_dataCenterId << Snowflakes.DataCenterIdLeftShift)
            | (_machineId << Snowflakes.MachineIdLeftShift)
            | sequence;
    }


    /// <summary>
    /// 获取最后一次时钟周期数。
    /// </summary>
    /// <returns>返回长整数。</returns>
    public virtual long GetLastTicks()
        => _lastTicks;

    /// <summary>
    /// 异步获取最后一次时钟周期数。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含长整数的异步操作。</returns>
    public virtual ValueTask<long> GetLastTicksAsync(CancellationToken cancellationToken = default)
        => cancellationToken.SimpleValueTaskResult(() => _lastTicksAsync);


    /// <summary>
    /// 解析时钟周期数。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="isOther">使用另外一种方法还原（可选；默认 FALSE）。</param>
    /// <returns>返回长整数。</returns>
    public virtual long ParseTicks(long id, bool isOther = false)
        => Snowflakes.ParseTicks(id, _baseTicks, isOther);

}

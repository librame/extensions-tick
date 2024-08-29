﻿#region License

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
/// 定义 COMB <see cref="Guid"/> 格式的雪花标识生成器（包含 64 位完整的时间标记，32 位工作标识，32 位序列）。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="CombSnowflakeIdGenerator"/>。
/// </remarks>
/// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
/// <param name="clock">给定的 <see cref="IClockDependency"/>。</param>
public class CombSnowflakeIdGenerator(IdGenerationOptions options, IClockDependency clock)
    : ClockIdGenerator<Guid>(options, clock)
{
    private readonly uint _workId = options.WorkId;

    private int _a;
    private int _b;

    private uint _sequence;

    private long _lastTicks = -1L;
    private long _lastTicksAsync = -1L;


    /// <summary>
    /// 转换时钟周期数精度。
    /// </summary>
    /// <param name="ticks">给定的时钟周期数。</param>
    /// <returns>返回长整数。</returns>
    protected override long ConvertTicksPrecision(long ticks)
        => ticks / 10; // 转为微秒


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    public override Guid GenerateId()
    {
        var nowTicks = GetNowTicks();

        Options.GeneratingAction?.Invoke(new(nowTicks, 0, TimePrecision.Microsecond));

        return CreateGuid(nowTicks, false);
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
    public override async ValueTask<Guid> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var nowTicksAsync = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);

        Options.GeneratingAction?.Invoke(new(nowTicksAsync, 0, TimePrecision.Microsecond));

        return CreateGuid(nowTicksAsync, true);
    }


    /// <summary>
    /// 创建 <see cref="Guid"/>。
    /// </summary>
    /// <param name="ticks">给定的时钟周期数。</param>
    /// <param name="isAsync">是否异步。</param>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    protected virtual Guid CreateGuid(long ticks, bool isAsync)
    {
        var lastTicks = isAsync ? _lastTicksAsync : _lastTicks;

        if (ticks > lastTicks)
        {
            UpdateTicks(ticks, isAsync);
        }
        else if (_sequence == uint.MaxValue)
        {
            UpdateTicks(lastTicks + 1, isAsync);
        }
        else
        {
            if (isAsync)
                _lastTicksAsync = ticks;
            else
                _lastTicks = ticks;
        }

        var a = _a;
        var b = _b;
        var s = _sequence++;

        var buffer = ArrayPool<byte>.Shared.Rent(16);

        buffer[0] = (byte)(a >> 24);
        buffer[1] = (byte)(a >> 16);
        buffer[2] = (byte)(a >> 8);
        buffer[3] = (byte)a;
        buffer[4] = (byte)(b >> 24);
        buffer[5] = (byte)(b >> 16);
        buffer[6] = (byte)(b >> 8);
        buffer[7] = (byte)b;
        buffer[8] = (byte)(_workId >> 24);
        buffer[9] = (byte)(_workId >> 16);
        buffer[10] = (byte)(_workId >> 8);
        buffer[11] = (byte)_workId;
        buffer[12] = (byte)(s >> 24);
        buffer[13] = (byte)(s >> 16);
        buffer[14] = (byte)(s >> 8);
        buffer[15] = (byte)(s >> 0);

        var guid = new Guid(buffer);

        ArrayPool<byte>.Shared.Return(buffer);

        return guid;
    }

    private void UpdateTicks(long ticks, bool isAsync)
    {
        _b = (int)(ticks & 0xFFFFFFFF);
        _a = (int)(ticks >> 32);

        _sequence = uint.MinValue;

        if (isAsync)
        {
            _lastTicksAsync = ticks;
        }
        else
        {
            _lastTicks = ticks;
        }
    }

}

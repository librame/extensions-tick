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
/// 定义 COMB <see cref="Guid"/> 格式的雪花标识生成器（包含 64 位完整的时间标记，32 位工作标识，32 位序列）。
/// </summary>
public class CombSnowflakeIdGenerator : AbstractClockIdGenerator<Guid>
{
    private readonly uint _workId;

    private int _a;
    private int _b;
    private long _lastTicks;
    private uint _sequence;


    /// <summary>
    /// 使用内置的 <see cref="Bootstrapper.GetClock()"/> 构造一个 <see cref="CombSnowflakeIdGenerator"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    public CombSnowflakeIdGenerator(IdGenerationOptions options)
        : this(options, Bootstrapper.GetClock())
    {
    }

    /// <summary>
    /// 构造一个 <see cref="CombSnowflakeIdGenerator"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>。</param>
    public CombSnowflakeIdGenerator(IdGenerationOptions options, IClockBootstrap clock)
        : base(options, clock)
    {
        _workId = options.WorkId;
    }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    public override Guid GenerateId()
    {
        var ticks = GetNowTicks();
        return CreateGuid(ticks / 10); // 转为微秒
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
    public override async Task<Guid> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var ticks = await GetNowTicksAsync(cancellationToken).DisableAwaitContext();
        return CreateGuid(ticks / 10); // 转为微秒
    }


    /// <summary>
    /// 创建 <see cref="Guid"/>。
    /// </summary>
    /// <param name="ticks">给定的时间刻度。</param>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    protected virtual Guid CreateGuid(long ticks)
    {
        if (ticks > _lastTicks)
            UpdateTicks(ticks);

        else if (_sequence == uint.MaxValue)
            UpdateTicks(_lastTicks + 1);

        var a = _a;
        var b = _b;
        var s = _sequence++;

        var bytes = new byte[16];

        bytes[0] = (byte)(a >> 24);
        bytes[1] = (byte)(a >> 16);
        bytes[2] = (byte)(a >> 8);
        bytes[3] = (byte)a;
        bytes[4] = (byte)(b >> 24);
        bytes[5] = (byte)(b >> 16);
        bytes[6] = (byte)(b >> 8);
        bytes[7] = (byte)b;
        bytes[8] = (byte)(_workId >> 24);
        bytes[9] = (byte)(_workId >> 16);
        bytes[10] = (byte)(_workId >> 8);
        bytes[11] = (byte)_workId;
        bytes[12] = (byte)(s >> 24);
        bytes[13] = (byte)(s >> 16);
        bytes[14] = (byte)(s >> 8);
        bytes[15] = (byte)(s >> 0);

        return new Guid(bytes);
    }

    private void UpdateTicks(long ticks)
    {
        _b = (int)(ticks & 0xFFFFFFFF);
        _a = (int)(ticks >> 32);

        _sequence = uint.MinValue;
        _lastTicks = ticks;
    }


    /// <summary>
    /// 转为 <see cref="DateTimeOffset"/>。
    /// </summary>
    /// <param name="combSnowflakeId">给定的 <see cref="Guid"/>。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public virtual DateTimeOffset ToDateTime(Guid combSnowflakeId)
    {
        var now = Clock.GetUtcNow();
        return ToDateTimeCore(combSnowflakeId, now.Offset);
    }

    /// <summary>
    /// 异步转为 <see cref="DateTimeOffset"/>。
    /// </summary>
    /// <param name="combSnowflakeId">给定的 <see cref="Guid"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public virtual async Task<DateTimeOffset> ToDateTimeAsync(Guid combSnowflakeId,
        CancellationToken cancellationToken = default)
    {
        var now = await Clock.GetUtcNowAsync(cancellationToken: cancellationToken).DisableAwaitContext();
        return ToDateTimeCore(combSnowflakeId, now.Offset);
    }

    /// <summary>
    /// 转为 <see cref="DateTimeOffset"/>。
    /// </summary>
    /// <param name="combSnowflakeId">给定的 <see cref="Guid"/>。</param>
    /// <param name="utcOffset">给定的 <see cref="TimeSpan"/> UTC 偏移量。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    protected virtual DateTimeOffset ToDateTimeCore(Guid combSnowflakeId, TimeSpan utcOffset)
    {
        var bytes = combSnowflakeId.ToByteArray();

        var ticks = (long)bytes[0] << 56;
        ticks += (long)bytes[1] << 48;
        ticks += (long)bytes[2] << 40;
        ticks += (long)bytes[3] << 32;
        ticks += (long)bytes[3] << 24;
        ticks += (long)bytes[3] << 16;
        ticks += (long)bytes[3] << 8;
        ticks += bytes[3];

        return new DateTimeOffset(AddBaseTicks(ticks * 10), utcOffset); // 转回纳秒计算，但精度只能到微秒
    }

}

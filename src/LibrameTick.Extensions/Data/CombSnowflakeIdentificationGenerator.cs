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

namespace Librame.Extensions.Data;

/// <summary>
/// 定义 COMB <see cref="Guid"/> 格式的雪花标识生成器（包含 64 位完整的时间标记，32 位工作标识，32 位序列）。
/// </summary>
public class CombSnowflakeIdentificationGenerator : AbstractIdentificationGenerator<Guid>
{
    private readonly IClockBootstrap _clock;
    private readonly ILockerBootstrap _locker;

    private readonly uint _workId;
    private int _a;
    private int _b;
    private long _lastTick;
    private uint _sequence;


    /// <summary>
    /// 构造一个 <see cref="CombIdentificationGenerator"/>。
    /// </summary>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>（如使用本地时钟可参考 <see cref="Bootstrapper.GetClock()"/>）。</param>
    /// <param name="locker">给定的 <see cref="ILockerBootstrap"/>（如使用本地锁定器可参考 <see cref="Bootstrapper.GetLocker()"/>）。</param>
    /// <param name="options">给定的 <see cref="IdentificationGenerationOptions"/>。</param>
    public CombSnowflakeIdentificationGenerator(IClockBootstrap clock, ILockerBootstrap locker,
        IdentificationGenerationOptions options)
    {
        _clock = clock;
        _locker = locker;
        _workId = options.WorkId;
    }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回 <see cref="Guid"/>。</returns>
    public override Guid GenerateId()
    {
        var ticks = _clock.GetUtcNow().Ticks;

        var a = 0;
        var b = 0;
        var sequence = uint.MinValue;

        _locker.SpinLock(() =>
        {
            if (ticks > _lastTick)
                UpdateTimestamp(ticks);
            else if (_sequence == uint.MaxValue)
                UpdateTimestamp(_lastTick + 1);

            sequence = _sequence++;

            a = _a;
            b = _b;
        });

        var s = sequence;
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

        void UpdateTimestamp(long tick)
        {
            _b = (int)(tick & 0xFFFFFFFF);
            _a = (int)(tick >> 32);

            _sequence = uint.MinValue;
            _lastTick = tick;
        }
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
    public override Task<Guid> GenerateIdAsync(CancellationToken cancellationToken = default)
        => cancellationToken.RunTask(GenerateId);


    /// <summary>
    /// 获取标识包含的日期与时间。
    /// </summary>
    /// <param name="combSnowflakeId">给定的 <see cref="Guid"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public static DateTimeOffset GetDateTime(Guid combSnowflakeId, IClockBootstrap clock)
    {
        var bytes = combSnowflakeId.ToByteArray();

        var tick = (long)bytes[0] << 56;
        tick += (long)bytes[1] << 48;
        tick += (long)bytes[2] << 40;
        tick += (long)bytes[3] << 32;
        tick += (long)bytes[3] << 24;
        tick += (long)bytes[3] << 16;
        tick += (long)bytes[3] << 8;
        tick += bytes[3];

        var utcNow = clock.GetUtcNow();
        return new DateTimeOffset(tick, utcNow.Offset);
    }

}

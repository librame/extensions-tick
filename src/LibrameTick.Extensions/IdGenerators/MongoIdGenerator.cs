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
/// 定义 MongoDB 字符串型标识生成器（可生成长度 24 位且包含数字、字母的字符串标识）。
/// </summary>
public class MongoIdGenerator : AbstractClockIdGenerator<string>
{
    private int _location = Environment.TickCount;


    /// <summary>
    /// 使用内置的 <see cref="Bootstrapper.GetClock()"/> 构造一个 <see cref="MongoIdGenerator"/>。
    /// </summary>
    /// <param name="mongos">给定的 <see cref="MongoIdOptions"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    public MongoIdGenerator(MongoIdOptions mongos, IdGenerationOptions options)
        : this(mongos, options, Bootstrapper.GetClock())
    {
    }

    /// <summary>
    /// 构造一个 <see cref="MongoIdGenerator"/>。
    /// </summary>
    /// <param name="mongos">给定的 <see cref="MongoIdOptions"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>。</param>
    public MongoIdGenerator(MongoIdOptions mongos, IdGenerationOptions options, IClockBootstrap clock)
        : base(options, clock)
    {
        Mongos = mongos;
    }


    /// <summary>
    /// Mongo 标识选项。
    /// </summary>
    public MongoIdOptions Mongos { get; init; }


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string GenerateId()
        => CreateId(GetNowTicks());

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public override async Task<string> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var ticksAsync = await GetNowTicksAsync(cancellationToken).DisableAwaitContext();
        return CreateId(ticksAsync);
    }


    /// <summary>
    /// 创建标识。
    /// </summary>
    /// <param name="ticks">给定的时间刻度。</param>
    /// <returns>返回字符串。</returns>
    protected virtual string CreateId(long ticks)
    {
        var increment = Interlocked.Increment(ref _location);
        return Mongos.CreateId(Convert.ToInt32(ticks / 10000000), increment); // 转为秒
    }


    /// <summary>
    /// 转为 <see cref="DateTimeOffset"/>。
    /// </summary>
    /// <param name="mongoId">给定的标识字符串。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public virtual DateTimeOffset ToDateTime(string mongoId)
    {
        var now = Clock.GetUtcNow();
        var _ = MongoIdOptions.Parse(mongoId, out var ticks);

        return new DateTimeOffset(AddBaseTicks(ticks * 10000000), now.Offset); // 转回纳秒计算，但精度只能到秒
    }

    /// <summary>
    /// 异步转为 <see cref="DateTimeOffset"/>。
    /// </summary>
    /// <param name="mongoId">给定的标识字符串。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public virtual async Task<DateTimeOffset> ToDateTimeAsync(string mongoId,
        CancellationToken cancellationToken = default)
    {
        var now = await Clock.GetUtcNowAsync(cancellationToken: cancellationToken).DisableAwaitContext();
        var _ = MongoIdOptions.Parse(mongoId, out var ticks);

        return new DateTimeOffset(AddBaseTicks(ticks * 10000000), now.Offset); // 转回纳秒计算，但精度只能到秒
    }

}

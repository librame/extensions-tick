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
/// 定义 MongoDB 字符串型标识生成器（可生成长度 24 位且包含数字、字母的字符串标识）。
/// </summary>
public class MongoIdGenerator : ClockIdGenerator<string>
{
    private int _location = Environment.TickCount;

    private readonly long _baseTicks = -1L;
    private long _lastTicks = -1L;
    private long _lastTicksAsync = -1L;


    /// <summary>
    /// 构造一个 <see cref="MongoIdGenerator"/>。
    /// </summary>
    /// <param name="mongos">给定的 <see cref="MongoIdOptions"/>。</param>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockDependency"/>。</param>
    public MongoIdGenerator(MongoIdOptions mongos, IdGenerationOptions options, IClockDependency clock)
        : base(options, clock)
    {
        _baseTicks = base.GetBaseTicks();

        Mongos = mongos;
    }


    /// <summary>
    /// Mongo 标识选项。
    /// </summary>
    public MongoIdOptions Mongos { get; init; }


    /// <summary>
    /// 转换时钟周期数精度。
    /// </summary>
    /// <param name="ticks">给定的时钟周期数。</param>
    /// <returns>返回长整数。</returns>
    protected override long ConvertTicksPrecision(long ticks)
        => ticks / 1000_000_0; // 转为秒


    /// <summary>
    /// 生成标识。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string GenerateId()
    {
        var nowTicks = GetNowTicks();

        _lastTicks = nowTicks;
        Options.GeneratingAction?.Invoke(new(nowTicks, _baseTicks, TimePrecision.Second));

        return CreateId(nowTicks - _baseTicks);
    }

    /// <summary>
    /// 异步生成标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含字符串的异步操作。</returns>
    public override async ValueTask<string> GenerateIdAsync(CancellationToken cancellationToken = default)
    {
        var nowTicksAsync = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);

        _lastTicksAsync = nowTicksAsync;
        Options.GeneratingAction?.Invoke(new(nowTicksAsync, _baseTicks, TimePrecision.Second));

        return CreateId(nowTicksAsync - _baseTicks);
    }


    /// <summary>
    /// 创建标识。
    /// </summary>
    /// <param name="deltaTicks">给定的时钟周期数变数。</param>
    /// <returns>返回字符串。</returns>
    protected virtual string CreateId(long deltaTicks)
    {
        var increment = Interlocked.Increment(ref _location);
        return Mongos.CreateId(Convert.ToInt32(deltaTicks), increment);
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
        => cancellationToken.SimpleValueTask(() => _lastTicksAsync);


    /// <summary>
    /// 解析时钟周期数。
    /// </summary>
    /// <param name="mongoId">给定的标识字符串。</param>
    /// <returns>返回长整数。</returns>
    public virtual long ParseTicks(string mongoId)
    {
        MongoIdOptions.Parse(mongoId, out var deltaTicks);
        return _baseTicks + deltaTicks;
    }

}

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

namespace Librame.Extensions.IdGeneration;

/// <summary>
/// 定义继承 <see cref="AbstractIdGenerator{TId}"/> 的时钟类标识生成器。
/// </summary>
/// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
/// <param name="clock">给定的 <see cref="IClockDependency"/>。</param>
public abstract class AbstractClockIdGenerator<TId>(IdGenerationOptions options, IClockDependency clock)
    : AbstractIdGenerator<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// 生成选项。
    /// </summary>
    public IdGenerationOptions Options { get; init; } = options;

    /// <summary>
    /// 时钟。
    /// </summary>
    public IClockDependency Clock { get; init; } = clock;


    /// <summary>
    /// 转换时钟周期数精度（默认不转换，即原 100ns 精度）。
    /// </summary>
    /// <param name="ticks">给定的时钟周期数。</param>
    /// <returns>返回长整数。</returns>
    protected virtual long ConvertTicksPrecision(long ticks)
        => ticks;


    /// <summary>
    /// 获取基础时钟周期数。
    /// </summary>
    /// <returns>返回长整数。</returns>
    protected virtual long GetBaseTicks()
        => ConvertTicksPrecision(Options.UtcBaseTicks);


    /// <summary>
    /// 获取当前时钟周期数。
    /// </summary>
    /// <returns>返回长整数。</returns>
    protected virtual long GetNowTicks()
    {
        var ticks = ConvertTicksPrecision(Clock.GetUtcNow().Ticks);

        Options.UpdateNowTicksAction?.Invoke(ticks);

        return ticks;
    }

    /// <summary>
    /// 获取当前时钟周期数（支持时间回拨）。
    /// </summary>
    /// <param name="lastTicks">给定的上次时钟周期数。</param>
    /// <returns>返回长整数。</returns>
    protected virtual long GetNowTicks(long lastTicks)
    {
        var nowTicks = GetNowTicks();

        while (nowTicks <= lastTicks)
            nowTicks = GetNowTicks();

        return nowTicks;
    }


    /// <summary>
    /// 异步获取当前时钟周期数。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含长整数的异步操作。</returns>
    protected virtual async ValueTask<long> GetNowTicksAsync(CancellationToken cancellationToken = default)
    {
        var now = await Clock.GetUtcNowAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        var ticks = ConvertTicksPrecision(now.Ticks);

        Options.UpdateNowTicksAction?.Invoke(ticks);

        return ticks;
    }

    /// <summary>
    /// 异步获取当前时钟周期数（支持时间回拨）。
    /// </summary>
    /// <param name="lastTicks">给定的上次时钟周期数。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含长整数的异步操作。</returns>
    protected virtual async ValueTask<long> GetNowTicksAsync(long lastTicks,
        CancellationToken cancellationToken = default)
    {
        var nowTicks = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);

        while (nowTicks <= lastTicks)
            nowTicks = await GetNowTicksAsync(cancellationToken).ConfigureAwait(false);

        return nowTicks;
    }

}

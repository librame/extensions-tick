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
/// 定义继承 <see cref="AbstractIdGenerator{TId}"/> 的时钟类标识生成器抽象实现。
/// </summary>
public abstract class AbstractClockIdGenerator<TId> : AbstractIdGenerator<TId>
    where TId : IEquatable<TId>
{
    /// <summary>
    /// 构造一个 <see cref="AbstractClockIdGenerator{TId}"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="IdGenerationOptions"/>。</param>
    /// <param name="clock">给定的 <see cref="IClockBootstrap"/>。如使用本地时钟可参考 <see cref="Bootstrapper.GetClock()"/>。</param>
    protected AbstractClockIdGenerator(IdGenerationOptions options, IClockBootstrap clock)
    {
        Options = options;
        Clock = clock;
    }


    /// <summary>
    /// 生成选项。
    /// </summary>
    public IdGenerationOptions Options { get; init; }

    /// <summary>
    /// 时钟。
    /// </summary>
    public IClockBootstrap Clock { get; init; }


    /// <summary>
    /// 转换时钟周期数精度（默认不转换，即原 100ns 精度）。
    /// </summary>
    /// <param name="ticks">给定的时钟周期数。</param>
    /// <returns>返回长整数。</returns>
    protected virtual long ConvertTicksAccuracy(long ticks)
        => ticks;


    /// <summary>
    /// 获取基础时钟周期数。
    /// </summary>
    /// <returns>返回长整数。</returns>
    protected virtual long GetBaseTicks()
        => ConvertTicksAccuracy(Options.UtcBaseTicks);


    /// <summary>
    /// 获取当前时钟周期数。
    /// </summary>
    /// <returns>返回长整数。</returns>
    protected virtual long GetNowTicks()
    {
        var ticks = ConvertTicksAccuracy(Clock.GetUtcNow().Ticks);

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
    protected virtual async Task<long> GetNowTicksAsync(CancellationToken cancellationToken = default)
    {
        var now = await Clock.GetUtcNowAsync(cancellationToken: cancellationToken).DisableAwaitContext();
        var ticks = ConvertTicksAccuracy(now.Ticks);

        Options.UpdateNowTicksAction?.Invoke(ticks);

        return ticks;
    }

    /// <summary>
    /// 异步获取当前时钟周期数（支持时间回拨）。
    /// </summary>
    /// <param name="lastTicks">给定的上次时钟周期数。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含长整数的异步操作。</returns>
    protected virtual async Task<long> GetNowTicksAsync(long lastTicks,
        CancellationToken cancellationToken = default)
    {
        var nowTicks = await GetNowTicksAsync(cancellationToken).DisableAwaitContext();

        while (nowTicks <= lastTicks)
            nowTicks = await GetNowTicksAsync(cancellationToken).DisableAwaitContext();

        return nowTicks;
    }

}

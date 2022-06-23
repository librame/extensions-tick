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
    private long? _lastTicks;
    private long? _lastTicksAsync;


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
    /// 更新当前时间刻度动作。
    /// </summary>
    public Action<long>? UpdateNowTicksAction { get; set; }


    /// <summary>
    /// 获取当前时间刻度。
    /// </summary>
    /// <returns>返回长整数。</returns>
    protected virtual long GetNowTicks()
    {
        var ticks = GetNowTicksSpan();

        if (_lastTicks is not null)
        {
            while (ticks <= _lastTicks)
            {
                ticks = GetNowTicksSpan();

                UpdateNowTicksAction?.Invoke(ticks);
            }
        }

        _lastTicks = ticks;

        return ticks;
    }

    /// <summary>
    /// 获取排除基础时间刻度的当前时间刻度差值。
    /// </summary>
    /// <returns>返回长整数。</returns>
    protected virtual long GetNowTicksSpan()
        => Clock.GetUtcNow().Ticks - Options.UtcBaseTicks;

    /// <summary>
    /// 增加基础时间刻度的当前时间刻度。
    /// </summary>
    /// <param name="ticks">给定的时间刻度。</param>
    /// <returns>返回长整数。</returns>
    protected virtual long AddBaseTicks(long ticks)
        => ticks + Options.UtcBaseTicks;


    /// <summary>
    /// 异步获取当前时间刻度。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回长整数。</returns>
    protected virtual async Task<long> GetNowTicksAsync(CancellationToken cancellationToken = default)
    {
        var ticksAsync = await GetNowTicksSpanAsync(cancellationToken);

        if (_lastTicksAsync is not null)
        {
            while (ticksAsync <= _lastTicksAsync)
            {
                ticksAsync = await GetNowTicksSpanAsync(cancellationToken);

                UpdateNowTicksAction?.Invoke(ticksAsync);
            }
        }

        _lastTicksAsync = ticksAsync;

        return ticksAsync;
    }

    /// <summary>
    /// 异步获取排除基础时间刻度的当前时间刻度差值。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回长整数。</returns>
    protected virtual async Task<long> GetNowTicksSpanAsync(CancellationToken cancellationToken = default)
    {
        var now = await Clock.GetUtcNowAsync(cancellationToken: cancellationToken).DisableAwaitContext();
        return now.Ticks - Options.UtcBaseTicks;
    }

}

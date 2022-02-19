#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 继承自 <see cref="AbstractEquilizer{TSource}"/> 的异常均衡器（执行单个来源）。
/// </summary>
/// <remarks>
/// 说明：每次调用单个来源的指定成员处理动作或方法。当处理发生异常，可自行依次切换到下一个来源处理；当切换次数达到指定的重试次数时，将抛出异常。
/// </remarks>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class ExceptionEquilizer<TSource> : AbstractEquilizer<TSource>
{
    private readonly int _numberOfRetries;
    private readonly TimeSpan _waitTimeBetweenRetries;


    /// <summary>
    /// 构造一个 <see cref="ExceptionEquilizer{TSource}"/> 实例。
    /// </summary>
    /// <param name="sources">给定的 <typeparamref name="TSource"/> 集合。</param>
    public ExceptionEquilizer(IEnumerable<TSource> sources)
        : this(sources, numberOfRetries: 0, waitTimeBetweenRetries: null)
    {
    }

    /// <summary>
    /// 构造一个 <see cref="ExceptionEquilizer{TSource}"/> 实例。
    /// </summary>
    /// <param name="sources">给定的 <typeparamref name="TSource"/> 集合。</param>
    /// <param name="numberOfRetries">给定的重试次数。如果次数小于1，则遍历所有来源。</param>
    /// <param name="waitTimeBetweenRetries">给定的重试等待时间间隔。</param>
    public ExceptionEquilizer(IEnumerable<TSource> sources,
        int numberOfRetries, TimeSpan? waitTimeBetweenRetries)
        : base(sources)
    {
        if (numberOfRetries < 1)
            numberOfRetries = sources.NonEnumeratedCount();

        _numberOfRetries = numberOfRetries;
        _waitTimeBetweenRetries = waitTimeBetweenRetries ?? TimeSpan.Zero;
    }


    /// <summary>
    /// 调用指定的方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    public override IEnumerable<TResult> Invoke<TResult>(Func<TSource, TResult> func)
    {
        var results = new List<TResult>();
        var currentRetries = 0;

        while (true)
        {
            try
            {
                // 初始使用第一个来源实例（采用跳过索引方式）
                var source = Sources.Skip(currentRetries).First();
                results.Add(func(source));

                return results;
            }
            catch
            {
                currentRetries++;

                if (currentRetries == _numberOfRetries)
                    throw;

                if (_waitTimeBetweenRetries != TimeSpan.Zero)
                    Thread.Sleep(_waitTimeBetweenRetries);
            }
        }
    }

}

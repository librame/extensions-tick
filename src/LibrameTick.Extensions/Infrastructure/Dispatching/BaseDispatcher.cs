#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Dispatching;

/// <summary>
/// 定义实现 <see cref="IDispatcher{TSource}"/> 的基础调度器。
/// </summary>
/// <remarks>
/// 说明：当调度发生异常时，可自行根据选项设定进行重试。当超过设定的重试次数后会依次切换到下一个来源处理。
/// </remarks>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
/// <remarks>
/// 抽象构造一个 <see cref="BaseDispatcher{TSource}"/>。
/// </remarks>
/// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
/// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
/// <param name="options">给定的 <see cref="DispatchingOptions"/>。</param>
public class BaseDispatcher<TSource>(IEnumerable<TSource> sources,
    DispatchingMode mode, DispatchingOptions options) : IDispatcher<TSource>
    where TSource : IEquatable<TSource>
{
    /// <summary>
    /// 调度器选项。
    /// </summary>
    public DispatchingOptions Options { get; init; } = options;

    /// <summary>
    /// 调度模式。
    /// </summary>
    public DispatchingMode Mode { get; init; } = mode;

    /// <summary>
    /// 来源集合。
    /// </summary>
    public IEnumerable<TSource> Sources { get; init; } = sources;

    /// <summary>
    /// 来源集合数。
    /// </summary>
    public int Count => Sources.Count();

    /// <summary>
    /// 首个来源。
    /// </summary>
    public TSource FirstSource => Sources.First();

    /// <summary>
    /// 末尾来源。
    /// </summary>
    public TSource LastSource => Sources.Last();


    /// <summary>
    /// 当前来源集合。
    /// </summary>
    public IEnumerable<TSource>? CurrentSources { get; protected set; }

    /// <summary>
    /// 当前来源。
    /// </summary>
    public TSource? CurrentSource { get; protected set; }

    /// <summary>
    /// 当前来源索引。
    /// </summary>
    public int CurrentIndex { get; protected set; } = -1;

    /// <summary>
    /// 当前失败重试次数。
    /// </summary>
    public int CurrentFailRetries { get; protected set; }


    /// <summary>
    /// 单次调度来源集合的错误动作。
    /// </summary>
    public Action<IDispatcher<TSource>, Exception>? ErrorAction { get; set; }


    #region Action

    /// <summary>
    /// 调度动作。
    /// </summary>
    /// <param name="action">给定的调度动作。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否遍历所有来源集合（可选；默认启用遍历）。</param>
    public virtual void DispatchAction(Action<IDispatcher<TSource>> action,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true)
    {
        CurrentSources = Sources;
        CurrentIndex = 0;

        DispatchingAction(action, breakFunc, isTraversal);
    }

    /// <summary>
    /// 调度动作。
    /// </summary>
    /// <param name="action">给定的调度动作。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法。</param>
    /// <param name="isTraversal">是否遍历所有来源集合。</param>
    protected virtual void DispatchingAction(Action<IDispatcher<TSource>> action,
        Func<IDispatcher<TSource>, bool>? breakFunc, bool isTraversal)
    {
        while (CurrentIndex < Count)
        {
            CurrentFailRetries = 0;

            // 调度单个来源
            DispatchActionCore(action);

            // 如果索引数与当前失败重试次数等于集合数，表示遍历已完成
            if (CurrentIndex + CurrentFailRetries == Count)
            {
                CurrentFailRetries = 0;
                break;
            }

            // 清空调度单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if (breakFunc is not null && breakFunc(this) || !isTraversal)
                break;
        }
    }

    /// <summary>
    /// 调度动作核心。
    /// </summary>
    /// <param name="action">给定的调度动作。</param>
    protected virtual void DispatchActionCore(Action<IDispatcher<TSource>> action)
    {
        try
        {
            CurrentSource = CurrentSources!.Skip(CurrentIndex).First();

            action(this);
        }
        catch (Exception ex)
        {
            ErrorAction?.Invoke(this, ex);

            if (CurrentFailRetries < Options.FailRetries)
                CurrentFailRetries++;
            else
                return;

            // 根据失败重试间隔时长休眠
            if (Options.FailRetryInterval != TimeSpan.Zero)
                Thread.Sleep(Options.FailRetryInterval);

            CurrentIndex++;
            DispatchActionCore(action);
        }
    }


    /// <summary>
    /// 异步调度动作。
    /// </summary>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual Task DispatchActionAsync(Func<IDispatcher<TSource>, Task> func,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        CurrentSources = Sources;
        CurrentIndex = 0;

        return DispatchingActionAsync(func, breakFunc, isTraversal, cancellationToken);
    }

    /// <summary>
    /// 异步调度动作。
    /// </summary>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    protected virtual async Task DispatchingActionAsync(Func<IDispatcher<TSource>, Task> func,
        Func<IDispatcher<TSource>, bool>? breakFunc, bool isTraversal,
        CancellationToken cancellationToken)
    {
        while (CurrentIndex < Count)
        {
            CurrentFailRetries = 0;

            // 调度单个来源
            await DispatchingActionCoreAsync(func, cancellationToken);

            // 如果索引数与当前失败重试次数等于集合数，表示遍历已完成
            if (CurrentIndex + CurrentFailRetries == Count)
            {
                CurrentFailRetries = 0;
                break;
            }

            // 清空调度单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if (breakFunc is not null && breakFunc(this) || !isTraversal)
                break;
        }
    }

    /// <summary>
    /// 异步调度动作核心。
    /// </summary>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    protected virtual async Task DispatchingActionCoreAsync(Func<IDispatcher<TSource>, Task> func,
        CancellationToken cancellationToken)
    {
        try
        {
            CurrentSource = CurrentSources!.Skip(CurrentIndex).First();

            await func(this);
        }
        catch (Exception ex)
        {
            ErrorAction?.Invoke(this, ex);

            if (CurrentFailRetries < Options.FailRetries)
                CurrentFailRetries++;
            else
                return;

            // 根据失败重试间隔时长休眠
            if (Options.FailRetryInterval != TimeSpan.Zero)
                await Task.Delay(Options.FailRetryInterval, cancellationToken);

            CurrentIndex++;
            await DispatchingActionCoreAsync(func, cancellationToken);
        }
    }

    #endregion


    #region Func

    /// <summary>
    /// 调度方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <returns>返回 <typeparamref name="TResult"/> 数组。</returns>
    public virtual TResult[] DispatchFunc<TResult>(Func<IDispatcher<TSource>, TResult> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true)
    {
        CurrentSources = Sources;
        CurrentIndex = 0;

        return DispatchingFunc(func, breakFunc, isTraversal).ToArray();
    }

    /// <summary>
    /// 调度方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    public virtual IEnumerable<TResult> DispatchingFunc<TResult>(Func<IDispatcher<TSource>, TResult> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc, bool isTraversal)
    {
        while (CurrentIndex < Count)
        {
            CurrentFailRetries = 0;

            // 调度单个来源
            var result = DispatchingFuncCore(func);
            if (result is not null)
                yield return result;

            // 如果索引数与当前失败重试次数等于集合数，表示遍历已完成
            if (CurrentIndex + CurrentFailRetries == Count)
            {
                CurrentFailRetries = 0;
                break;
            }

            // 清空调度单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if (breakFunc is not null && breakFunc(this, result!) || !isTraversal)
                break;
        }
    }

    /// <summary>
    /// 调度方法核心并返回结果。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调度方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    protected virtual TResult? DispatchingFuncCore<TResult>(Func<IDispatcher<TSource>, TResult?> func)
    {
        try
        {
            CurrentSource = CurrentSources!.Skip(CurrentIndex).First();

            return func(this);
        }
        catch (Exception ex)
        {
            ErrorAction?.Invoke(this, ex);

            if (CurrentFailRetries < Options.FailRetries)
                CurrentFailRetries++;
            else
                return default;

            // 根据失败重试间隔时长休眠
            if (Options.FailRetryInterval != TimeSpan.Zero)
                Thread.Sleep(Options.FailRetryInterval);

            CurrentIndex++;
            return DispatchingFuncCore(func);
        }
    }


    /// <summary>
    /// 异步调度方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TResult"/> 数组的异步操作。</returns>
    public virtual async Task<TResult[]> DispatchFuncAsync<TResult>(
        Func<IDispatcher<TSource>, Task<TResult>> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        CurrentSources = Sources;
        CurrentIndex = 0;

        return (await DispatchingFuncAsync(func, breakFunc, isTraversal, cancellationToken)).ToArray();
    }

    /// <summary>
    /// 异步调度方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IEnumerable{TResult}"/> 的异步操作。</returns>
    protected virtual async Task<IEnumerable<TResult>> DispatchingFuncAsync<TResult>(
        Func<IDispatcher<TSource>, Task<TResult>> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc, bool isTraversal,
        CancellationToken cancellationToken = default)
    {
        var results = new List<TResult>();

        while (CurrentIndex < Count)
        {
            CurrentFailRetries = 0;

            // 调度单个来源
            var result = await DispatchingFuncCoreAsync(func, cancellationToken);
            if (result is not null)
                results.Add(result);

            // 如果索引数与当前失败重试次数等于集合数，表示遍历已完成
            if (CurrentIndex + CurrentFailRetries == Count)
            {
                CurrentFailRetries = 0;
                break;
            }

            // 清空调度单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if (breakFunc is not null && breakFunc(this, result!) || !isTraversal)
                break;
        }

        return results;
    }

    /// <summary>
    /// 异步调度方法核心并返回结果。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调度方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    protected virtual async Task<TResult?> DispatchingFuncCoreAsync<TResult>(
        Func<IDispatcher<TSource>, Task<TResult>> func,
        CancellationToken cancellationToken)
    {
        try
        {
            CurrentSource = CurrentSources!.Skip(CurrentIndex).First();

            return await func(this);
        }
        catch (Exception ex)
        {
            ErrorAction?.Invoke(this, ex);

            if (CurrentFailRetries < Options.FailRetries)
                CurrentFailRetries++;
            else
                return default;

            // 根据失败重试间隔时长休眠
            if (Options.FailRetryInterval != TimeSpan.Zero)
                await Task.Delay(Options.FailRetryInterval, cancellationToken);

            CurrentIndex++;
            return await DispatchingFuncCoreAsync(func, cancellationToken);
        }
    }

    #endregion


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IDispatcher{TSource}"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(IDispatcher<TSource>? other)
        => other is not null && ToString() == other.ToString();

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => obj is IDispatcher<TSource> other && Equals(other);

    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();

    /// <summary>
    /// 转为以英文逗号分隔的所有来源字符串形式集合的字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => string.Join(',', Sources.Select(static s => s?.ToString()).Distinct());


    /// <summary>
    /// 获取来源可枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerable{TSource}"/>。</returns>
    public virtual IEnumerator<TSource> GetEnumerator()
        => Sources.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}

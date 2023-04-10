#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义实现 <see cref="IDispatcher{TSource}"/> 的基础调度器。
/// </summary>
/// <remarks>
/// 说明：当调用发生异常时，可自行根据选项设定进行重试。当超过设定的重试次数后会依次切换到下一个来源处理。
/// </remarks>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class BaseDispatcher<TSource> : IDispatcher<TSource>
{
    /// <summary>
    /// 抽象构造一个 <see cref="BaseDispatcher{TSource}"/>。
    /// </summary>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>。</param>
    public BaseDispatcher(IEnumerable<TSource> sources, DispatchingOptions options)
    {
        Options = options;
        Sources = sources;
        Count = sources.NonEnumeratedCount();
        CurrentSource = First = sources.First();
        Last = sources.Last();

        CurrentIndex = -1;
    }


    /// <summary>
    /// 调度器选项。
    /// </summary>
    public DispatchingOptions Options { get; init; }

    /// <summary>
    /// 来源集合。
    /// </summary>
    public IEnumerable<TSource> Sources { get; init; }

    /// <summary>
    /// 来源集合数。
    /// </summary>
    public int Count { get; init; }

    /// <summary>
    /// 首个来源。
    /// </summary>
    public TSource First { get; init; }

    /// <summary>
    /// 末尾来源。
    /// </summary>
    public TSource Last { get; init; }


    /// <summary>
    /// 当前来源（初始为 <see cref="First"/>）。
    /// </summary>
    public TSource CurrentSource { get; protected set; }

    /// <summary>
    /// 当前来源索引。
    /// </summary>
    public int CurrentIndex { get; protected set; }

    /// <summary>
    /// 当前失败重试次数。
    /// </summary>
    public int CurrentFailRetries { get; protected set; }


    /// <summary>
    /// 单次调用来源集合的错误动作。
    /// </summary>
    public Action<IDispatcher<TSource>, Exception>? ErrorAction { get; set; }


    #region Invoke

    /// <summary>
    /// 调用来源。
    /// </summary>
    /// <param name="action">给定的调用动作。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否遍历所有来源集合（可选；默认启用遍历）。</param>
    public virtual void InvokeAction(Action<IDispatcher<TSource>> action,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true)
    {
        CurrentIndex = 0;
        while (CurrentIndex < Count)
        {
            CurrentSource = Sources.Skip(CurrentIndex).First();
            CurrentFailRetries = 0;

            // 调用单个来源
            InvokeActionCore(action);

            // 清空调用单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if ((breakFunc is not null && breakFunc(this)) || !isTraversal)
                break;

            CurrentIndex++;
        }
    }

    /// <summary>
    /// 调用核心。
    /// </summary>
    /// <param name="action">给定的调用动作。</param>
    protected virtual void InvokeActionCore(Action<IDispatcher<TSource>> action)
    {
        try
        {
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
                Task.Delay(Options.FailRetryInterval);

            InvokeActionCore(action);
        }
    }


    /// <summary>
    /// 调用来源并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    public virtual IEnumerable<TResult> InvokeFunc<TResult>(Func<IDispatcher<TSource>, TResult> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true)
    {
        var results = new List<TResult>();

        CurrentIndex = 0;
        while (CurrentIndex < Count)
        {
            CurrentSource = Sources.Skip(CurrentIndex).First();
            CurrentFailRetries = 0;

            // 调用单个来源
            var result = InvokeCore(func);
            if (result is not null)
                results.Add(result);

            // 清空调用单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if ((breakFunc is not null && breakFunc(this, result!)) || !isTraversal)
            {
                break;
            }

            CurrentIndex++;
        }

        return results;
    }

    /// <summary>
    /// 调用来源核心并返回结果。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调用方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    protected virtual TResult? InvokeCore<TResult>(Func<IDispatcher<TSource>, TResult?> func)
    {
        try
        {
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
                Task.Delay(Options.FailRetryInterval);

            return InvokeCore(func);
        }
    }

    #endregion


    #region InvokeAsync

    /// <summary>
    /// 异步调用来源。
    /// </summary>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual async Task InvokeActionAsync(Func<IDispatcher<TSource>, Task> func,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        CurrentIndex = 0;
        while (CurrentIndex < Count)
        {
            CurrentSource = Sources.Skip(CurrentIndex).First();
            CurrentFailRetries = 0;

            // 调用单个来源
            await InvokeCoreAsync(func, cancellationToken);

            // 清空调用单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if ((breakFunc is not null && breakFunc(this)) || !isTraversal)
                break;

            CurrentIndex++;
        }
    }

    /// <summary>
    /// 异步调用来源核心。
    /// </summary>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    protected virtual async Task InvokeCoreAsync(Func<IDispatcher<TSource>, Task> func,
        CancellationToken cancellationToken)
    {
        try
        {
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

            await InvokeCoreAsync(func, cancellationToken);
        }
    }


    /// <summary>
    /// 异步调用来源并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IEnumerable{TResult}"/> 的异步操作。</returns>
    public virtual async Task<IEnumerable<TResult>> InvokeFuncAsync<TResult>(
        Func<IDispatcher<TSource>, Task<TResult>> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        var results = new List<TResult>();

        CurrentIndex = 0;
        while (CurrentIndex < Count)
        {
            CurrentSource = Sources.Skip(CurrentIndex).First();
            CurrentFailRetries = 0;

            // 调用单个来源
            var result = await InvokeCoreAsync(func, cancellationToken);
            if (result is not null)
                results.Add(result);

            // 清空调用单个来源可能导致的异常重试次数
            if (CurrentFailRetries > 0)
                CurrentFailRetries = 0;

            // 如果需要强制跳出循环或不启用遍历集合
            if ((breakFunc is not null && breakFunc(this, result!)) || !isTraversal)
                break;

            CurrentIndex++;
        }

        return results;
    }

    /// <summary>
    /// 异步调用来源核心并返回结果。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    protected virtual async Task<TResult?> InvokeCoreAsync<TResult>(
        Func<IDispatcher<TSource>, Task<TResult>> func,
        CancellationToken cancellationToken)
    {
        try
        {
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

            return await InvokeCoreAsync(func, cancellationToken);
        }
    }

    #endregion

}

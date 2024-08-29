#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// 任务静态扩展。
/// </summary>
public static class TaskExtensions
{

    #region SkipByTimeout

    /// <summary>
    /// 超时跳过当前异步任务。
    /// </summary>
    /// <param name="sourceTask">给定的来源 <see cref="Task"/>。</param>
    /// <param name="timeout">给定的超时 <see cref="TimeSpan"/>。</param>
    /// <returns>返回一个异步操作。</returns>
    public static async Task SkipByTimeout(this Task sourceTask, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource();

        var timeoutTask = Task.Delay(timeout, cts.Token);
        var resultTask = await Task.WhenAny(sourceTask, timeoutTask);

        if (resultTask == timeoutTask)
        {
            // sourceTask 来源任务不能取消
        }
        else
        {
            await sourceTask;
        }

        // 取消计时器任务
        cts.Cancel();
    }

    /// <summary>
    /// 超时跳过当前异步任务。
    /// </summary>
    /// <param name="sourceTask">给定的来源 <see cref="Task{TResult}"/>。</param>
    /// <param name="timeout">给定的超时 <see cref="TimeSpan"/>。</param>
    /// <returns>返回一个包含结果的异步操作。</returns>
    public static async Task<TResult?> SkipByTimeout<TResult>(this Task<TResult> sourceTask, TimeSpan timeout)
    {
        TResult? result;

        using var cts = new CancellationTokenSource();

        var timeoutTask = Task.Delay(timeout, cts.Token);
        var resultTask = await Task.WhenAny(sourceTask, timeoutTask);

        if (resultTask == timeoutTask)
        {
            // sourceTask 来源任务不能取消
            result = default;
        }
        else
        {
            result = await sourceTask;
        }

        // 取消计时器任务
        cts.Cancel();

        return result;
    }

    #endregion


    #region SimpleTask

    /// <summary>
    /// 执行一个简单异步任务。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <param name="action">给定要执行的动作。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    public static async Task SimpleTask(this CancellationToken cancellationToken, Action action)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await Task.CompletedTask;
        }

        action.BeginInvoke(action.EndInvoke, null);
    }

    /// <summary>
    /// 执行一个包含结果的简单异步任务。
    /// </summary>
    /// <remarks>
    /// 直接调用 <paramref name="resultFunc"/> 方法并使用 <see cref="Task.FromResult{TResult}(TResult)"/> 对结果包装。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <param name="resultFunc">给定的结果。</param>
    /// <returns>返回 <see cref="Task{TResult}"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="resultFunc"/> invoke result is null.
    /// </exception>
    public static Task<TResult> SimpleTask<TResult>(this CancellationToken cancellationToken,
        Func<TResult> resultFunc)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<TResult>(cancellationToken);
        }

        TResult? result = default;

        resultFunc.BeginInvoke(asyncResult => result = resultFunc.EndInvoke(asyncResult), @object: null);

        return Task.FromResult(result ?? throw InvokeResultFuncIsNull(nameof(resultFunc)));
    }

    /// <summary>
    /// 执行一个包含结果的简单异步任务。
    /// </summary>
    /// <remarks>
    /// 直接使用 <see cref="Task.FromResult{TResult}(TResult)"/> 对结果包装。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <param name="result">给定的结果。</param>
    /// <returns>返回 <see cref="Task{TResult}"/>。</returns>
    public static Task<TResult> SimpleTask<TResult>(this CancellationToken cancellationToken, TResult result)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.FromCanceled<TResult>(cancellationToken);
        }

        return Task.FromResult(result);
    }


    /// <summary>
    /// 执行一个简单异步任务。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <param name="action">给定要执行的动作。</param>
    /// <returns>返回 <see cref="ValueTask"/>。</returns>
    public static async ValueTask SimpleValueTask(this CancellationToken cancellationToken, Action action)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            await ValueTask.CompletedTask;
        }

        action.BeginInvoke(action.EndInvoke, @object: null);
    }

    /// <summary>
    /// 执行一个包含结果的简单异步任务。
    /// </summary>
    /// <remarks>
    /// 直接调用 <paramref name="resultFunc"/> 方法并使用 <see cref="Task.FromResult{TResult}(TResult)"/> 对结果包装。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <param name="resultFunc">给定的结果。</param>
    /// <returns>返回 <see cref="ValueTask{TResult}"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// The <paramref name="resultFunc"/> invoke result is null.
    /// </exception>
    public static ValueTask<TResult> SimpleValueTask<TResult>(this CancellationToken cancellationToken,
        Func<TResult> resultFunc)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<TResult>(cancellationToken);
        }

        TResult? result = default;

        resultFunc.BeginInvoke(asyncResult => result = resultFunc.EndInvoke(asyncResult), @object: null);

        return ValueTask.FromResult(result ?? throw InvokeResultFuncIsNull(nameof(resultFunc)));
    }

    /// <summary>
    /// 执行一个包含结果的简单异步值任务。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <param name="result">给定的结果。</param>
    /// <returns>返回 <see cref="ValueTask{TResult}"/>。</returns>
    public static ValueTask<TResult> SimpleValueTask<TResult>(this CancellationToken cancellationToken, TResult result)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return ValueTask.FromCanceled<TResult>(cancellationToken);
        }

        return ValueTask.FromResult(result);
    }

    private static ArgumentNullException InvokeResultFuncIsNull(string paraName)
        => new(paraName, $"The {paraName} invoke result is null.");

    #endregion

}

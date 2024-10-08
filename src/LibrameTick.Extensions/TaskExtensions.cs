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

    /// <summary>
    /// 异步调用当前动作。
    /// </summary>
    /// <param name="action">给定的调用动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    public static async Task InvokeAsync(this Action action,
        CancellationToken cancellationToken = default)
        => await Task.Run(action, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// 异步调用当前方法。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含结果实例的异步方法。</returns>
    public static async Task<TResult> InvokeAsync<TResult>(this Func<TResult> func,
        CancellationToken cancellationToken = default)
        => await Task.Run(func, cancellationToken).ConfigureAwait(false);


    #region CancelByTimeout

    /// <summary>
    /// 异步超时取消当前动作。
    /// </summary>
    /// <param name="action">给定要执行的动作。</param>
    /// <param name="timeout">给定的超时 <see cref="TimeSpan"/>。</param>
    /// <returns>返回一个包含任务状态的异步操作（通常为 <see cref="TaskStatus.Canceled"/> 或 <see cref="TaskStatus.RanToCompletion"/>）。</returns>
    public static async Task<TaskStatus> CancelByTimeoutAsync(
        this Action action, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource(timeout);

        var timeoutTask = Task.Delay(timeout, cts.Token);

        // 如果不指定超时令牌，则动作任务将不会在超时后自动取消
        var actionTask = new Task(action, cts.Token);

        // 启动动作任务（不启动 Action 将不会被调用）
        actionTask.Start();

        await Task.WhenAny(actionTask, timeoutTask).ConfigureAwait(false);

        // 取消计时器任务
        cts.Cancel();

        // 如果超时任务完成，则表示动作任务被自动取消
        return timeoutTask.Status == TaskStatus.RanToCompletion
            ? TaskStatus.Canceled
            : TaskStatus.RanToCompletion;
    }

    /// <summary>
    /// 异步超时取消当前方法。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定要执行的方法。</param>
    /// <param name="timeout">给定的超时 <see cref="TimeSpan"/>。</param>
    /// <param name="canceledResult">给定任务被取消的默认 <typeparamref name="TResult"/>。</param>
    /// <returns>返回一个包含方法结果的异步操作。</returns>
    public static async Task<TResult> CancelByTimeoutAsync<TResult>(
        this Func<TResult> func, TimeSpan timeout, TResult canceledResult)
    {
        using var cts = new CancellationTokenSource(timeout);

        var timeoutTask = Task.Run(() =>
        {
            Thread.Sleep(timeout);
            return canceledResult;
        },
        cts.Token);

        // 如果不指定超时令牌，则动作任务将不会在超时后自动取消
        var funcTask = new Task<TResult>(func, cts.Token);

        // 启动动作任务（不启动 Action 将不会被调用）
        funcTask.Start();

        var resultTask = await Task.WhenAny(funcTask, timeoutTask).ConfigureAwait(false);

        var result = resultTask.Result;

        // 取消计时器任务
        cts.Cancel();

        return result;
    }

    #endregion


    #region SkipByTimeout

    /// <summary>
    /// 异步超时跳过当前任务。
    /// </summary>
    /// <param name="sourceTask">给定的来源 <see cref="Task"/>。</param>
    /// <param name="timeout">给定的超时 <see cref="TimeSpan"/>。</param>
    /// <returns>返回一个异步操作。</returns>
    public static async Task SkipByTimeoutAsync(this Task sourceTask, TimeSpan timeout)
    {
        using var cts = new CancellationTokenSource();

        var timeoutTask = Task.Delay(timeout, cts.Token);
        var resultTask = await Task.WhenAny(sourceTask, timeoutTask);

        if (resultTask == timeoutTask)
        {
            // 如果来源任务之前没指定超时令牌，则此处不能取消此任务
        }
        else
        {
            await sourceTask;
        }

        // 取消计时器任务
        cts.Cancel();
    }

    /// <summary>
    /// 异步超时跳过当前任务。
    /// </summary>
    /// <param name="sourceTask">给定的来源 <see cref="Task{TResult}"/>。</param>
    /// <param name="timeout">给定的超时 <see cref="TimeSpan"/>。</param>
    /// <returns>返回一个包含结果的异步操作。</returns>
    public static async Task<TResult?> SkipByTimeoutAsync<TResult>(
        this Task<TResult?> sourceTask, TimeSpan timeout)
    {
        TResult? result;

        using var cts = new CancellationTokenSource();

        var timeoutTask = Task.Delay(timeout, cts.Token);
        var resultTask = await Task.WhenAny(sourceTask, timeoutTask);

        if (resultTask == timeoutTask)
        {
            // 如果来源任务之前没指定超时令牌，则此处不能取消此任务
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

}

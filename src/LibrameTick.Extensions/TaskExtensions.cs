#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions
{
    /// <summary>
    /// 任务静态扩展。
    /// </summary>
    public static class TaskExtensions
    {

        /// <summary>
        /// 转换为 <see cref="ValueTask"/>。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        /// <returns>返回 <see cref="ValueTask"/>。</returns>
        public static ValueTask AsValueTask(this Task task)
            => new ValueTask(task);

        /// <summary>
        /// 转换为 <see cref="ValueTask{TResult}"/>。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <see cref="ValueTask{TResult}"/>。</returns>
        public static ValueTask<TResult> AsValueTask<TResult>(this Task<TResult> task)
            => new ValueTask<TResult>(task);


        #region RunTask

        /// <summary>
        /// 运行一个异步任务。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定要执行的动作。</param>
        /// <returns>返回 <see cref="Task"/>。</returns>
        public static Task RunTask(this CancellationToken cancellationToken, Action action)
            => Task.Run(action, cancellationToken);

        /// <summary>
        /// 运行一个包含结果的异步任务。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定要执行的函数。</param>
        /// <returns>返回 <see cref="Task{TResult}"/>。</returns>
        public static Task<TResult> RunTask<TResult>(this CancellationToken cancellationToken, Func<TResult> func)
            => Task.Run(func, cancellationToken);


        /// <summary>
        /// 运行一个异步值任务。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="action">给定要执行的动作。</param>
        /// <returns>返回 <see cref="ValueTask"/>。</returns>
        public static ValueTask RunValueTask(this CancellationToken cancellationToken, Action action)
            => Task.Run(action, cancellationToken).AsValueTask();

        /// <summary>
        /// 运行一个包含结果的异步值任务。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
        /// <param name="func">给定要执行的函数。</param>
        /// <returns>返回 <see cref="ValueTask{TResult}"/>。</returns>
        public static ValueTask<TResult> RunValueTask<TResult>(this CancellationToken cancellationToken, Func<TResult> func)
            => Task.Run(func, cancellationToken).AsValueTask();

        #endregion


        #region ConfiguredTaskAwaitable

        /// <summary>
        /// 结束对已完成任务的等待。
        /// </summary>
        /// <param name="awaitable">给定的 <see cref="ConfiguredTaskAwaitable" />。</param>
        public static void Await(this ConfiguredTaskAwaitable awaitable)
            => awaitable.GetAwaiter().GetResult();

        /// <summary>
        /// 结束对已完成任务的等待，并返回执行结果。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="awaitable">给定的 <see cref="ConfiguredTaskAwaitable" />。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult AwaitResult<TResult>(this ConfiguredTaskAwaitable<TResult> awaitable)
            => awaitable.GetAwaiter().GetResult();


        /// <summary>
        /// 禁用捕获上下文配置可等待任务。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable"/>。</returns>
        public static ConfiguredTaskAwaitable ConfigureAwait(this Task task)
            => task.NotNull(nameof(task)).ConfigureAwait(false);

        /// <summary>
        /// 禁用捕获上下文配置可等待任务。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(this Task<TResult> task)
            => task.NotNull(nameof(task)).ConfigureAwait(false);


        /// <summary>
        /// 使用捕获上下文配置可等待任务。
        /// </summary>
        /// <param name="task">给定的 <see cref="Task"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable"/>。</returns>
        public static ConfiguredTaskAwaitable ConfigureAwaitWithContext(this Task task)
            => task.NotNull(nameof(task)).ConfigureAwait(true);

        /// <summary>
        /// 使用捕获上下文配置可等待任务。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="task">给定的 <see cref="Task{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredTaskAwaitable<TResult> ConfigureAwaitWithContext<TResult>(this Task<TResult> task)
            => task.NotNull(nameof(task)).ConfigureAwait(true);

        #endregion


        #region ConfiguredValueTaskAwaitable

        /// <summary>
        /// 结束对已完成任务的等待。
        /// </summary>
        /// <param name="awaitable">给定的 <see cref="ConfiguredValueTaskAwaitable" />。</param>
        public static void Await(this ConfiguredValueTaskAwaitable awaitable)
            => awaitable.GetAwaiter().GetResult();

        /// <summary>
        /// 结束对已完成任务的等待，并返回执行结果。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="awaitable">给定的 <see cref="ConfiguredValueTaskAwaitable" />。</param>
        /// <returns>返回 <typeparamref name="TResult"/>。</returns>
        public static TResult AwaitResult<TResult>(this ConfiguredValueTaskAwaitable<TResult> awaitable)
            => awaitable.GetAwaiter().GetResult();


        /// <summary>
        /// 使用捕获上下文配置可等待任务。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable"/>。</returns>
        public static ConfiguredValueTaskAwaitable ConfigureAwaitWithContext(this ValueTask valueTask)
            => valueTask.ConfigureAwait(true);

        /// <summary>
        /// 使用捕获上下文配置可等待任务。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredValueTaskAwaitable<TResult> ConfigureAwaitWithContext<TResult>(this ValueTask<TResult> valueTask)
            => valueTask.ConfigureAwait(true);


        /// <summary>
        /// 禁用捕获上下文配置可等待任务。
        /// </summary>
        /// <param name="valueTask">给定的 <see cref="ValueTask"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable"/>。</returns>
        public static ConfiguredValueTaskAwaitable ConfigureAwaitWithoutContext(this ValueTask valueTask)
            => valueTask.ConfigureAwait(false);

        /// <summary>
        /// 禁用捕获上下文配置可等待任务。
        /// </summary>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="valueTask">给定的 <see cref="ValueTask{TResult}"/>。</param>
        /// <returns>返回 <see cref="ConfiguredValueTaskAwaitable{TResult}"/>。</returns>
        public static ConfiguredValueTaskAwaitable<TResult> ConfigureAwaitWithoutContext<TResult>(this ValueTask<TResult> valueTask)
            => valueTask.ConfigureAwait(false);

        #endregion

    }
}

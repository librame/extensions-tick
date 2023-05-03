#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义一个用于处理集合的调度器接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public interface IDispatcher<TSource> : IComposable<TSource>, IEquatable<IDispatcher<TSource>>
    where TSource : IEquatable<TSource>
{
    /// <summary>
    /// 来源集合。
    /// </summary>
    IEnumerable<TSource> Sources { get; }

    /// <summary>
    /// 调度模式。
    /// </summary>
    DispatchingMode Mode { get; }

    /// <summary>
    /// 首个来源。
    /// </summary>
    TSource First { get; }

    /// <summary>
    /// 末尾来源。
    /// </summary>
    TSource Last { get; }


    /// <summary>
    /// 当前来源集合。
    /// </summary>
    IEnumerable<TSource>? CurrentSources { get; }

    /// <summary>
    /// 当前来源。
    /// </summary>
    TSource? CurrentSource { get; }

    /// <summary>
    /// 当前来源索引。
    /// </summary>
    int CurrentIndex { get; }

    /// <summary>
    /// 当前失败重试次数。
    /// </summary>
    int CurrentFailRetries { get; }


    /// <summary>
    /// 单次调用来源集合的错误动作。
    /// </summary>
    Action<IDispatcher<TSource>, Exception>? ErrorAction { get; set; }


    /// <summary>
    /// 调度动作。
    /// </summary>
    /// <param name="action">给定的调用动作。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否遍历所有来源集合（可选；默认启用遍历）。</param>
    void DispatchAction(Action<IDispatcher<TSource>> action,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true);

    /// <summary>
    /// 异步调度动作。
    /// </summary>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    Task DispatchActionAsync(Func<IDispatcher<TSource>, Task> func,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// 调度方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <returns>返回 <typeparamref name="TResult"/> 数组。</returns>
    TResult[] DispatchFunc<TResult>(Func<IDispatcher<TSource>, TResult> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true);

    /// <summary>
    /// 异步调度方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TResult"/> 数组的异步操作。</returns>
    Task<TResult[]> DispatchFuncAsync<TResult>(Func<IDispatcher<TSource>, Task<TResult>> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default);
}

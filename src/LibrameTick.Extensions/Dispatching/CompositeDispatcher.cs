#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Dispatching;

/// <summary>
/// 定义实现 <see cref="IDispatcher{TSource}"/> 的复合调度器。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
/// <remarks>
/// 抽象构造一个 <see cref="CompositeDispatcher{TSource}"/>。
/// </remarks>
/// <param name="dispatchers">给定的调度器集合（默认使用 <see cref="DispatchingMode.Striping"/> 复合所有 <typeparamref name="TSource"/>）。</param>
/// <param name="options">给定的 <see cref="DispatchingOptions"/>。</param>
public class CompositeDispatcher<TSource>(IEnumerable<IDispatcher<TSource>> dispatchers, DispatchingOptions options)
    : BaseDispatcher<TSource>(dispatchers.SelectMany(static s => s), DispatchingMode.Striping, options), IComposable<IDispatcher<TSource>>
    where TSource : IEquatable<TSource>
{
    private IEnumerable<IDispatcher<TSource>> _dispatchers = dispatchers;


    /// <summary>
    /// 事务中断的错误动作。
    /// </summary>
    public Action<IDispatcher<TSource>, Exception>? TransactionAbortedErrorAction { get; set; }


    /// <summary>
    /// 调度器集合数。
    /// </summary>
    public int DispatchersCount { get; init; } = dispatchers.Count();


    #region Action

    /// <summary>
    /// 调用动作。
    /// </summary>
    /// <param name="action">给定的调用动作。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否遍历所有来源集合（可选；默认启用遍历）。</param>
    public override void DispatchAction(Action<IDispatcher<TSource>> action,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                foreach (var dispatcher in _dispatchers)
                {
                    CurrentSources = dispatcher.Sources;
                    CurrentIndex = 0;

                    base.DispatchingAction(action, breakFunc, isTraversal);
                }

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
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
    public override async Task DispatchActionAsync(Func<IDispatcher<TSource>, Task> func,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                foreach (var dispatcher in _dispatchers)
                {
                    CurrentSources = Sources;
                    CurrentIndex = 0;

                    await base.DispatchingActionAsync(func, breakFunc, isTraversal, cancellationToken).ConfigureAwait(false);
                }

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
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
    public override TResult[] DispatchFunc<TResult>(Func<IDispatcher<TSource>, TResult> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true)
    {
        var result = new List<TResult>(Count);

        using (var transaction = new TransactionScope())
        {
            try
            {
                foreach (var dispatcher in _dispatchers)
                {
                    CurrentSources = Sources;
                    CurrentIndex = 0;

                    result.AddRange(base.DispatchingFunc(func, breakFunc, isTraversal));
                }

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
        }

        return result.ToArray();
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
    public override async Task<TResult[]> DispatchFuncAsync<TResult>(
        Func<IDispatcher<TSource>, Task<TResult>> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        var result = new List<TResult>(Count);

        using (var transaction = new TransactionScope())
        {
            try
            {
                foreach (var dispatcher in _dispatchers)
                {
                    CurrentSources = Sources;
                    CurrentIndex = 0;

                    result.AddRange(await base.DispatchFuncAsync(func, breakFunc, isTraversal, cancellationToken).ConfigureAwait(false));
                }

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
        }

        return result.ToArray();
    }

    #endregion


    IEnumerator<IDispatcher<TSource>> IEnumerable<IDispatcher<TSource>>.GetEnumerator()
        => _dispatchers.GetEnumerator();

}

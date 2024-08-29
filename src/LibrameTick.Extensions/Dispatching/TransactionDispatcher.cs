#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dispatching;

/// <summary>
/// 定义继承 <see cref="BaseDispatcher{TSource}"/> 的事务遍历调度器（依次执行多个来源）。
/// </summary>
/// <remarks>
/// 说明：依次调度每个来源的指定成员处理动作或方法。当处理发生异常，将自行回滚之前的所有处理，并捕获 <see cref="TransactionAbortedException"/> 异常进行处理。
/// </remarks>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
/// <remarks>
/// 构造一个 <see cref="TransactionDispatcher{TSource}"/> 实例。
/// </remarks>
/// <param name="sources">给定的 <typeparamref name="TSource"/> 集合。</param>
/// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
/// <param name="options">给定的 <see cref="DispatchingOptions"/>。</param>
public class TransactionDispatcher<TSource>(IEnumerable<TSource> sources,
    DispatchingMode mode, DispatchingOptions options) : BaseDispatcher<TSource>(sources, mode, options)
    where TSource : IEquatable<TSource>
{
    /// <summary>
    /// 事务中断的错误动作。
    /// </summary>
    public Action<IDispatcher<TSource>, Exception>? TransactionAbortedErrorAction { get; set; }


    #region Action

    /// <summary>
    /// 调度动作。
    /// </summary>
    /// <param name="action">给定的调度动作。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否遍历所有来源集合（可选；默认启用遍历）。</param>
    public override void DispatchAction(Action<IDispatcher<TSource>> action,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                base.DispatchAction(action, breakFunc, isTraversal);

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
        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                await base.DispatchActionAsync(func, breakFunc, isTraversal, cancellationToken).ConfigureAwait(false);

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
        var result = Array.Empty<TResult>();

        using (var transaction = new TransactionScope())
        {
            try
            {
                result = base.DispatchFunc(func, breakFunc, isTraversal);

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
        }

        return result;
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
        var result = Array.Empty<TResult>();

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                result = await base.DispatchFuncAsync(func, breakFunc, isTraversal, cancellationToken).ConfigureAwait(false);

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
        }

        return result;
    }

    #endregion

}

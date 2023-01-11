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
/// 继承自 <see cref="BaseDispatcher{TSource}"/> 的分布式事务遍历调度器（依次执行多个来源）。
/// </summary>
/// <remarks>
/// 说明：依次调用每个来源的指定成员处理动作或方法。当处理发生异常，将自行回滚之前的所有处理，并捕获 <see cref="TransactionAbortedException"/> 异常进行处理。
/// </remarks>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class TransactionTraversalDispatcher<TSource> : BaseDispatcher<TSource>
{
    /// <summary>
    /// 构造一个 <see cref="TransactionTraversalDispatcher{TSource}"/> 实例。
    /// </summary>
    /// <param name="sources">给定的 <typeparamref name="TSource"/> 集合。</param>
    /// <param name="options">给定的 <see cref="DispatcherOptions"/>。</param>
    public TransactionTraversalDispatcher(IEnumerable<TSource> sources, DispatcherOptions options)
        : base(sources, options)
    {
    }


    /// <summary>
    /// 事务中断的错误动作。
    /// </summary>
    public Action<IDispatcher<TSource>, Exception>? TransactionAbortedErrorAction { get; set; }


    #region Invoke

    /// <summary>
    /// 调用来源。
    /// </summary>
    /// <param name="action">给定的调用动作。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否遍历所有来源集合（可选；默认启用遍历）。</param>
    public override void InvokeAction(Action<IDispatcher<TSource>> action,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                base.InvokeAction(action, breakFunc, isTraversal);

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
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
    public override IEnumerable<TResult> InvokeFunc<TResult>(Func<IDispatcher<TSource>, TResult> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true)
    {
        IEnumerable<TResult> result = Enumerable.Empty<TResult>();

        using (var transaction = new TransactionScope())
        {
            try
            { 
                result = base.InvokeFunc(func, breakFunc, isTraversal);

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


    #region InvokeAsync

    /// <summary>
    /// 异步调用来源。
    /// </summary>
    /// <param name="func">给定的调用方法。</param>
    /// <param name="breakFunc">给定需要强制跳出循环的方法（可选；默认不强制跳出）。</param>
    /// <param name="isTraversal">是否对来源集合进行遍历（可选；默认启用遍历）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public override async Task InvokeActionAsync(Func<IDispatcher<TSource>, Task> func,
        Func<IDispatcher<TSource>, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                await base.InvokeActionAsync(func, breakFunc, isTraversal);

                transaction.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                TransactionAbortedErrorAction?.Invoke(this, ex);
            }
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
    public override async Task<IEnumerable<TResult>> InvokeFuncAsync<TResult>(
        Func<IDispatcher<TSource>, Task<TResult>> func,
        Func<IDispatcher<TSource>, TResult, bool>? breakFunc = null, bool isTraversal = true,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<TResult> result = Enumerable.Empty<TResult>();

        using (var transaction = new TransactionScope())
        {
            try
            {
                result = await base.InvokeFuncAsync(func, breakFunc, isTraversal);

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

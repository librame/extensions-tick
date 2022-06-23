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
/// 继承自 <see cref="BaseDispatcher{TSource}"/> 的分布式事务遍历均衡器（依次执行多个来源）。
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
    /// <param name="retries">给定所有来源集合的遍历重试次数（可选；默认 1 次）。</param>
    /// <param name="retryInterval">给定的单次重试间隔（将所有来源集合循环 1 遍为 1 次。可选；默认 1 秒）。</param>
    public TransactionTraversalDispatcher(IEnumerable<TSource> sources,
        int retries = 1, TimeSpan? retryInterval = null)
        : base(sources, retries, retryInterval, isTraversal: true)
    {
    }


    /// <summary>
    /// 事务中断的错误动作。
    /// </summary>
    public Action<IDispatcher<TSource>, Exception>? TransactionAbortedErrorAction { get; set; }


    /// <summary>
    /// 调用指定的方法。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    public override void Invoke(Action<TSource> action)
    {
        try
        {
            using (var transaction = new TransactionScope())
            {
               base.Invoke(action);

                transaction.Complete();
            }
        }
        catch (TransactionAbortedException ex)
        {
            TransactionAbortedErrorAction?.Invoke(this, ex);
        }
    }

    /// <summary>
    /// 调用指定的方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    public override IEnumerable<TResult> Invoke<TResult>(Func<TSource, TResult> func)
    {
        IEnumerable<TResult> result = Enumerable.Empty<TResult>();

        try
        {
            using (var transaction = new TransactionScope())
            {
                result = base.Invoke(func);

                transaction.Complete();
            }
        }
        catch (TransactionAbortedException ex)
        {
            TransactionAbortedErrorAction?.Invoke(this, ex);
        }

        return result;
    }

}

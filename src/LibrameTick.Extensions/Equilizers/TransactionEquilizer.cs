#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Equilizers;

/// <summary>
/// 继承自 <see cref="AbstractEquilizer{TSource}"/> 的分布式事务均衡器（依次执行多个来源）。
/// </summary>
/// <remarks>
/// 说明：依次调用每个来源的指定成员处理动作或方法。当处理发生异常，将自行回滚之前的所有处理，并抛出 <see cref="TransactionAbortedException"/> 异常。
/// </remarks>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class TransactionEquilizer<TSource> : AbstractEquilizer<TSource>
{
    /// <summary>
    /// 构造一个 <see cref="ExceptionEquilizer{TSource}"/> 实例。
    /// </summary>
    /// <param name="sources">给定的 <typeparamref name="TSource"/> 集合。</param>
    public TransactionEquilizer(IEnumerable<TSource> sources)
        : base(sources)
    {
    }


    /// <summary>
    /// 调用指定的方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    /// <exception cref="TransactionAbortedException"/>
    public override IEnumerable<TResult> Invoke<TResult>(Func<TSource, TResult> func)
    {
        var results = new List<TResult>();

        try
        {
            using (var transaction = new TransactionScope())
            {
                foreach (var source in Sources)
                {
                    results.Add(func(source));
                }

                transaction.Complete();
            }
        }
        catch (TransactionAbortedException)
        {
            throw;
        }

        return results;
    }


    /// <summary>
    /// 获取泛型枚举器。
    /// </summary>
    /// <returns>返回 <see cref="IEnumerator{T}"/>。</returns>
    public override IEnumerator<TSource> GetEnumerator()
        => Sources.GetEnumerator();

}

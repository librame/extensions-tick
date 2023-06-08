#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义实现 <see cref="ICounting{TCount}"/> 的泛型计数聚合接口。
/// </summary>
/// <typeparam name="TAggreg">指定的计数聚合类型。</typeparam>
/// <typeparam name="TCount">指定的计数类型。</typeparam>
public interface ICountingAggregation<TAggreg, TCount> : ICounting<TCount>
    where TAggreg : class, ICountingAggregation<TAggreg, TCount>
    where TCount : IAdditionOperators<TCount, TCount, TCount>, ISubtractionOperators<TCount, TCount, TCount>
        , IDecrementOperators<TCount>, IIncrementOperators<TCount>
{
    /// <summary>
    /// 当前聚合实例。
    /// </summary>
    TAggreg CurrentAggregation
        => (this as TAggreg)!;


    /// <summary>
    /// 递减属性计数。
    /// </summary>
    /// <param name="property">给定要递减的属性计数表达式。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <returns>返回 <typeparamref name="TCount"/>。</returns>
    TCount Decremental(Expression<Func<TAggreg, TCount>> property, TCount? decrement)
        => DecrementalCount(property.Compile()(CurrentAggregation), decrement);

    /// <summary>
    /// 异步递减属性计数。
    /// </summary>
    /// <param name="property">给定要递减的属性计数表达式。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
    ValueTask<TCount> DecrementalAsync(Expression<Func<TAggreg, TCount>> property,
        TCount? decrement, CancellationToken cancellationToken = default)
        => DecrementalCountAsync(property.Compile()(CurrentAggregation), decrement, cancellationToken);


    /// <summary>
    /// 递加属性计数。
    /// </summary>
    /// <param name="property">给定要递加的属性计数表达式。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <returns>返回 <typeparamref name="TCount"/>。</returns>
    TCount Incremental(Expression<Func<TAggreg, TCount>> property, TCount? increment)
        => IncrementalCount(property.Compile()(CurrentAggregation), increment);

    /// <summary>
    /// 异步递加属性计数。
    /// </summary>
    /// <param name="property">给定要递加的属性计数表达式。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
    ValueTask<TCount> IncrementalAsync(Expression<Func<TAggreg, TCount>> property,
        TCount? increment, CancellationToken cancellationToken = default)
        => IncrementalCountAsync(property.Compile()(CurrentAggregation), increment, cancellationToken);
}

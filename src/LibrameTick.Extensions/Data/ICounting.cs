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
/// 定义实现 <see cref="IObjectCounting"/> 的泛型计数接口。
/// </summary>
/// <typeparam name="TCount">指定的计数类型。</typeparam>
public interface ICounting<TCount> : IObjectCounting
    where TCount : IAdditionOperators<TCount, TCount, TCount>, ISubtractionOperators<TCount, TCount, TCount>
        , IDecrementOperators<TCount>, IIncrementOperators<TCount>
{
    /// <summary>
    /// 递减计数。
    /// </summary>
    /// <param name="count">给定要递减的计数。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <returns>返回 <typeparamref name="TCount"/>。</returns>
    TCount DecrementalCount(TCount count, TCount? decrement)
        => decrement is null ? --count : count -= decrement;

    /// <summary>
    /// 异步递减计数。
    /// </summary>
    /// <param name="count">给定要递减的计数。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
    ValueTask<TCount> DecrementalCountAsync(TCount count, TCount? decrement, CancellationToken cancellationToken = default)
        => cancellationToken.SimpleValueTask(decrement is null ? --count : count -= decrement);


    /// <summary>
    /// 递加计数。
    /// </summary>
    /// <param name="count">给定要递加的计数。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <returns>返回 <typeparamref name="TCount"/>。</returns>
    TCount IncrementalCount(TCount count, TCount? increment)
        => increment is null ? ++count : count += increment;

    /// <summary>
    /// 异步递加计数。
    /// </summary>
    /// <param name="count">给定要递加的计数。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
    ValueTask<TCount> IncrementalCountAsync(TCount count, TCount? increment, CancellationToken cancellationToken = default)
        => cancellationToken.SimpleValueTask(increment is null ? ++count : count += increment);


    /// <summary>
    /// 转换为计数。
    /// </summary>
    /// <param name="count">给定的计数对象。</param>
    /// <param name="paramName">给定的参数名（可选；默认为 <paramref name="count"/> 调用参数名）。</param>
    /// <returns>返回 <typeparamref name="TCount"/>。</returns>
    TCount ToCount(object count, [CallerArgumentExpression(nameof(count))] string? paramName = null)
        => count.As<TCount>(paramName);


    #region IObjectCounting

    /// <summary>
    /// 计数类型。
    /// </summary>
    [NotMapped]
    Type IObjectCounting.CountType
        => typeof(TCount);


    /// <summary>
    /// 递减对象计数。
    /// </summary>
    /// <param name="count">给定要递减的计数对象。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <returns>返回对象。</returns>
    object IObjectCounting.DecrementalObjectCount(object count, object? decrement)
        => DecrementalCount(ToCount(count), (TCount?)decrement);

    /// <summary>
    /// 异步递减对象计数。
    /// </summary>
    /// <param name="count">给定要递减的计数对象。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
    ValueTask<object> IObjectCounting.DecrementalObjectCountAsync(object count, object? decrement,
        CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTask(DecrementalObjectCount(count, decrement));


    /// <summary>
    /// 递加对象计数。
    /// </summary>
    /// <param name="count">给定要递加的对象计数对象。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <returns>返回对象。</returns>
    object IObjectCounting.IncrementalObjectCount(object count, object? increment)
        => IncrementalCount(ToCount(count), (TCount?)increment);

    /// <summary>
    /// 异步递加对象计数。
    /// </summary>
    /// <param name="count">给定要递加的对象计数对象。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
    ValueTask<object> IObjectCounting.IncrementalObjectCountAsync(object count, object? increment,
        CancellationToken cancellationToken)
        => cancellationToken.SimpleValueTask(IncrementalObjectCount(count, increment));

    #endregion

}

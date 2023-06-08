#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// <see cref="Maybe{T}"/> 静态扩展。
/// </summary>
public static class MaybeExtensions
{

    /// <summary>
    /// 转为值列表。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="maybe">给定的 <see cref="Maybe{T}"/>。</param>
    /// <returns>返回 <see cref="List{T}"/>。</returns>
    public static List<T> ToList<T>(this Maybe<T> maybe)
        => maybe.GetValueOrDefault(static value => new List<T> { value }, new List<T>());

    /// <summary>
    /// 绑定。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    /// <param name="maybe">给定的 <see cref="Maybe{TSource}"/>。</param>
    /// <param name="func">给定的绑定方法。</param>
    /// <returns>返回 <see cref="Maybe{TTarget}"/>。</returns>
    public static Maybe<TTarget> Bind<TSource, TTarget>(this Maybe<TSource> maybe,
        Func<Maybe<TSource>, Maybe<TTarget>> func)
    {
        if (maybe.HasNoValue)
            return Maybe<TTarget>.None;

        return func(maybe.GetValueOrThrow());
    }

    /// <summary>
    /// 映射。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <typeparam name="TTarget">指定的目标类型。</typeparam>
    /// <param name="maybe">给定的 <see cref="Maybe{TSource}"/>。</param>
    /// <param name="func">给定的映射方法。</param>
    /// <returns>返回 <see cref="Maybe{TTarget}"/>。</returns>
    public static Maybe<TTarget> Map<TSource, TTarget>(this Maybe<TSource> maybe,
        Func<TSource, TTarget> func)
    {
        if (maybe.HasNoValue)
            return Maybe<TTarget>.None;

        return func(maybe.GetValueOrThrow());
    }

    /// <summary>
    /// 筛选。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="maybe">给定的 <see cref="Maybe{T}"/>。</param>
    /// <param name="predicate">给定的断定方法。</param>
    /// <returns>如果满足断定方法则返回当前 <paramref name="maybe"/>，反之则返回 <see cref="Maybe{T}.None"/>。</returns>
    public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate)
    {
        if (maybe.HasNoValue)
            return Maybe<T>.None;

        if (predicate(maybe.GetValueOrThrow()))
            return maybe;

        return Maybe<T>.None;
    }

}

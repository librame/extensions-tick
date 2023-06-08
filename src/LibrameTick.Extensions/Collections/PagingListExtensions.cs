#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Collections;

/// <summary>
/// <see cref="PagingList{T}"/> 静态扩展。
/// </summary>
public static class PagingListExtensions
{

    /// <summary>
    /// 可枚举内存分页。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is null.
    /// </exception>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="collection">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public static IPagingList<T> AsPaging<T>(this IEnumerable<T> collection, Action<IPagingList<T>> pageAction)
    {
        var list = new PagingList<T>(collection);
        pageAction(list);

        return list;
    }

    /// <summary>
    /// 可查询分页。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="queryable"/> is null.
    /// </exception>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public static IPagingList<T> AsPaging<T>(this IQueryable<T> queryable, Action<IPagingList<T>> pageAction)
    {
        var list = new PagingList<T>(queryable);
        pageAction(list);

        return list;
    }


    /// <summary>
    /// 异步可枚举内存分页。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> is null.
    /// </exception>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="collection">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public static Task<IPagingList<T>> AsPagingAsync<T>(this IEnumerable<T> collection, Action<IPagingList<T>> pageAction,
        CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(() => collection.AsPaging(pageAction));

    /// <summary>
    /// 异步可查询分页。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="queryable"/> is null.
    /// </exception>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="queryable">给定的 <see cref="IQueryable{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public static Task<IPagingList<T>> AsPagingAsync<T>(this IQueryable<T> queryable, Action<IPagingList<T>> pageAction,
        CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(() => queryable.AsPaging(pageAction));


    /// <summary>
    /// 复合分页。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="queryables">给定的 <see cref="IQueryable{T}"/> 可枚举集合。</param>
    /// <returns>返回复合的 <see cref="IPagingList{T}"/>。</returns>
    public static IPagingList<T> CompositePaging<T>(this IEnumerable<IPagingList<T>> queryables)
        => new CompositePagingList<T>(queryables);

    /// <summary>
    /// 异步复合分页。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="queryables">给定的 <see cref="IQueryable{T}"/> 可枚举集合。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回复合的 <see cref="IPagingList{T}"/> 异步操作。</returns>
    public static Task<IPagingList<T>> CompositePagingAsync<T>(this IEnumerable<IPagingList<T>> queryables,
        CancellationToken cancellationToken = default)
        => cancellationToken.SimpleTask(() => queryables.CompositePaging());

}

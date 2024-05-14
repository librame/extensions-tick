#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data;

namespace Librame.Extensions.Collections;

/// <summary>
/// 定义集合静态扩展。
/// </summary>
public static class CollectionsExtensions
{

    #region PagingList

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

    #endregion


    #region TreeingList

    /// <summary>
    /// 将已实现 <see cref="IParentIdentifier{TId}"/> 的元素可枚举集合转换为树形列表。
    /// </summary>
    /// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/> 的元素类型。</typeparam>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="items">给定的类型实例集合。</param>
    /// <returns>返回 <see cref="ITreeingList{TItem, TId}"/>。</returns>
    public static ITreeingList<TItem, TId> AsTreeing<TItem, TId>(this IEnumerable<TItem> items)
        where TItem : IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        // 提取根父标识
        var rootParentId = items.Select(static s => s.ParentId).Min();

        return new TreeingList<TItem, TId>(LookupNodes(items, rootParentId));
    }

    /// <summary>
    /// 异步将已实现 <see cref="IParentIdentifier{TId}"/> 的元素可枚举集合转换为树形列表。
    /// </summary>
    /// <typeparam name="TItem">指定实现 <see cref="IParentIdentifier{TId}"/> 的元素类型。</typeparam>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="items">给定的类型实例集合。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含 <see cref="ITreeingList{TItem, TId}"/> 的异步操作。</returns>
    public static Task<ITreeingList<TItem, TId>> AsTreeingAsync<TItem, TId>(this IEnumerable<TItem> items,
        CancellationToken cancellationToken = default)
        where TItem : IParentIdentifier<TId>
        where TId : IEquatable<TId>
        => cancellationToken.SimpleTask(items.AsTreeing<TItem, TId>);


    private static List<TreeingNode<TItem, TId>> LookupNodes<TItem, TId>(IEnumerable<TItem> items,
        TId? currentParentId, int currentHierarchy = 0)
        where TItem : IParentIdentifier<TId>
        where TId : IEquatable<TId>
    {
        var nodes = new List<TreeingNode<TItem, TId>>();

        // 提取当前父标识的父元素集合
        List<TItem> parents;

        if (currentParentId is null)
            parents = items.Where(static p => p.ParentId is null).ToList();
        else
            parents = items.Where(p => p.ParentId is not null && p.ParentId.Equals(currentParentId)).ToList();

        if (parents.Count < 1)
            return nodes;

        foreach (var parent in parents)
        {
            var parentHierarchy = currentHierarchy;

            var children = LookupNodes(items, parent.Id, currentHierarchy++);

            nodes.Add(new TreeingNode<TItem, TId>(parent, children, parentHierarchy));
        }

        return nodes;
    }

    #endregion

}

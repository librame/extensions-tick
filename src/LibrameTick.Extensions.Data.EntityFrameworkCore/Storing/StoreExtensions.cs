#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Collections;
using Librame.Extensions.Data.Specification;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// <see cref="IStore{T}"/> 静态扩展。
/// </summary>
public static class StoreExtensions
{

    #region Find

    /// <summary>
    /// 通过指定断定条件查找类型实例集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    public static IList<T> FindList<T>(this IStore<T> store, Expression<Func<T, bool>>? predicate = null,
        int? group = null, bool fromWriteAccessor = false)
        where T : class
    {
        var query = store.GetQueryable(group, fromWriteAccessor);

        if (predicate is not null)
            query = query.Where(predicate);

        return query.ToList();
    }

    /// <summary>
    /// 异步通过指定断定条件查找类型实例集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    public static Task<IList<T>> FindListAsync<T>(this IStore<T> store,
        Expression<Func<T, bool>>? predicate = null, int? group = null, bool fromWriteAccessor = false,
        CancellationToken cancellationToken = default)
        where T : class
        => cancellationToken.RunTask(() => store.FindList(predicate, group, fromWriteAccessor));


    /// <summary>
    /// 查找带有规约的类型实例集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    public static IList<T> FindListWithSpecification<T>(this IStore<T> store,
        ISpecification<T>? specification = null, int? group = null, bool fromWriteAccessor = false)
        where T : class
        => store.GetAccessor(group, fromWriteAccessor).FindListWithSpecification(specification);

    /// <summary>
    /// 异步查找带有规约的类型实例集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    public static Task<IList<T>> FindListWithSpecificationAsync<T>(this IStore<T> store,
        ISpecification<T>? specification = null, int? group = null, bool fromWriteAccessor = false,
        CancellationToken cancellationToken = default)
        where T : class
        => store.GetAccessor(group, fromWriteAccessor)
            .FindListWithSpecificationAsync(specification, cancellationToken);


    /// <summary>
    /// 查找类型实例分页集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public static IPagingList<T> FindPagingList<T>(this IStore<T> store, Action<IPagingList<T>> pageAction,
        int? group = null, bool fromWriteAccessor = false)
        where T : class
        => store.GetAccessor(group, fromWriteAccessor).FindPagingList(pageAction);

    /// <summary>
    /// 异步查找类型实例分页集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public static Task<IPagingList<T>> FindPagingListAsync<T>(this IStore<T> store,
        Action<IPagingList<T>> pageAction, int? group = null, bool fromWriteAccessor = false,
        CancellationToken cancellationToken = default)
        where T : class
        => store.GetAccessor(group, fromWriteAccessor).FindPagingListAsync(pageAction, cancellationToken);


    /// <summary>
    /// 查找带有规约的类型实例分页集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public static IPagingList<T> FindPagingListWithSpecification<T>(this IStore<T> store,
        Action<IPagingList<T>> pageAction, ISpecification<T>? specification = null, int? group = null,
        bool fromWriteAccessor = false)
        where T : class
        => store.GetAccessor(group, fromWriteAccessor)
            .FindPagingListWithSpecification(pageAction, specification);

    /// <summary>
    /// 异步查找带有规约的类型实例分页集合（支持强制从写入访问器查询）。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    /// <param name="store">给定的 <see cref="IStore{T}"/>。</param>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="group">给定的所属群组（可选；默认使用初始访问器）。</param>
    /// <param name="fromWriteAccessor">强制从写入访问器查询（可选；默认不强制）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public static Task<IPagingList<T>> FindPagingListWithSpecificationAsync<T>(this IStore<T> store,
        Action<IPagingList<T>> pageAction, ISpecification<T>? specification = null, int? group = null,
        bool fromWriteAccessor = false, CancellationToken cancellationToken = default)
        where T : class
        => store.GetAccessor(group, fromWriteAccessor)
            .FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken);

    #endregion

}

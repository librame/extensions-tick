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
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Specifications;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// 定义泛型商店接口。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public interface IStore<T>
{
    /// <summary>
    /// <see cref="IAccessor"/> 管理器。
    /// </summary>
    IAccessorManager Accessors { get; }

    /// <summary>
    /// <see cref="IIdentificationGenerator{TId}"/> 工厂。
    /// </summary>
    IIdentificationGeneratorFactory IdGeneratorFactory { get; }

    /// <summary>
    /// 当前访问器。
    /// </summary>
    IAccessor? CurrentAccessor { get; }


    /// <summary>
    /// 获取访问器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetAccessor(IAccessorSpecification specification);

    /// <summary>
    /// 获取可查询接口（支持强制从写入访问器查询）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> GetQueryable(IAccessorSpecification? specification = null);


    #region Find

    /// <summary>
    /// 通过标识查找类型实例（支持强制从写入访问器查询）。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    T? FindById(object id, IAccessorSpecification? specification = null);


    /// <summary>
    /// 通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    IList<T> FindList(Expression<Func<T, bool>>? predicate = null,
        IAccessorSpecification? specification = null);

    /// <summary>
    /// 异步通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    Task<IList<T>> FindListAsync(Expression<Func<T, bool>>? predicate = null,
        IAccessorSpecification? specification = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// 查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    IList<T> FindListWithSpecification(IEntitySpecification<T>? entitySpecification = null,
        IAccessorSpecification? accessorSpecification = null);

    /// <summary>
    /// 异步查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    Task<IList<T>> FindListWithSpecificationAsync(IEntitySpecification<T>? entitySpecification = null,
        IAccessorSpecification? accessorSpecification = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// 查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    IPagingList<T> FindPagingList(Action<IPagingList<T>> pageAction,
        IAccessorSpecification? specification = null);

    /// <summary>
    /// 异步查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    Task<IPagingList<T>> FindPagingListAsync(Action<IPagingList<T>> pageAction,
        IAccessorSpecification? specification = null, CancellationToken cancellationToken = default);


    /// <summary>
    /// 查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    IPagingList<T> FindPagingListWithSpecification(Action<IPagingList<T>> pageAction,
        IEntitySpecification<T>? entitySpecification = null, IAccessorSpecification? accessorSpecification = null);

    /// <summary>
    /// 异步查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    Task<IPagingList<T>> FindPagingListWithSpecificationAsync(Action<IPagingList<T>> pageAction,
        IEntitySpecification<T>? entitySpecification = null, IAccessorSpecification? accessorSpecification = null,
        CancellationToken cancellationToken = default);

    #endregion


    #region Add

    /// <summary>
    /// 如果不存在则添加类型实例（仅支持写入访问器）。
    /// </summary>
    /// <param name="item">给定要添加的类型实例。</param>
    /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    void AddIfNotExists(T item, Expression<Func<T, bool>> predicate, IAccessorSpecification? specification = null);

    /// <summary>
    /// 添加类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="items">给定的类型实例数组集合。</param>
    void Add(IAccessorSpecification? specification = null, params T[] items);

    /// <summary>
    /// 添加类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    void Add(IEnumerable<T> items, IAccessorSpecification? specification = null);

    #endregion


    #region Remove

    /// <summary>
    /// 移除类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="items">给定的类型实例数组集合。</param>
    void Remove(IAccessorSpecification? specification = null, params T[] items);

    /// <summary>
    /// 移除类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    void Remove(IEnumerable<T> items, IAccessorSpecification? specification = null);

    #endregion


    #region Update

    /// <summary>
    /// 更新类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="items">给定的类型实例数组集合。</param>
    void Update(IAccessorSpecification? specification = null, params T[] items);

    /// <summary>
    /// 更新类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    void Update(IEnumerable<T> items, IAccessorSpecification? specification = null);

    #endregion


    #region SaveChanges

    /// <summary>
    /// 保存更改（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <returns>返回受影响的行数。</returns>
    int SaveChanges(IAccessorSpecification? specification = null);

    /// <summary>
    /// 异步保存更改（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    Task<int> SaveChangesAsync(IAccessorSpecification? specification = null,
        CancellationToken cancellationToken = default);

    #endregion

}

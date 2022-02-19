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
/// 定义实现 <see cref="IStore{T}"/> 的泛型基础商店。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class BaseStore<T> : IStore<T>
    where T : class
{
    /// <summary>
    /// 构造一个 <see cref="BaseStore{T}"/>。
    /// </summary>
    /// <param name="accessors">给定的 <see cref="IAccessorManager"/>。</param>
    /// <param name="idGeneratorFactory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
    public BaseStore(IAccessorManager accessors,
        IIdentificationGeneratorFactory idGeneratorFactory)
    {
        Accessors = accessors;
        IdGeneratorFactory = idGeneratorFactory;
    }


    /// <summary>
    /// <see cref="IAccessor"/> 管理器。
    /// </summary>
    public IAccessorManager Accessors { get; init; }

    /// <summary>
    /// <see cref="IIdentificationGenerator{TId}"/> 工厂。
    /// </summary>
    public IIdentificationGeneratorFactory IdGeneratorFactory { get; init; }

    /// <summary>
    /// 当前访问器。
    /// </summary>
    public IAccessor? CurrentAccessor { get; private set; }


    /// <summary>
    /// 获取访问器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public virtual IAccessor GetAccessor(IAccessorSpecification specification)
        => Accessors.GetAccessor(specification);

    /// <summary>
    /// 获取可查询接口。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    public virtual IQueryable<T> GetQueryable(IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetReadAccessor(specification)).GetQueryable<T>();


    #region Find

    /// <summary>
    /// 通过标识查找类型实例。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    public virtual T? FindById(object id, IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetReadAccessor(specification)).Find<T>(id);


    /// <summary>
    /// 通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    public virtual IList<T> FindList(Expression<Func<T, bool>>? predicate = null,
        IAccessorSpecification? specification = null)
    {
        var query = GetQueryable(specification);

        if (predicate is not null)
            query = query.Where(predicate);

        return query.ToList();
    }

    /// <summary>
    /// 异步通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    public virtual Task<IList<T>> FindListAsync(Expression<Func<T, bool>>? predicate = null,
        IAccessorSpecification? specification = null, CancellationToken cancellationToken = default)
        => cancellationToken.RunTask(() => FindList(predicate, specification));


    /// <summary>
    /// 查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    public virtual IList<T> FindListWithSpecification(IEntitySpecification<T>? entitySpecification = null,
        IAccessorSpecification? accessorSpecification = null)
        => (CurrentAccessor = Accessors.GetReadAccessor(accessorSpecification))
            .FindListWithSpecification(entitySpecification);

    /// <summary>
    /// 异步查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    public virtual Task<IList<T>> FindListWithSpecificationAsync(IEntitySpecification<T>? entitySpecification = null,
        IAccessorSpecification? accessorSpecification = null, CancellationToken cancellationToken = default)
        => (CurrentAccessor = Accessors.GetReadAccessor(accessorSpecification))
            .FindListWithSpecificationAsync(entitySpecification, cancellationToken);


    /// <summary>
    /// 查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public virtual IPagingList<T> FindPagingList(Action<IPagingList<T>> pageAction,
        IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetReadAccessor(specification)).FindPagingList(pageAction);

    /// <summary>
    /// 异步查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public virtual Task<IPagingList<T>> FindPagingListAsync(Action<IPagingList<T>> pageAction,
        IAccessorSpecification? specification = null, CancellationToken cancellationToken = default)
        => (CurrentAccessor = Accessors.GetReadAccessor(specification))
            .FindPagingListAsync(pageAction, cancellationToken);


    /// <summary>
    /// 查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    public virtual IPagingList<T> FindPagingListWithSpecification(Action<IPagingList<T>> pageAction,
        IEntitySpecification<T>? entitySpecification = null, IAccessorSpecification? accessorSpecification = null)
        => (CurrentAccessor = Accessors.GetReadAccessor(accessorSpecification))
            .FindPagingListWithSpecification(pageAction, entitySpecification);

    /// <summary>
    /// 异步查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="IEntitySpecification{T}"/>（可选）。</param>
    /// <param name="accessorSpecification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    public virtual Task<IPagingList<T>> FindPagingListWithSpecificationAsync(Action<IPagingList<T>> pageAction,
        IEntitySpecification<T>? entitySpecification = null, IAccessorSpecification? accessorSpecification = null,
        CancellationToken cancellationToken = default)
        => (CurrentAccessor = Accessors.GetReadAccessor(accessorSpecification))
            .FindPagingListWithSpecificationAsync(pageAction, entitySpecification, cancellationToken);

    #endregion


    #region Add

    /// <summary>
    /// 如果不存在则添加类型实例（仅支持写入访问器）。
    /// </summary>
    /// <param name="item">给定要添加的类型实例。</param>
    /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    public virtual void AddIfNotExists(T item, Expression<Func<T, bool>> predicate,
        IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).AddIfNotExists(item, predicate); // 使用元素对象作为分库依据

    /// <summary>
    /// 添加类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="entities">给定的类型实例数组集合。</param>
    public virtual void Add(IAccessorSpecification? specification = null, params T[] entities)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).AddRange(entities);

    /// <summary>
    /// 添加类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    public virtual void Add(IEnumerable<T> entities, IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).AddRange(entities);

    #endregion


    #region Remove

    /// <summary>
    /// 移除类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="entities">给定的类型实例数组集合。</param>
    public virtual void Remove(IAccessorSpecification? specification = null, params T[] entities)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).RemoveRange(entities);

    /// <summary>
    /// 移除类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    public virtual void Remove(IEnumerable<T> entities, IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).RemoveRange(entities);

    #endregion


    #region Update

    /// <summary>
    /// 更新类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="entities">给定的类型实例数组集合。</param>
    public virtual void Update(IAccessorSpecification? specification = null, params T[] entities)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).UpdateRange(entities);

    /// <summary>
    /// 更新类型实例集合（仅支持写入访问器）。
    /// </summary>
    /// <param name="entities">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    public virtual void Update(IEnumerable<T> entities, IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).UpdateRange(entities);

    #endregion


    #region SaveChanges

    /// <summary>
    /// 保存更改（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <returns>返回受影响的行数。</returns>
    public virtual int SaveChanges(IAccessorSpecification? specification = null)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).SaveChanges();

    /// <summary>
    /// 异步保存更改（仅支持写入访问器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    public virtual Task<int> SaveChangesAsync(IAccessorSpecification? specification = null,
        CancellationToken cancellationToken = default)
        => (CurrentAccessor = Accessors.GetWriteAccessor(specification)).SaveChangesAsync(cancellationToken);

    #endregion

}

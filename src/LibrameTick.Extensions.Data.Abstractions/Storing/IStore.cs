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
using Librame.Extensions.Dispatchers;
using Librame.Extensions.IdGenerators;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Storing;

/// <summary>
/// 定义泛型存储接口。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public interface IStore<T>
{
    /// <summary>
    /// <see cref="IAccessor"/> 上下文。
    /// </summary>
    IAccessorContext AccessorContext { get; }

    /// <summary>
    /// <see cref="IIdGenerator{TId}"/> 工厂。
    /// </summary>
    IIdGeneratorFactory IdGeneratorFactory { get; }

    /// <summary>
    /// 当前读取调度器存取器。
    /// </summary>
    IDispatcherAccessors CurrentReadAccessor { get; set; }

    /// <summary>
    /// 当前写入调度器存取器。
    /// </summary>
    IDispatcherAccessors CurrentWriteAccessor { get; set; }


    /// <summary>
    /// 使用读取与写入访问器。
    /// </summary>
    /// <param name="accessorName">给定的 <see cref="IAccessor"/> 名称。</param>
    /// <returns>返回 <see cref="IStore{T}"/>。</returns>
    IStore<T> UseAccessor(string accessorName);

    /// <summary>
    /// 使用读取访问器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IStore{T}"/>。</returns>
    IStore<T> UseReadAccessor(ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 使用写入访问器。77
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IStore{T}"/>。</returns>
    IStore<T> UseWriteAccessor(ISpecification<IAccessor>? specification = null);


    #region Query

    /// <summary>
    /// 获取可查询接口。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> Query(ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 获取可查询接口。
    /// </summary>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> Query(string name, ISpecification<IAccessor>? specification = null);


    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> QueryBySql(string sql, params object[] parameters);

    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> QueryBySql(string sql, object[] parameters,
        ISpecification<IAccessor> specification);


    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> QueryBySql(string name, string sql, params object[] parameters);

    /// <summary>
    /// 通过 SQL 语句获取可查询接口。
    /// </summary>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IQueryable{T}"/>。</returns>
    IQueryable<T> QueryBySql(string name, string sql, object[] parameters,
        ISpecification<IAccessor> specification);

    #endregion


    #region Exists

    /// <summary>
    /// 在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回布尔值。</returns>
    bool Exists(Expression<Func<T, bool>> predicate, bool checkLocal = true,
        ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 异步在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default,
        ISpecification<IAccessor>? specification = null);

    #endregion


    #region Find

    /// <summary>
    /// 通过标识查找类型实例。
    /// </summary>
    /// <param name="id">给定的标识。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    T? FindById(object id, ISpecification<IAccessor>? specification = null);


    /// <summary>
    /// 通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    IList<T>? FindList(Expression<Func<T, bool>>? predicate = null,
        ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 异步通过指定断定条件查找类型实例集合。
    /// </summary>
    /// <param name="predicate">给定的断定条件（可选；为空表示查询所有）</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    Task<IList<T>?> FindListAsync(Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default, ISpecification<IAccessor>? specification = null);


    /// <summary>
    /// 查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IList{T}"/>。</returns>
    IList<T>? FindListWithSpecification(ISpecification<T>? entitySpecification = null,
        ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 异步查找带有规约的类型实例集合。
    /// </summary>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IList{T}"/> 的异步操作。</returns>
    Task<IList<T>?> FindListWithSpecificationAsync(ISpecification<T>? entitySpecification = null,
        CancellationToken cancellationToken = default, ISpecification<IAccessor>? specification = null);


    /// <summary>
    /// 查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    IPagingList<T>? FindPagingList(Action<IPagingList<T>> pageAction,
        ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 异步查找类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    Task<IPagingList<T>?> FindPagingListAsync(Action<IPagingList<T>> pageAction,
        CancellationToken cancellationToken = default, ISpecification<IAccessor>? specification = null);


    /// <summary>
    /// 查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IPagingList{T}"/>。</returns>
    IPagingList<T>? FindPagingListWithSpecification(Action<IPagingList<T>> pageAction,
        ISpecification<T>? entitySpecification = null, ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 异步查找带有规约的类型实例分页集合。
    /// </summary>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="entitySpecification">给定的 <see cref="ISpecification{T}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{T}"/> 的异步操作。</returns>
    Task<IPagingList<T>?> FindPagingListWithSpecificationAsync(Action<IPagingList<T>> pageAction,
        ISpecification<T>? entitySpecification = null, CancellationToken cancellationToken = default,
        ISpecification<IAccessor>? specification = null);

    #endregion


    #region Add

    /// <summary>
    /// 如果不存在则添加类型实例（仅支持写入存取器）。
    /// </summary>
    /// <param name="item">给定要添加的类型实例。</param>
    /// <param name="predicate">给定用于判定是否存在的工厂方法。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    void AddIfNotExists(T item, Expression<Func<T, bool>> predicate,
        ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 添加类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="items">给定的类型实例数组集合。</param>
    void Add(params T[] items);

    /// <summary>
    /// 添加类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    void Add(IEnumerable<T> items, ISpecification<IAccessor>? specification = null);

    #endregion


    #region Remove

    /// <summary>
    /// 移除类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="items">给定的类型实例数组集合。</param>
    void Remove(params T[] items);

    /// <summary>
    /// 移除类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    void Remove(IEnumerable<T> items, ISpecification<IAccessor>? specification = null);

    #endregion


    #region Update

    /// <summary>
    /// 更新类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="items">给定的类型实例数组集合。</param>
    void Update(params T[] items);

    /// <summary>
    /// 更新类型实例集合（仅支持写入存取器）。
    /// </summary>
    /// <param name="items">给定的 <see cref="IEnumerable{T}"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    void Update(IEnumerable<T> items, ISpecification<IAccessor>? specification = null);

    #endregion


    #region DirectExecute

    /// <summary>
    /// 直接删除，不通过跟踪实体实现。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定条件（可选；默认为空表示删除所有）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回受影响的行数。</returns>
    int DirectDelete<TEntity>(Expression<Func<TEntity, bool>>? predicate = null,
        ISpecification<IAccessor>? specification = null)
        where TEntity : class;

    /// <summary>
    /// 异步直接删除，不通过跟踪实体实现。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定条件（可选；默认为空表示删除所有）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    Task<int> DirectDeleteAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default, ISpecification<IAccessor>? specification = null)
        where TEntity : class;

    #endregion


    #region SaveChanges

    /// <summary>
    /// 保存更改（仅支持写入存取器；操作结束后将自行切换为读取存取器）。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回受影响的行数。</returns>
    int SaveChanges(ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 异步保存更改（仅支持写入存取器；操作结束后将自行切换为读取存取器）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default,
        ISpecification<IAccessor>? specification = null);

    #endregion

}

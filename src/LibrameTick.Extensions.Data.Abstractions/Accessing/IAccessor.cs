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
using Librame.Extensions.Core;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Data.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义表示数据访问的存取器接口（主要用于适配数据实现层的访问对象；如 EFCore 实现层的 DbContext 对象）。
/// </summary>
public interface IAccessor : IConnectable<IAccessor>, ISaveChangeable, ISortable, IShardable, IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 存取器标识。
    /// </summary>
    string AccessorId { get; }

    /// <summary>
    /// 存取器类型。
    /// </summary>
    Type AccessorType { get; }

    /// <summary>
    /// 存取器描述符。
    /// </summary>
    AccessorDescriptor? AccessorDescriptor { get; }


    #region Exists

    /// <summary>
    /// 在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回布尔值。</returns>
    bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true)
        where TEntity : class;

    /// <summary>
    /// 异步在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class;

    #endregion


    #region Find

    /// <summary>
    /// 查找与指定键值对集合匹配的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值对集合。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity? Find<TEntity>(params object?[]? keyValues)
        where TEntity : class;

    /// <summary>
    /// 查找与指定键值对集合匹配的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值对集合。</param>
    /// <returns>返回实体对象。</returns>
    object? Find(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// 异步查找与指定键值对集合匹配的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值对集合。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的 <see cref="ValueTask"/>。</returns>
    ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken)
        where TEntity : class;

    /// <summary>
    /// 异步查找与指定键值对集合匹配的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值对集合。</param>
    /// <returns>返回一个包含实体对象的 <see cref="ValueTask"/>。</returns>
    ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// 异步查找与指定键值对集合匹配的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值对集合。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>。</param>
    /// <returns>返回一个包含实体对象的 <see cref="ValueTask"/>。</returns>
    ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken);

    /// <summary>
    /// 异步查找与指定键值对集合匹配的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值对集合。</param>
    /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的 <see cref="ValueTask"/>。</returns>
    ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues)
        where TEntity : class;


    /// <summary>
    /// 查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    IList<TEntity> FindListWithSpecification<TEntity>(IEntitySpecification<TEntity>? specification = null)
        where TEntity : class;

    /// <summary>
    /// 异步查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{TEntity}"/> 的异步操作。</returns>
    Task<IList<TEntity>> FindListWithSpecificationAsync<TEntity>(IEntitySpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class;


    /// <summary>
    /// 查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class;

    /// <summary>
    /// 异步查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class;


    /// <summary>
    /// 查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class;

    /// <summary>
    /// 异步查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class;

    #endregion


    #region GetQueryable

    /// <summary>
    /// 从表达式建立指定结果的可查询接口。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="expression">给定的 <see cref="IQueryable{TResult}"/> 表达式。</param>
    /// <returns>返回 <see cref="IQueryable{TResult}"/>。</returns>
    IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression);


    /// <summary>
    /// 获取指定实体的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class;

    /// <summary>
    /// 获取指定实体的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    IQueryable<TEntity> GetQueryable<TEntity>(string name)
        where TEntity : class;

    #endregion


    #region Add

    /// <summary>
    /// 添加不存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity AddIfNotExists<TEntity>(TEntity entity,
        Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class;

    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">给定要添加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    object Add(object entity);

    /// <summary>
    /// 添加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity Add<TEntity>(TEntity entity)
        where TEntity : class;


    /// <summary>
    /// 添加实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    void AddRange(IEnumerable<object> entities);

    /// <summary>
    /// 添加实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    void AddRange(params object[] entities);

    /// <summary>
    /// 异步添加实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    /// <param name="cancellationToken"></param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    Task AddRangeAsync(IEnumerable<object> entities,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步添加实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    Task AddRangeAsync(params object[] entities);

    #endregion


    #region Attach

    /// <summary>
    /// 附加实体对象。
    /// </summary>
    /// <param name="entity">给定要附加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    object Attach(object entity);

    /// <summary>
    /// 附加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要附加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class;


    /// <summary>
    /// 附加实体范围集合。
    /// </summary>
    /// <param name="entities">给定要附加的实体对象集合。</param>
    void AttachRange(params object[] entities);

    /// <summary>
    /// 附加实体范围集合。
    /// </summary>
    /// <param name="entities">给定要附加的实体对象集合。</param>
    void AttachRange(IEnumerable<object> entities);

    #endregion


    #region Remove

    /// <summary>
    /// 移除实体对象。
    /// </summary>
    /// <param name="entity">给定要移除的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    object Remove(object entity);

    /// <summary>
    /// 移除实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体。</typeparam>
    /// <param name="entity">给定要移除的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class;


    /// <summary>
    /// 移除实体范围集合。
    /// </summary>
    /// <param name="entities">给定要移除的实体对象集合。</param>
    void RemoveRange(params object[] entities);

    /// <summary>
    /// 移除实体范围集合。
    /// </summary>
    /// <param name="entities">给定要移除的实体对象集合。</param>
    void RemoveRange(IEnumerable<object> entities);

    #endregion


    #region Update

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">给定要更新的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    object Update(object entity);

    /// <summary>
    /// 更新实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要更新的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity Update<TEntity>(TEntity entity)
        where TEntity : class;


    /// <summary>
    /// 更新实体范围集合。
    /// </summary>
    /// <param name="entities">给定要更新的实体对象集合。</param>
    void UpdateRange(params object[] entities);

    /// <summary>
    /// 更新实体范围集合。
    /// </summary>
    /// <param name="entities">给定要更新的实体对象集合。</param>
    void UpdateRange(IEnumerable<object> entities);

    #endregion

}

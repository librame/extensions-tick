#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个继承 <see cref="IDisposable"/> 与 <see cref="IAsyncDisposable"/> 的数据库上下文接口。
/// </summary>
public interface IDbContext : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// 上下文类型。
    /// </summary>
    Type ContextType { get; }

    /// <summary>
    /// 分片上下文。
    /// </summary>
    IShardingContext ShardingContext { get; }


    #region Find

    /// <summary>
    /// 从表达式建立指定结果的可查询接口。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="expression">给定的 <see cref="IQueryable{TResult}"/> 表达式。</param>
    /// <returns>返回 <see cref="IQueryable{TResult}"/>。</returns>
    IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression);


    /// <summary>
    /// 查找指定键值数组的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回实体对象。</returns>
    object? Find(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// 异步查找指定键值数组的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回一个包含实体对象的异步操作。</returns>
    ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues);

    /// <summary>
    /// 异步查找指定键值数组的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含实体对象的异步操作。</returns>
    ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues,
        CancellationToken cancellationToken);


    /// <summary>
    /// 查找指定键值数组的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    TEntity? Find<TEntity>(params object?[]? keyValues)
        where TEntity : class;

    /// <summary>
    /// 异步查找指定键值数组的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步结果。</returns>
    ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues)
        where TEntity : class;

    /// <summary>
    /// 异步查找指定键值数组的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步结果。</returns>
    ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues,
        CancellationToken cancellationToken)
        where TEntity : class;

    #endregion


    #region Add

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
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
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


    #region SaveChanges

    /// <summary>
    /// 保存更改。
    /// </summary>
    /// <returns>返回受影响的行数。</returns>
    int SaveChanges();

    /// <summary>
    /// 保存更改。
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
    /// <returns>返回受影响的行数。</returns>
    int SaveChanges(bool acceptAllChangesOnSuccess);


    /// <summary>
    /// 异步保存更改。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// 异步保存更改。
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default);

    #endregion

}

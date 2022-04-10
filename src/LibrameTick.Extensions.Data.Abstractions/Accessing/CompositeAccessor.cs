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
using Librame.Extensions.Equilizers;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个表示数据访问集合的复合存取器（支持针对读取异常切换与写入事务遍历等功能）。
/// </summary>
public class CompositeAccessor : AbstractSortable, IAccessor
{
    /// <summary>
    /// 构造一个 <see cref="CompositeAccessor"/>。
    /// </summary>
    /// <param name="accessors">给定要复合的 <see cref="IEnumerable{IAccessor}"/>。</param>
    public CompositeAccessor(IEnumerable<IAccessor> accessors)
    {
        SwitchingEquilizer = new(accessors);
        TraversalEquilizer = new(accessors);
    }


    /// <summary>
    /// 异常切换均衡器。
    /// </summary>
    public ExceptionSwitchingEquilizer<IAccessor> SwitchingEquilizer { get; init; }

    /// <summary>
    /// 分布式事务遍历均衡器。
    /// </summary>
    public TransactionTraversalEquilizer<IAccessor> TraversalEquilizer { get; init; }


    /// <summary>
    /// 最后一次成功访问的存取器描述符。
    /// </summary>
    public virtual AccessorDescriptor? AccessorDescriptor
        => SwitchingEquilizer.InvokeGetLast(a => a.AccessorDescriptor);

    /// <summary>
    /// 最后一次成功访问的存取器标识。
    /// </summary>
    public virtual string AccessorId
        => SwitchingEquilizer.InvokeGetLast(a => a.AccessorId);

    /// <summary>
    /// 当前复合存取器类型。
    /// </summary>
    public virtual Type AccessorType
        => typeof(CompositeAccessor);


    #region IAsyncDisposable

    /// <summary>
    /// 异步释放所有存取器资源。
    /// </summary>
    /// <returns>返回 <see cref="ValueTask"/>。</returns>
    public virtual ValueTask DisposeAsync()
        => TraversalEquilizer.InvokeGetLast(a => a.DisposeAsync());

    #endregion


    #region IDisposable

    /// <summary>
    /// 释放所有存取器资源。
    /// </summary>
    public virtual void Dispose()
        => TraversalEquilizer.Invoke(a => a.Dispose());

    #endregion


    #region IConnectable<IAccessor>

    /// <summary>
    /// 最后一次成功访问存取器的当前连接字符串。
    /// </summary>
    public virtual string? CurrentConnectionString
        => SwitchingEquilizer.InvokeGetLast(a => a.CurrentConnectionString);

    /// <summary>
    /// 所有存取器连接改变时动作（始终抛出异常）。
    /// </summary>
    public virtual Action<IAccessor>? ConnectionChangingAction
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }

    /// <summary>
    /// 所有存取器连接改变后动作（始终抛出异常）。
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public virtual Action<IAccessor>? ConnectionChangedAction
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    }


    /// <summary>
    /// 改变数据库连接（始终抛出异常）。
    /// </summary>
    /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    /// <exception cref="NotImplementedException"></exception>
    public virtual IAccessor ChangeConnection(string newConnectionString)
        => throw new NotImplementedException();


    /// <summary>
    /// 尝试创建所有存取器的数据库。
    /// </summary>
    /// <returns>返回最后一次创建结果布尔值。</returns>
    public virtual bool TryCreateDatabase()
        => TraversalEquilizer.InvokeGetLast(a => a.TryCreateDatabase());

    /// <summary>
    /// 尝试创建所有存取器的数据库。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回最后一次创建结果布尔值。</returns>
    public virtual Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
        => TraversalEquilizer.InvokeGetLast(a => a.TryCreateDatabaseAsync(cancellationToken));

    #endregion


    #region ISaveChangeable

    /// <summary>
    /// 保存所有存取器更改。
    /// </summary>
    /// <returns>返回受影响的行数。</returns>
    public virtual int SaveChanges()
        => SaveChanges(acceptAllChangesOnSuccess: true);

    /// <summary>
    /// 保存所有存取器更改。
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
    /// <returns>返回受影响的行数。</returns>
    public virtual int SaveChanges(bool acceptAllChangesOnSuccess)
        => TraversalEquilizer.InvokeGetLast(a => a.SaveChanges(acceptAllChangesOnSuccess));


    /// <summary>
    /// 异步保存所有存取器更改。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);

    /// <summary>
    /// 异步保存所有存取器更改。
    /// </summary>
    /// <param name="acceptAllChangesOnSuccess">指示是否在更改已成功发送到数据库之后调用。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
        => TraversalEquilizer.InvokeGetLast(a => a.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));

    #endregion


    #region IShardable

    /// <summary>
    /// 最后一次成功访问存取器的分片管理器。
    /// </summary>
    public virtual IShardingManager ShardingManager
        => SwitchingEquilizer.InvokeGetLast(a => a.ShardingManager);

    #endregion


    #region Query

    /// <summary>
    /// 创建最后一次成功访问存取器的指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>()
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.Query<TEntity>());

    /// <summary>
    /// 创建最后一次成功访问存取器的指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>(string name)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.Query<TEntity>(name));


    /// <summary>
    /// 通过 SQL 语句创建最后一次成功访问存取器的指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> QueryBySql<TEntity>(string sql,
        params object[] parameters)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.QueryBySql<TEntity>(sql, parameters));

    /// <summary>
    /// 通过 SQL 语句创建最后一次成功访问存取器的指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> QueryBySql<TEntity>(string name,
        string sql, params object[] parameters)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.QueryBySql<TEntity>(name, sql, parameters));

    #endregion


    #region Exists

    /// <summary>
    /// 最后一次成功访问的存取器在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.Exists(predicate));

    /// <summary>
    /// 异步最后一次成功访问的存取器在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.ExistsAsync(predicate, checkLocal, cancellationToken));

    #endregion


    #region Find

    /// <summary>
    /// 从表达式建立最后一次成功访问存取器的指定结果的可查询接口。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="expression">给定的 <see cref="IQueryable{TResult}"/> 表达式。</param>
    /// <returns>返回 <see cref="IQueryable{TResult}"/>。</returns>
    public virtual IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression)
        => SwitchingEquilizer.InvokeGetLast(a => a.FromExpression(expression));


    /// <summary>
    /// 查找最后一次成功访问存取器的指定键值数组的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object? Find(Type entityType, params object?[]? keyValues)
        => SwitchingEquilizer.InvokeGetLast(a => a.Find(entityType, keyValues));

    /// <summary>
    /// 异步查找最后一次成功访问存取器的指定键值数组的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回一个包含实体对象的异步操作。</returns>
    public virtual ValueTask<object?> FindAsync(Type entityType, params object?[]? keyValues)
        => SwitchingEquilizer.InvokeGetLast(a => a.FindAsync(entityType, keyValues));

    /// <summary>
    /// 异步查找最后一次成功访问存取器的指定键值数组的实体对象。
    /// </summary>
    /// <param name="entityType">给定的实体类型。</param>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含实体对象的异步操作。</returns>
    public virtual ValueTask<object?> FindAsync(Type entityType, object?[]? keyValues, CancellationToken cancellationToken)
        => SwitchingEquilizer.InvokeGetLast(a => a.FindAsync(entityType, keyValues, cancellationToken));


    /// <summary>
    /// 查找最后一次成功访问存取器的指定键值数组的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity? Find<TEntity>(params object?[]? keyValues)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.Find<TEntity>(keyValues));

    /// <summary>
    /// 异步查找最后一次成功访问存取器的指定键值数组的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步结果。</returns>
    public virtual ValueTask<TEntity?> FindAsync<TEntity>(params object?[]? keyValues)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindAsync<TEntity>(keyValues));

    /// <summary>
    /// 异步查找最后一次成功访问存取器的指定键值数组的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="keyValues">给定的键值数组。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TEntity"/> 的异步结果。</returns>
    public virtual ValueTask<TEntity?> FindAsync<TEntity>(object?[]? keyValues, CancellationToken cancellationToken)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindAsync<TEntity>(keyValues, cancellationToken));


    /// <summary>
    /// 查找最后一次成功访问存取器的带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    public virtual IList<TEntity> FindListWithSpecification<TEntity>(IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindListWithSpecification(specification));

    /// <summary>
    /// 异步查找最后一次成功访问存取器的带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{TEntity}"/> 的异步操作。</returns>
    public virtual Task<IList<TEntity>> FindListWithSpecificationAsync<TEntity>(IEntitySpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindListWithSpecificationAsync(specification, cancellationToken));


    /// <summary>
    /// 查找最后一次成功访问存取器的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindPagingList(pageAction));

    /// <summary>
    /// 异步查找最后一次成功访问存取器的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public virtual Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindPagingListAsync(pageAction, cancellationToken));


    /// <summary>
    /// 查找最后一次成功访问存取器的带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindPagingListWithSpecification(pageAction, specification));

    /// <summary>
    /// 异步查找最后一次成功访问存取器的带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public virtual Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
        => SwitchingEquilizer.InvokeGetLast(a => a.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

    #endregion


    #region Add

    /// <summary>
    /// 添加所有存取器不存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity AddIfNotExists<TEntity>(TEntity entity,
        Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class
        => TraversalEquilizer.InvokeGetLast(a => a.AddIfNotExists(entity, predicate, checkLocal));

    /// <summary>
    /// 添加所有存取器实体对象。
    /// </summary>
    /// <param name="entity">给定要添加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Add(object entity)
        => TraversalEquilizer.InvokeGetLast(a => a.Add(entity));

    /// <summary>
    /// 添加所有存取器实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
        => TraversalEquilizer.InvokeGetLast(a => a.Add(entity));


    /// <summary>
    /// 添加所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    public virtual void AddRange(IEnumerable<object> entities)
        => TraversalEquilizer.Invoke(a => a.AddRange(entities));

    /// <summary>
    /// 添加所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    public virtual void AddRange(params object[] entities)
        => TraversalEquilizer.Invoke(a => a.AddRange(entities));

    /// <summary>
    /// 异步添加所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    public virtual Task AddRangeAsync(IEnumerable<object> entities,
        CancellationToken cancellationToken = default)
        => TraversalEquilizer.InvokeGetLast(a => a.AddRangeAsync(entities, cancellationToken));

    /// <summary>
    /// 异步添加所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要添加的实体对象集合。</param>
    /// <returns>返回 <see cref="Task"/>。</returns>
    public virtual Task AddRangeAsync(params object[] entities)
        => TraversalEquilizer.InvokeGetLast(a => a.AddRangeAsync(entities));

    #endregion


    #region Attach

    /// <summary>
    /// 附加所有存取器实体对象。
    /// </summary>
    /// <param name="entity">给定要附加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Attach(object entity)
        => TraversalEquilizer.InvokeGetLast(a => a.Attach(entity));

    /// <summary>
    /// 附加所有存取器实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要附加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
        => TraversalEquilizer.InvokeGetLast(a => a.Attach(entity));


    /// <summary>
    /// 附加所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要附加的实体对象集合。</param>
    public virtual void AttachRange(params object[] entities)
        => TraversalEquilizer.Invoke(a => a.AttachRange(entities));

    /// <summary>
    /// 附加所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要附加的实体对象集合。</param>
    public virtual void AttachRange(IEnumerable<object> entities)
        => TraversalEquilizer.Invoke(a => a.AttachRange(entities));

    #endregion


    #region Remove

    /// <summary>
    /// 移除所有存取器实体对象。
    /// </summary>
    /// <param name="entity">给定要移除的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Remove(object entity)
        => TraversalEquilizer.InvokeGetLast(a => a.Remove(entity));

    /// <summary>
    /// 移除所有存取器实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体。</typeparam>
    /// <param name="entity">给定要移除的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns> 
    public virtual TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
        => TraversalEquilizer.InvokeGetLast(a => a.Remove(entity));


    /// <summary>
    /// 移除所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要移除的实体对象集合。</param>
    public virtual void RemoveRange(params object[] entities)
        => TraversalEquilizer.Invoke(a => a.RemoveRange(entities));

    /// <summary>
    /// 移除所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要移除的实体对象集合。</param>
    public virtual void RemoveRange(IEnumerable<object> entities)
        => TraversalEquilizer.Invoke(a => a.RemoveRange(entities));

    #endregion


    #region Update

    /// <summary>
    /// 更新所有存取器实体对象。
    /// </summary>
    /// <param name="entity">给定要更新的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Update(object entity)
        => TraversalEquilizer.InvokeGetLast(a => a.Update(entity));

    /// <summary>
    /// 更新所有存取器实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要更新的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
        => TraversalEquilizer.InvokeGetLast(a => a.Update(entity));


    /// <summary>
    /// 更新所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要更新的实体对象集合。</param>
    public virtual void UpdateRange(params object[] entities)
        => TraversalEquilizer.Invoke(a => a.UpdateRange(entities));

    /// <summary>
    /// 更新所有存取器实体范围集合。
    /// </summary>
    /// <param name="entities">给定要更新的实体对象集合。</param>
    public virtual void UpdateRange(IEnumerable<object> entities)
        => TraversalEquilizer.Invoke(a => a.UpdateRange(entities));

    #endregion

}

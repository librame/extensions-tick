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
using Librame.Extensions.Dispatchers;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个实现 <see cref="IAccessor"/> 的可冗余存取器集合。
/// </summary>
public class RedundableAccessors : AbstractSortable, IAccessor
{
    /// <summary>
    /// 构造一个 <see cref="RedundableAccessors"/>。
    /// </summary>
    /// <param name="equilizer">给定的 <see cref="IDispatcher{IAccessor}"/> 读写均衡器存取器。</param>
    public RedundableAccessors(IDispatcher<IAccessor> equilizer)
        : this(equilizer, equilizer)
    {
        IsWritingSeparation = false;
    }

    /// <summary>
    /// 构造一个 <see cref="RedundableAccessors"/>。
    /// </summary>
    /// <param name="readingEquilizer">给定的 <see cref="IDispatcher{IAccessor}"/> 读均衡器存取器。</param>
    /// <param name="writingEquilizer">给定的 <see cref="IDispatcher{IAccessor}"/> 写均衡器存取器。</param>
    public RedundableAccessors(IDispatcher<IAccessor> readingEquilizer, IDispatcher<IAccessor> writingEquilizer)
    {
        ReadingEquilizer = readingEquilizer;
        WritingEquilizer = writingEquilizer;

        IsWritingSeparation = true;
    }


    /// <summary>
    /// 读均衡器存取器。
    /// </summary>
    public IDispatcher<IAccessor> ReadingEquilizer { get; init; }

    /// <summary>
    /// 写均衡器存取器。
    /// </summary>
    public IDispatcher<IAccessor> WritingEquilizer { get; init; }

    /// <summary>
    /// 是否读写分离。
    /// </summary>
    public bool IsWritingSeparation { get; init; }


    /// <summary>
    /// 当前数据库上下文。
    /// </summary>
    public virtual IDbContext CurrentContext
        => ReadingEquilizer.InvokeGetLast(a => a.CurrentContext);


    /// <summary>
    /// 存取器描述符。
    /// </summary>
    public AccessorDescriptor? AccessorDescriptor
        => ReadingEquilizer.InvokeGetLast(a => a.AccessorDescriptor);

    /// <summary>
    /// 存取器标识。
    /// </summary>
    public string AccessorId
        => ReadingEquilizer.InvokeGetLast(a => a.AccessorId);

    /// <summary>
    /// 存取器类型。
    /// </summary>
    public Type AccessorType
        => ReadingEquilizer.InvokeGetLast(a => a.AccessorType);


    #region Connection

    /// <summary>
    /// 当前连接字符串。
    /// </summary>
    public virtual string? CurrentConnectionString
        => ReadingEquilizer.InvokeGetLast(a => a.CurrentConnectionString);


    /// <summary>
    /// 改变数据库连接（始终抛出异常）。
    /// </summary>
    /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    /// <exception cref="NotImplementedException">
    /// The redundable accessors does not support this operation.
    /// </exception>
    public virtual IAccessor ChangeConnection(string newConnectionString)
        => throw new NotImplementedException();


    /// <summary>
    /// 尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public virtual bool TryCreateDatabase()
    {
        var result = ReadingEquilizer.InvokeGetLast(a => a.TryCreateDatabase());

        if (IsWritingSeparation)
            return result && WritingEquilizer.InvokeGetLast(a => a.TryCreateDatabase());

        return result;
    }

    /// <summary>
    /// 异步尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        var result = await ReadingEquilizer.InvokeGetLast(a => a.TryCreateDatabaseAsync(cancellationToken));

        if (IsWritingSeparation)
            return result && await WritingEquilizer.InvokeGetLast(a => a.TryCreateDatabaseAsync(cancellationToken));

        return result;
    }

    #endregion


    #region IShardable

    /// <summary>
    /// 分片管理器。
    /// </summary>
    public IShardingManager ShardingManager
        => ReadingEquilizer.InvokeGetLast(a => a.ShardingManager);

    #endregion


    #region ISortable

    /// <summary>
    /// 排序优先级（数值越小越优先）。
    /// </summary>
    public override float Priority
        => ReadingEquilizer.InvokeGetLast(a => a.Priority);


    /// <summary>
    /// 与指定的 <see cref="ISortable"/> 比较大小。
    /// </summary>
    /// <param name="other">给定的 <see cref="ISortable"/>。</param>
    /// <returns>返回整数。</returns>
    public override int CompareTo(ISortable? other)
        => ReadingEquilizer.InvokeGetLast(a => a.CompareTo(other));

    #endregion


    #region Query

    /// <summary>
    /// 创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>()
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.Query<TEntity>());

    /// <summary>
    /// 创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>(string name)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.Query<TEntity>(name));


    /// <summary>
    /// 通过 SQL 语句创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> QueryBySql<TEntity>(string sql,
        params object[] parameters)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.QueryBySql<TEntity>(sql, parameters));

    /// <summary>
    /// 通过 SQL 语句创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> QueryBySql<TEntity>(string name,
        string sql, params object[] parameters)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.QueryBySql<TEntity>(name, sql, parameters));

    #endregion


    #region Exists

    /// <summary>
    /// 在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.Exists(predicate, checkLocal));

    /// <summary>
    /// 异步在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.ExistsAsync(predicate, checkLocal, cancellationToken));

    #endregion


    #region Find

    /// <summary>
    /// 查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    public virtual IList<TEntity> FindListWithSpecification<TEntity>(IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindListWithSpecification(specification));

    /// <summary>
    /// 异步查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{TEntity}"/> 的异步操作。</returns>
    public virtual Task<IList<TEntity>> FindListWithSpecificationAsync<TEntity>(IEntitySpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindListWithSpecificationAsync(specification, cancellationToken));


    /// <summary>
    /// 查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingList(pageAction));

    /// <summary>
    /// 异步查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public virtual Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingListAsync(pageAction, cancellationToken));


    /// <summary>
    /// 查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingListWithSpecification(pageAction, specification));

    /// <summary>
    /// 异步查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public virtual Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
        => ReadingEquilizer.InvokeGetLast(a => a.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

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
    public virtual TEntity AddIfNotExists<TEntity>(TEntity entity, Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class
        => WritingEquilizer.InvokeGetLast(a => a.AddIfNotExists(entity, predicate, checkLocal));

    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">给定要添加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Add(object entity)
        => WritingEquilizer.InvokeGetLast(a => a.Add(entity));

    /// <summary>
    /// 添加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
        => WritingEquilizer.InvokeGetLast(a => a.Add(entity));

    #endregion


    #region Attach

    /// <summary>
    /// 附加实体对象。
    /// </summary>
    /// <param name="entity">给定要附加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Attach(object entity)
        => WritingEquilizer.InvokeGetLast(a => a.Attach(entity));

    /// <summary>
    /// 附加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要附加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
        => WritingEquilizer.InvokeGetLast(a => a.Attach(entity));

    #endregion


    #region Remove

    /// <summary>
    /// 移除实体对象。
    /// </summary>
    /// <param name="entity">给定要移除的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Remove(object entity)
        => WritingEquilizer.InvokeGetLast(a => a.Remove(entity));

    /// <summary>
    /// 移除实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体。</typeparam>
    /// <param name="entity">给定要移除的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
        => WritingEquilizer.InvokeGetLast(a => a.Remove(entity));

    #endregion


    #region Update

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">给定要更新的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Update(object entity)
        => WritingEquilizer.InvokeGetLast(a => a.Update(entity));

    /// <summary>
    /// 更新实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要更新的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
        => WritingEquilizer.InvokeGetLast(a => a.Update(entity));

    #endregion

}

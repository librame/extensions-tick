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
    /// <param name="dispatcher">给定的 <see cref="IDispatcher{IAccessor}"/> 读写存取器调度器。</param>
    public RedundableAccessors(IDispatcher<IAccessor> dispatcher)
        : this(dispatcher, dispatcher)
    {
        IsWritingSeparation = false;
    }

    /// <summary>
    /// 构造一个 <see cref="RedundableAccessors"/>。
    /// </summary>
    /// <param name="readingDispatcher">给定的 <see cref="IDispatcher{IAccessor}"/> 读存取器调度器。</param>
    /// <param name="writingDispatcher">给定的 <see cref="IDispatcher{IAccessor}"/> 写存取器调度器。</param>
    public RedundableAccessors(IDispatcher<IAccessor> readingDispatcher,
        IDispatcher<IAccessor> writingDispatcher)
    {
        ReadingDispatcher = readingDispatcher;
        WritingDispatcher = writingDispatcher;

        IsWritingSeparation = true;
    }


    /// <summary>
    /// 默认访问器（初始返回 <see cref="ReadingDispatcher"/> 第一项）。
    /// </summary>
    public virtual IAccessor DefaultAccessor
        => ReadingDispatcher.First;

    /// <summary>
    /// 默认写访问器（如果启用读写分离，则初始返回 <see cref="WritingDispatcher"/> 第一项，反之则返回 <see cref="DefaultAccessor"/>）。
    /// </summary>
    public virtual IAccessor DefaultWritingAccessor
        => IsWritingSeparation ? WritingDispatcher.First : DefaultAccessor;


    /// <summary>
    /// 读存取器调度器。
    /// </summary>
    public IDispatcher<IAccessor> ReadingDispatcher { get; init; }

    /// <summary>
    /// 写存取器调度器。
    /// </summary>
    public IDispatcher<IAccessor> WritingDispatcher { get; init; }

    /// <summary>
    /// 是否读写分离。
    /// </summary>
    public bool IsWritingSeparation { get; init; }


    /// <summary>
    /// 当前数据库上下文（默认返回读存取器的第一项 <see cref="IDbContext"/>）。
    /// </summary>
    public virtual IDbContext CurrentContext
        => DefaultAccessor.CurrentContext;


    /// <summary>
    /// 存取器描述符（默认返回读存取器的第一项 <see cref="AccessorDescriptor"/>）。
    /// </summary>
    public AccessorDescriptor? AccessorDescriptor
        => DefaultAccessor.AccessorDescriptor;

    /// <summary>
    /// 存取器标识（默认返回读存取器的所有项存取器标识集合）。
    /// </summary>
    public string AccessorId
        => string.Join(",", ReadingDispatcher.InvokeFunc(a => a.CurrentSource.AccessorId));

    /// <summary>
    /// 存取器类型。
    /// </summary>
    public Type AccessorType
        => GetType();


    #region Connection

    /// <summary>
    /// 当前连接字符串（默认返回读存取器的所有项连接字符串集合）。
    /// </summary>
    public virtual string? CurrentConnectionString
        => string.Join(",", ReadingDispatcher.InvokeFunc(a => a.CurrentSource.CurrentConnectionString));


    /// <summary>
    /// 改变默认访问器数据库连接。
    /// </summary>
    /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    /// <exception cref="NotImplementedException">
    /// The redundable accessors does not support this operation.
    /// </exception>
    public virtual IAccessor ChangeConnection(string newConnectionString)
        => DefaultAccessor.ChangeConnection(newConnectionString);


    /// <summary>
    /// 尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public virtual bool TryCreateDatabase()
    {
        var results = ReadingDispatcher.InvokeFunc(a => a.CurrentSource.TryCreateDatabase()).ToList();

        // 启用写入分离时需确保写入库已存在
        if (IsWritingSeparation)
            results.AddRange(WritingDispatcher.InvokeFunc(a => a.CurrentSource.TryCreateDatabase()));

        // 只要有一个 FALSE 就返回 FALSE
        return !results.Any(result => false);
    }

    /// <summary>
    /// 异步尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        var results = (await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.TryCreateDatabaseAsync(cancellationToken),
            cancellationToken: cancellationToken)).ToList();

        // 启用写入分离时需确保写入库已存在
        if (IsWritingSeparation)
        {
            results.AddRange(await WritingDispatcher.InvokeFuncAsync(a => a.CurrentSource.TryCreateDatabaseAsync(cancellationToken),
                cancellationToken: cancellationToken));
        }

        // 只要有一个 FALSE 就返回 FALSE
        return !results.Any(result => false);
    }

    #endregion


    #region IShardable

    /// <summary>
    /// 分片管理器。
    /// </summary>
    public IShardingManager ShardingManager
        => DefaultAccessor.ShardingManager;

    #endregion


    #region ISortable

    /// <summary>
    /// 排序优先级（数值越小越优先）。
    /// </summary>
    public override float Priority
        => ReadingDispatcher.Sources.Max(s => s.Priority) + 1; // 复合访问器优先级最低

    #endregion


    #region Query

    /// <summary>
    /// 创建指定实体类型的可查询接口（默认返回读存取器的第一项 <see cref="IQueryable{TEntity}"/>）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>()
        where TEntity : class
        => DefaultAccessor.Query<TEntity>();

    /// <summary>
    /// 创建指定实体类型的可查询接口（默认返回读存取器的第一项 <see cref="IQueryable{TEntity}"/>）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>(string name)
        where TEntity : class
        => DefaultAccessor.Query<TEntity>(name);


    /// <summary>
    /// 通过 SQL 语句创建指定实体类型的可查询接口（默认返回读存取器的第一项 <see cref="IQueryable{TEntity}"/>）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> QueryBySql<TEntity>(string sql,
        params object[] parameters)
        where TEntity : class
        => DefaultAccessor.QueryBySql<TEntity>(sql, parameters);

    /// <summary>
    /// 通过 SQL 语句创建指定实体类型的可查询接口（默认返回读存取器的第一项 <see cref="IQueryable{TEntity}"/>）。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <param name="sql">给定的 SQL 语句（可使用“${Schema}、${Table}/${TableName}”模板关键字分别代替架构、表名等参数值）。</param>
    /// <param name="parameters">给定的参数数组。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> QueryBySql<TEntity>(string name,
        string sql, params object[] parameters)
        where TEntity : class
        => DefaultAccessor.QueryBySql<TEntity>(name, sql, parameters);

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
    {
        var results = ReadingDispatcher.InvokeFunc(a => a.CurrentSource.Exists(predicate, checkLocal),
            (a, result) => result);

        // 只要有一个 TRUE 就返回 TRUE
        return results.Any(result => true);
    }

    /// <summary>
    /// 异步在本地缓存或数据库中是否存在指定断定方法的实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定方法表达式。</param>
    /// <param name="checkLocal">是否检查本地缓存（可选；默认启用检查）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> ExistsAsync<TEntity>(Expression<Func<TEntity, bool>> predicate,
        bool checkLocal = true, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var results = await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.ExistsAsync(predicate, checkLocal, cancellationToken),
            (a, result) => result, cancellationToken: cancellationToken);
        
        // 只要有一个 TRUE 就返回 TRUE
        return results.Any(result => true);
    }

    #endregion


    #region Find

    /// <summary>
    /// 查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    public virtual IList<TEntity> FindsWithSpecification<TEntity>(
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => ReadingDispatcher.InvokeFunc(a => a.CurrentSource.FindsWithSpecification(specification)).SelectMany(s => s).ToList();

    /// <summary>
    /// 异步查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{TEntity}"/> 的异步操作。</returns>
    public virtual async Task<IList<TEntity>> FindsWithSpecificationAsync<TEntity>(
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var result = await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.FindsWithSpecificationAsync(specification, cancellationToken));

        return result.SelectMany(s => s).ToList();
    }


    /// <summary>
    /// 查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
    {
        var pagings = ReadingDispatcher.InvokeFunc(a => a.CurrentSource.FindPagingList(pageAction));

        return pagings.CompositePaging();
    }

    /// <summary>
    /// 异步查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public virtual async Task<IPagingList<TEntity>> FindPagingListAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var pagings = await ReadingDispatcher.InvokeFuncAsync(a => a.CurrentSource.FindPagingListAsync(pageAction, cancellationToken));

        return await pagings.CompositePagingAsync(cancellationToken);
    }


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
    {
        var pagings = ReadingDispatcher.InvokeFunc(a => a.CurrentSource.FindPagingListWithSpecification(pageAction, specification));

        return pagings.CompositePaging();
    }

    /// <summary>
    /// 异步查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public virtual async Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var pagings = await ReadingDispatcher.InvokeFuncAsync(a
            => a.CurrentSource.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

        return await pagings.CompositePagingAsync(cancellationToken);
    }

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
    public virtual TEntity AddIfNotExists<TEntity>(TEntity entity,
        Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.AddIfNotExists(entity, predicate, checkLocal), isTraversal: false).First();

    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">给定要添加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Add(object entity)
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Add(entity), isTraversal: false).First();

    /// <summary>
    /// 添加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Add(entity), isTraversal: false).First();

    #endregion


    #region Attach

    /// <summary>
    /// 附加实体对象。
    /// </summary>
    /// <param name="entity">给定要附加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Attach(object entity)
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Attach(entity), isTraversal: false).First();

    /// <summary>
    /// 附加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要附加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Attach(entity), isTraversal: false).First();

    #endregion


    #region Remove

    /// <summary>
    /// 移除实体对象。
    /// </summary>
    /// <param name="entity">给定要移除的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Remove(object entity)
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Remove(entity), isTraversal: false).First();

    /// <summary>
    /// 移除实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体。</typeparam>
    /// <param name="entity">给定要移除的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Remove(entity), isTraversal: false).First();

    #endregion


    #region Update

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">给定要更新的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Update(object entity)
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Update(entity), isTraversal: false).First();

    /// <summary>
    /// 更新实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要更新的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.InvokeFunc(a => a.CurrentSource.Remove(entity), isTraversal: false).First();

    #endregion

}

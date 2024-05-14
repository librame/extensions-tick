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
using Librame.Extensions.Data;
using Librame.Extensions.Data.Accessing;
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Infrastructure.Core;
using Librame.Extensions.Infrastructure.Dispatching;
using Librame.Extensions.Infrastructure.Specification;

namespace Librame.Extensions.Dispatching;

/// <summary>
/// 定义一个实现 <see cref="IAccessor"/> 的基础调度器存取器集合。
/// </summary>
public class BaseDispatcherAccessors : AbstractPriorable, IDispatcherAccessors
{
    /// <summary>
    /// 构造一个 <see cref="BaseDispatcherAccessors"/>。
    /// </summary>
    /// <param name="dispatcher">给定的 <see cref="IDispatcher{IAccessor}"/> 读写存取器调度器。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    public BaseDispatcherAccessors(IDispatcher<IAccessor> dispatcher, DispatchingMode mode)
        : this(dispatcher, dispatcher, mode)
    {
        IsWritingSeparation = false;
    }

    /// <summary>
    /// 构造一个 <see cref="BaseDispatcherAccessors"/>。
    /// </summary>
    /// <param name="readingDispatcher">给定的 <see cref="IDispatcher{IAccessor}"/> 读存取器调度器。</param>
    /// <param name="writingDispatcher">给定的 <see cref="IDispatcher{IAccessor}"/> 写存取器调度器。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    public BaseDispatcherAccessors(IDispatcher<IAccessor> readingDispatcher,
        IDispatcher<IAccessor> writingDispatcher, DispatchingMode mode)
    {
        ReadingDispatcher = readingDispatcher;
        WritingDispatcher = writingDispatcher;
        Mode = mode;

        IsWritingSeparation = true;
    }


    /// <summary>
    /// 默认访问器（初始返回 <see cref="ReadingDispatcher"/> 第一项）。
    /// </summary>
    public virtual IAccessor DefaultAccessor
        => ReadingDispatcher.FirstSource;

    /// <summary>
    /// 默认写访问器（如果启用读写分离，则初始返回 <see cref="WritingDispatcher"/> 第一项，反之则返回 <see cref="DefaultAccessor"/>）。
    /// </summary>
    public virtual IAccessor DefaultWritingAccessor
        => IsWritingSeparation ? WritingDispatcher.FirstSource : DefaultAccessor;


    /// <summary>
    /// 读存取器调度器。
    /// </summary>
    public IDispatcher<IAccessor> ReadingDispatcher { get; init; }

    /// <summary>
    /// 写存取器调度器。
    /// </summary>
    public IDispatcher<IAccessor> WritingDispatcher { get; init; }

    /// <summary>
    /// 调度模式。
    /// </summary>
    public DispatchingMode Mode { get; init; }

    /// <summary>
    /// 是否读写分离。
    /// </summary>
    public bool IsWritingSeparation { get; init; }


    /// <summary>
    /// 当前数据库上下文（默认返回读存取器的第一项 <see cref="IDataContext"/>）。
    /// </summary>
    public virtual IDataContext CurrentContext
        => DefaultAccessor.CurrentContext;


    /// <summary>
    /// 存取器描述符（默认返回读存取器的第一项 <see cref="AccessorDescriptor"/>）。
    /// </summary>
    public AccessorDescriptor? AccessorDescriptor
        => DefaultAccessor.AccessorDescriptor;

    /// <summary>
    /// 分库描述符（默认返回读存取器的第一项 <see cref="ShardingDescriptor"/>）。
    /// </summary>
    public ShardingDescriptor? ShardingDescriptor
        => DefaultAccessor.ShardingDescriptor;

    /// <summary>
    /// 存取器标识（默认返回读存取器的所有项存取器标识集合）。
    /// </summary>
    public string AccessorId
        => DefaultAccessor.AccessorId;

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
        => DefaultAccessor.CurrentConnectionString;


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
    /// 异步尝试改变数据库连接。
    /// </summary>
    /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IAccessor"/> 的异步操作。</returns>
    public virtual async Task<IAccessor> ChangeConnectionAsync(string newConnectionString,
        CancellationToken cancellationToken = default)
        => await DefaultAccessor.ChangeConnectionAsync(newConnectionString, cancellationToken);


    /// <summary>
    /// 尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public virtual bool TryCreateDatabase()
    {
        var results = ReadingDispatcher.DispatchFunc(static a => a.CurrentSource!.TryCreateDatabase()).ToList();

        // 启用写入分离时需确保写入库已存在
        if (IsWritingSeparation)
            results.AddRange(WritingDispatcher.DispatchFunc(static a => a.CurrentSource!.TryCreateDatabase()));

        // 只要有一个 FALSE 就返回 FALSE
        return !results.Any(static result => false);
    }

    /// <summary>
    /// 异步尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        var results = (await ReadingDispatcher.DispatchFuncAsync(a =>
            a.CurrentSource!.TryCreateDatabaseAsync(cancellationToken),
            cancellationToken: cancellationToken)).ToList();

        // 启用写入分离时需确保写入库已存在
        if (IsWritingSeparation)
        {
            results.AddRange(await WritingDispatcher.DispatchFuncAsync(a =>
                a.CurrentSource!.TryCreateDatabaseAsync(cancellationToken),
                cancellationToken: cancellationToken));
        }

        // 只要有一个 FALSE 就返回 FALSE
        return !results.Any(static result => false);
    }

    #endregion


    #region IPriorable

    /// <summary>
    /// 获取优先级。
    /// </summary>
    /// <returns>返回浮点数。</returns>
    public override float GetPriority()
        => ReadingDispatcher.Sources.Max(static s => s.GetPriority()) + 1; // 复合访问器优先级最低

    #endregion


    #region IShardable

    /// <summary>
    /// 分片上下文。
    /// </summary>
    public IShardingContext ShardingContext
        => DefaultAccessor.ShardingContext;

    #endregion


    #region IShardingValue<DateTimeOffset>

    /// <summary>
    /// 获取分片值。
    /// </summary>
    /// <param name="defaultValue">给定的默认值。</param>
    /// <returns>返回 <see cref="DateTimeOffset"/>。</returns>
    public virtual DateTimeOffset GetShardedValue(DateTimeOffset defaultValue)
        => DateTimeOffset.UtcNow;

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
    public virtual bool Exists<TEntity>(Expression<Func<TEntity, bool>> predicate, bool checkLocal = true)
        where TEntity : class
    {
        var results = ReadingDispatcher.DispatchFunc(a => a.CurrentSource!.Exists(predicate, checkLocal),
            (a, result) => result);

        // 只要有一个 TRUE 就返回 TRUE
        return results.Any(static result => true);
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
        var results = await ReadingDispatcher.DispatchFuncAsync(a =>
            a.CurrentSource!.ExistsAsync(predicate, checkLocal, cancellationToken),
            (a, result) => result, cancellationToken: cancellationToken);

        // 只要有一个 TRUE 就返回 TRUE
        return results.Any(static result => true);
    }

    #endregion


    #region Find

    /// <summary>
    /// 查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="ISpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    public virtual IList<TEntity> FindsWithSpecification<TEntity>(
        ISpecification<TEntity>? specification = null)
        where TEntity : class
        => ReadingDispatcher.DispatchFunc(a => a.CurrentSource!.FindsWithSpecification(specification))
            .SelectMany(s => s).ToList();

    /// <summary>
    /// 异步查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="ISpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IList{TEntity}"/> 的异步操作。</returns>
    public virtual async Task<IList<TEntity>> FindsWithSpecificationAsync<TEntity>(
        ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var result = await ReadingDispatcher.DispatchFuncAsync(a =>
            a.CurrentSource!.FindsWithSpecificationAsync(specification, cancellationToken));

        return result.SelectMany(static s => s).ToList();
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
        var pagings = ReadingDispatcher.DispatchFunc(a => a.CurrentSource!.FindPagingList(pageAction));

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
        var pagings = await ReadingDispatcher.DispatchFuncAsync(a =>
            a.CurrentSource!.FindPagingListAsync(pageAction, cancellationToken));

        return await pagings.CompositePagingAsync(cancellationToken);
    }


    /// <summary>
    /// 查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        ISpecification<TEntity>? specification = null)
        where TEntity : class
    {
        var pagings = ReadingDispatcher.DispatchFunc(a => a.CurrentSource!
            .FindPagingListWithSpecification(pageAction, specification));

        return pagings.CompositePaging();
    }

    /// <summary>
    /// 异步查找带有规约的实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IPagingList{TEntity}"/> 的异步操作。</returns>
    public virtual async Task<IPagingList<TEntity>> FindPagingListWithSpecificationAsync<TEntity>(Action<IPagingList<TEntity>> pageAction,
        ISpecification<TEntity>? specification = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var pagings = await ReadingDispatcher.DispatchFuncAsync(a
            => a.CurrentSource!.FindPagingListWithSpecificationAsync(pageAction, specification, cancellationToken));

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
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.AddIfNotExists(entity, predicate, checkLocal),
            isTraversal: false).First();

    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">给定要添加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Add(object entity)
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Add(entity), isTraversal: false).First();

    /// <summary>
    /// 添加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Add(entity), isTraversal: false).First();

    #endregion


    #region Attach

    /// <summary>
    /// 附加实体对象。
    /// </summary>
    /// <param name="entity">给定要附加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Attach(object entity)
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Attach(entity), isTraversal: false).First();

    /// <summary>
    /// 附加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要附加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Attach(entity), isTraversal: false).First();

    #endregion


    #region Remove

    /// <summary>
    /// 移除实体对象。
    /// </summary>
    /// <param name="entity">给定要移除的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Remove(object entity)
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Remove(entity), isTraversal: false).First();

    /// <summary>
    /// 移除实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体。</typeparam>
    /// <param name="entity">给定要移除的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Remove(entity), isTraversal: false).First();

    #endregion


    #region Update

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">给定要更新的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Update(object entity)
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Update(entity), isTraversal: false).First();

    /// <summary>
    /// 更新实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要更新的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.Remove(entity), isTraversal: false).First();

    #endregion


    #region DirectExecute

    /// <summary>
    /// 直接删除，不通过跟踪实体实现。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定条件（可选；默认为空表示删除所有）。</param>
    /// <returns>返回受影响的行数。</returns>
    public virtual int DirectDelete<TEntity>(Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class
        => WritingDispatcher.DispatchFunc(a => a.CurrentSource!.DirectDelete(predicate), isTraversal: false).First();

    /// <summary>
    /// 异步直接删除，不通过跟踪实体实现。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="predicate">给定的断定条件（可选；默认为空表示删除所有）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    public virtual async Task<int> DirectDeleteAsync<TEntity>(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => await WritingDispatcher.DispatchFunc(a => a.CurrentSource!.DirectDeleteAsync(predicate, cancellationToken),
            isTraversal: false).First();

    #endregion


    /// <summary>
    /// 因此为复合实现，故始终返回不相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IAccessor"/>。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool Equals(IAccessor? other)
        => false;


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IDispatcherAccessors"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(IDispatcherAccessors? other)
        => other is not null && ToString() == other.ToString();

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => obj is IDispatcherAccessors other && Equals(other);

    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();

    /// <summary>
    /// 转为以英文逗号分隔的读/写调度器集合字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
    {
        var list = new List<string>();

        // 读/写分开断定，不具体检测内部各存取器
        if (Mode == DispatchingMode.Mirroring)
            list.Add("Reading:" + ReadingDispatcher.First().ToString());
        else
            list.Add("Reading:" + ReadingDispatcher.ToString());

        if (IsWritingSeparation)
        {
            if (Mode == DispatchingMode.Mirroring)
                list.Add(";Writing:" + WritingDispatcher.First().ToString());
            else
                list.Add(";Writing:" + WritingDispatcher.ToString());
        }

        return list.ToString() ?? string.Empty;
    }


    /// <summary>
    /// 获取枚举器。
    /// </summary>
    /// <returns>返回调度器枚举器。</returns>
    public IEnumerator<IDispatcher<IAccessor>> GetEnumerator()
    {
        // 读/写分开断定，不具体检测内部各存取器
        yield return ReadingDispatcher;

        if (IsWritingSeparation)
            yield return WritingDispatcher;
    }

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

}

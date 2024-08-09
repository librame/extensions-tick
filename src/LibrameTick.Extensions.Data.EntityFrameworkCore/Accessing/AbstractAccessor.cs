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
using Librame.Extensions.Infrastructure.Core;
using Librame.Extensions.Specification;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个抽象实现 <see cref="IAccessor"/> 的存取器。
/// </summary>
public abstract class AbstractAccessor : AbstractPriorable, IAccessor
{
    /// <summary>
    /// 构造一个 <see cref="AbstractAccessor"/>。
    /// </summary>
    /// <param name="context">给定的 <see cref="DataContext"/>。</param>
    protected AbstractAccessor(DataContext context)
    {
        OriginalContext = context;
        CurrentContext = context;

        InitializeAccessor();
    }

    private void InitializeAccessor()
    {
        OriginalContext.PostAccessor ??= this;

        // 首次尝试将默认数据库分库
        var newConnectionString = CurrentContext.Services.ShardingContext.ShardingDatabase(CurrentContext);
        if (newConnectionString is not null)
        {
            ChangeConnection(newConnectionString);
        }
    }


    /// <summary>
    /// 原始数据上下文。
    /// </summary>
    public DataContext OriginalContext { get; init; }

    /// <summary>
    /// 当前数据上下文。
    /// </summary>
    public virtual IDataContext CurrentContext { get; protected set; }


    /// <summary>
    /// 存取器描述符。
    /// </summary>
    public AccessorDescriptor? AccessorDescriptor
        => OriginalContext.CurrentServices.ContextAccessorOptions?.ToDescriptor(this);

    /// <summary>
    /// 分库描述符。
    /// </summary>
    public ShardingDescriptor? ShardingDescriptor
        => OriginalContext.CurrentServices.InitialShardingDescriptor;

    /// <summary>
    /// 存取器标识。
    /// </summary>
    public string AccessorId
        => OriginalContext.ContextId.ToString();

    /// <summary>
    /// 存取器类型。
    /// </summary>
    public Type AccessorType
        => GetType();


    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public DataExtensionOptions DataOptions
        => OriginalContext.CurrentServices.DataOptions;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreOptions
        => OriginalContext.CurrentServices.CoreOptions;


    /// <summary>
    /// 关系连接接口。
    /// </summary>
    protected IRelationalConnection RelationalConnection
        => OriginalContext.CurrentServices.RelationalConnection;

    /// <summary>
    /// 当前连接字符串。
    /// </summary>
    public virtual string? CurrentConnectionString
        => RelationalConnection.ConnectionString;


    /// <summary>
    /// 改变数据库连接。
    /// </summary>
    /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public virtual IAccessor ChangeConnection(string newConnectionString)
    {
        if (newConnectionString.Equals(RelationalConnection.ConnectionString, StringComparison.Ordinal))
            return this;

        RelationalConnection.ConnectionString = newConnectionString;

        TryCreateDatabase();

        return this;
    }

    /// <summary>
    /// 异步尝试改变数据库连接。
    /// </summary>
    /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IAccessor"/> 的异步操作。</returns>
    public virtual async Task<IAccessor> ChangeConnectionAsync(string newConnectionString,
        CancellationToken cancellationToken = default)
    {
        if (newConnectionString.Equals(RelationalConnection.ConnectionString, StringComparison.Ordinal))
            return this;

        RelationalConnection.ConnectionString = newConnectionString;

        await TryCreateDatabaseAsync(cancellationToken);

        return this;
    }


    /// <summary>
    /// 尝试创建数据库。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public virtual bool TryCreateDatabase()
    {
        if (DataOptions.Access.EnsureDatabaseCreated)
        {
            try
            {
                OriginalContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                // 用于临时解决文件型数据库分库后，DatabaseFacade 仍使用分库前的基础库名
                // 判断数据库是否存在，实际上分库后的新库已存在导致建表发生已存在的异常
                if (!ex.Message.Contains("already exists"))
                    throw;
            }
        }

        return false;
    }

    /// <summary>
    /// 异步尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        if (DataOptions.Access.EnsureDatabaseCreated)
        {
            try
            {
                await OriginalContext.Database.EnsureCreatedAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // 用于临时解决文件型数据库分库后，DatabaseFacade 仍使用分库前的基础库名
                // 判断数据库是否存在，实际上分库后的新库已存在导致建表发生已存在的异常
                if (!ex.Message.Contains("already exists"))
                    throw;
            }
        }
        
        return false;
    }


    #region Query

    /// <summary>
    /// 创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>()
        where TEntity : class
        => OriginalContext.Set<TEntity>();

    /// <summary>
    /// 创建指定实体类型的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> Query<TEntity>(string name)
        where TEntity : class
        => OriginalContext.Set<TEntity>(name);


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
    {
        var entityType = OriginalContext.Model.FindEntityType(typeof(TEntity));
        if (entityType is not null)
        {
            sql = DataOptions.Access.FormatSchema(sql, entityType.GetSchema());
            sql = DataOptions.Access.FormatTableName(sql, entityType.GetTableName());
        }

        return OriginalContext.Set<TEntity>().FromSqlRaw(sql, parameters);
    }

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
    {
        var entityType = OriginalContext.Model.FindEntityType(typeof(TEntity));
        if (entityType is not null)
        {
            sql = DataOptions.Access.FormatSchema(sql, entityType.GetSchema());
            sql = DataOptions.Access.FormatTableName(sql, entityType.GetTableName());
        }

        return OriginalContext.Set<TEntity>(name).FromSqlRaw(sql, parameters);
    }

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
        => OriginalContext.Set<TEntity>().ExistsWithLocal(predicate, checkLocal);

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
        => OriginalContext.Set<TEntity>().ExistsWithLocalAsync(predicate, checkLocal, cancellationToken);

    #endregion


    #region Find

    /// <summary>
    /// 查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="ISpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IEnumerable{TEntity}"/>。</returns>
    public virtual IList<TEntity> FindsWithSpecification<TEntity>(ISpecification<TEntity>? specification = null)
        where TEntity : class
        => specification is null
            ? Query<TEntity>().ToList()
            : Query<TEntity>().Where(specification.IsSatisfiedBy).ToList();

    /// <summary>
    /// 异步查找带有规约的实体集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="specification">给定的 <see cref="ISpecification{TEntity}"/>（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="IEnumerable{TEntity}"/> 的异步操作。</returns>
    public virtual async Task<IList<TEntity>> FindsWithSpecificationAsync<TEntity>(ISpecification<TEntity>? specification = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
        => specification is null
            ? await Query<TEntity>().ToListAsync(cancellationToken)
            : await cancellationToken.SimpleTask(() => Query<TEntity>().Where(specification.IsSatisfiedBy).ToList());


    /// <summary>
    /// 查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => Query<TEntity>().AsPaging(pageAction);

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
        => Query<TEntity>().AsPagingAsync(pageAction, cancellationToken);


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
        => specification is null
            ? Query<TEntity>().AsPaging(pageAction)
            : Query<TEntity>().Where(specification.IsSatisfiedBy).AsPaging(pageAction);

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
        => specification is null
            ? await Query<TEntity>().AsPagingAsync(pageAction, cancellationToken)
            : await Query<TEntity>().Where(specification.IsSatisfiedBy).AsPagingAsync(pageAction, cancellationToken);

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
    {
        if (!Exists(predicate, checkLocal))
            OriginalContext.Add(entity);

        return entity;
    }

    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">给定要添加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Add(object entity)
    {
        OriginalContext.Add(entity);
        return entity;
    }

    /// <summary>
    /// 添加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
    {
        OriginalContext.Add(entity);
        return entity;
    }

    #endregion


    #region Attach

    /// <summary>
    /// 附加实体对象。
    /// </summary>
    /// <param name="entity">给定要附加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Attach(object entity)
    {
        OriginalContext.Attach(entity);
        return entity;
    }

    /// <summary>
    /// 附加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要附加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
    {
        OriginalContext.Attach(entity);
        return entity;
    }

    #endregion


    #region Remove

    /// <summary>
    /// 移除实体对象。
    /// </summary>
    /// <param name="entity">给定要移除的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Remove(object entity)
    {
        OriginalContext.Remove(entity);
        return entity;
    }

    /// <summary>
    /// 移除实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体。</typeparam>
    /// <param name="entity">给定要移除的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
    {
        OriginalContext.Remove(entity);
        return entity;
    }

    #endregion


    #region Update

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">给定要更新的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual object Update(object entity)
    {
        OriginalContext.Update(entity);
        return entity;
    }

    /// <summary>
    /// 更新实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要更新的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
    {
        OriginalContext.Update(entity);
        return entity;
    }

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
    {
        if (predicate is null)
            return Query<TEntity>().ExecuteDelete(); // Delete All

        return Query<TEntity>().Where(predicate).ExecuteDelete();
    }

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
    {
        if (predicate is null)
            return await Query<TEntity>().ExecuteDeleteAsync(cancellationToken); // Delete All

        return await Query<TEntity>().Where(predicate).ExecuteDeleteAsync(cancellationToken);
    }


    /// <summary>
    /// 直接更新，不通过跟踪实体实现。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="setPropertyCalls">给定要更新的属性调用。</param>
    /// <param name="predicate">给定的断定条件（可选；默认为空表示更新所有）。</param>
    /// <returns>返回受影响的行数。</returns>
    public virtual int DirectUpdate<TEntity>(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        Expression<Func<TEntity, bool>>? predicate = null)
        where TEntity : class
    {
        if (predicate is null)
            return Query<TEntity>().ExecuteUpdate(setPropertyCalls); // Update All

        return Query<TEntity>().Where(predicate).ExecuteUpdate(setPropertyCalls);
    }

    /// <summary>
    /// 异步直接更新，不通过跟踪实体实现。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="setPropertyCalls">给定要更新的属性调用。</param>
    /// <param name="predicate">给定的断定条件（可选；默认为空表示删除所有）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含受影响行数的异步操作。</returns>
    public virtual async Task<int> DirectUpdateAsync<TEntity>(Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls, 
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        if (predicate is null)
            return await Query<TEntity>().ExecuteUpdateAsync(setPropertyCalls, cancellationToken); // Delete All

        return await Query<TEntity>().Where(predicate).ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
    }

    #endregion


    #region IPriorable

    /// <summary>
    /// 获取优先级。
    /// </summary>
    /// <returns>返回浮点数。</returns>
    public override float GetPriority()
        => AccessorDescriptor?.Priority ?? DataOptions.Access.DefaultPriority;

    #endregion


    #region IShardable

    /// <summary>
    /// 分片上下文。
    /// </summary>
    public virtual IShardingContext ShardingContext
        => OriginalContext.CurrentServices.ShardingContext.Initialize(CurrentContext);

    #endregion


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="IAccessor"/>。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public virtual bool Equals(IAccessor? other)
        => other is not null && ToString() == other.ToString();

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回是否相等的布尔值。</returns>
    public override bool Equals(object? obj)
        => obj is IAccessor other && Equals(other);

    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => ToString().GetHashCode();

    /// <summary>
    /// 转为以英文逗号分隔的所有来源字符串形式集合的字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => AccessorId;

}

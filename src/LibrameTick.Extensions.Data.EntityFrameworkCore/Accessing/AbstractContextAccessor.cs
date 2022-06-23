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
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个抽象实现 <see cref="IAccessor"/> 的 <see cref="DbContext"/> 存取器。
/// </summary>
public abstract class AbstractContextAccessor : IAccessor
{
    private readonly IOptionsMonitor<DataExtensionOptions> _dataOptionsMonitor;
    private readonly IOptionsMonitor<CoreExtensionOptions> _coreOptionsMonitor;

    private readonly AccessorDbContextOptionsExtension? _accessorExtension;
    private readonly RelationalOptionsExtension? _relationalExtension;


    /// <summary>
    /// 构造一个 <see cref="AbstractContextAccessor"/>。
    /// </summary>
    /// <param name="context">给定的 <see cref="DbContext"/>。</param>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    /// <param name="dataOptionsMonitor">给定的 <see cref="IOptionsMonitor{DataExtensionOptions}"/>。</param>
    /// <param name="coreOptionsMonitor">给定的 <see cref="IOptionsMonitor{CoreExtensionOptions}"/>。</param>
    protected AbstractContextAccessor(DbContext context, DbContextOptions options,
        IOptionsMonitor<DataExtensionOptions> dataOptionsMonitor,
        IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor)
    {
        _accessorExtension = options.FindExtension<AccessorDbContextOptionsExtension>();
        _relationalExtension = options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();

        _dataOptionsMonitor = dataOptionsMonitor;
        _coreOptionsMonitor = coreOptionsMonitor;

        OriginalContext = context;
    }


    /// <summary>
    /// 原始数据库上下文。
    /// </summary>
    public DbContext OriginalContext { get; init; }

    /// <summary>
    /// 当前数据库上下文。
    /// </summary>
    public abstract IDbContext CurrentContext { get; protected set; }


    /// <summary>
    /// 存取器描述符。
    /// </summary>
    public AccessorDescriptor? AccessorDescriptor
        => _accessorExtension?.ToDescriptor(this);

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
        => _dataOptionsMonitor.CurrentValue;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreOptions
        => _coreOptionsMonitor.CurrentValue;

    /// <summary>
    /// 存取器选项扩展。
    /// </summary>
    protected AccessorDbContextOptionsExtension? AccessorExtension
        => _accessorExtension;

    /// <summary>
    /// 关系型选项扩展。
    /// </summary>
    protected RelationalOptionsExtension? RelationalExtension
        => _relationalExtension;


    #region Connection

    /// <summary>
    /// 关系连接接口。
    /// </summary>
    protected IRelationalConnection RelationalConnection
        => OriginalContext.GetService<IRelationalConnection>();

    /// <summary>
    /// 当前连接字符串。
    /// </summary>
    public virtual string? CurrentConnectionString
        => RelationalConnection?.ConnectionString;


    /// <summary>
    /// 改变数据库连接。
    /// </summary>
    /// <param name="newConnectionString">给定的新数据库连接字符串。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public virtual IAccessor ChangeConnection(string newConnectionString)
    {
        var connection = _relationalExtension?.Connection
            ?? RelationalConnection.DbConnection;

        if (connection is not null)
        {
            DataOptions.Access.ConnectionChangingAction?.Invoke(this);

            connection.ConnectionString = newConnectionString;

            DataOptions.Access.ConnectionChangedAction?.Invoke(this);
        }

        return this;
    }


    /// <summary>
    /// 尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public virtual bool TryCreateDatabase()
    {
        if (DataOptions.Access.EnsureDatabaseDeleted)
            OriginalContext.Database.EnsureDeleted();

        if (DataOptions.Access.EnsureDatabaseCreated)
            return OriginalContext.Database.EnsureCreated();

        return false;
    }

    /// <summary>
    /// 异步尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> TryCreateDatabaseAsync(CancellationToken cancellationToken = default)
    {
        if (DataOptions.Access.EnsureDatabaseDeleted)
            await OriginalContext.Database.EnsureDeletedAsync(cancellationToken);

        if (DataOptions.Access.EnsureDatabaseCreated)
            await OriginalContext.Database.EnsureCreatedAsync(cancellationToken);

        return false;
    }

    #endregion


    #region IShardable

    /// <summary>
    /// 分片管理器。
    /// </summary>
    public IShardingManager ShardingManager
        => OriginalContext.GetService<IShardingManager>();

    #endregion


    #region ISortable

    /// <summary>
    /// 排序优先级（数值越小越优先）。
    /// </summary>
    public virtual float Priority
        => DataOptions.Access.DefaultPriority;


    /// <summary>
    /// 与指定的 <see cref="ISortable"/> 比较大小。
    /// </summary>
    /// <param name="other">给定的 <see cref="ISortable"/>。</param>
    /// <returns>返回整数。</returns>
    public virtual int CompareTo(ISortable? other)
        => Priority.CompareTo(other?.Priority ?? 0);

    #endregion


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
        => OriginalContext.Set<TEntity>().ExistsBySpecification(predicate, checkLocal);

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
        => OriginalContext.Set<TEntity>().ExistsBySpecificationAsync(predicate, checkLocal, cancellationToken);

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
        => Query<TEntity>().EvaluateList(specification);

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
        => Query<TEntity>().EvaluateListAsync(specification, cancellationToken);


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
    /// <param name="specification">给定的 <see cref="IEntitySpecification{TEntity}"/>（可选）。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingListWithSpecification<TEntity>(Action<IPagingList<TEntity>> pageAction,
        IEntitySpecification<TEntity>? specification = null)
        where TEntity : class
        => Query<TEntity>().EvaluatePagingList(pageAction, specification);

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
        => Query<TEntity>().EvaluatePagingListAsync(pageAction, specification, cancellationToken);

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

}

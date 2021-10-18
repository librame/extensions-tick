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
using Librame.Extensions.Data.Specification;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义抽象 <see cref="AbstractAccessor"/> 的泛型实现。
/// </summary>
/// <typeparam name="TAccessor">指定实现 <see cref="AbstractAccessor"/> 的访问器类型。</typeparam>
public abstract class AbstractAccessor<TAccessor> : AbstractAccessor
    where TAccessor : AbstractAccessor
{
    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractAccessor{TAccessor}"/> 实例。
    /// </summary>
    /// <remarks>
    /// 备注：如果需要注册多个 <see cref="DbContext"/> 扩展，参数必须使用泛型 <see cref="DbContextOptions{TAccessor}"/> 形式，
    /// 不能使用非泛型 <see cref="DbContextOptions"/> 形式，因为 <paramref name="options"/> 参数也会注册到容器中以供使用。
    /// </remarks>
    /// <param name="options">给定的 <see cref="DbContextOptions{TAccessor}"/>。</param>
    protected AbstractAccessor(DbContextOptions<TAccessor> options)
        : base(options)
    {
    }


    /// <summary>
    /// 访问器类型。
    /// </summary>
    public override Type AccessorType
        => typeof(TAccessor);
}


/// <summary>
/// 定义抽象 <see cref="DbContext"/> 与 <see cref="IAccessor"/> 的实现。
/// </summary>
public abstract class AbstractAccessor : DbContext, IAccessor
{
    private readonly AccessorDbContextOptionsExtension? _accessorExtension;
    private readonly RelationalOptionsExtension? _relationalExtension;


    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractAccessor"/> 实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    protected AbstractAccessor(DbContextOptions options)
        : base(options)
    {
        _accessorExtension = options.FindExtension<AccessorDbContextOptionsExtension>();
        _relationalExtension = options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();
        
        // 当启用分库功能时，需在切换到分库后尝试创建数据库
        ChangedAction = accessor => accessor.TryCreateDatabase();
    }


    /// <summary>
    /// 访问器标识。
    /// </summary>
    public virtual string AccessorId
        => ContextId.ToString();

    /// <summary>
    /// 访问器类型。
    /// </summary>
    public virtual Type AccessorType
        => GetType();

    /// <summary>
    /// 访问器描述符。
    /// </summary>
    public virtual AccessorDescriptor? AccessorDescriptor
        => _accessorExtension is not null ? _accessorExtension.ToDescriptor(this) : null;

    /// <summary>
    /// 数据扩展选项。
    /// </summary>
    public DataExtensionOptions DataOptions
        => this.GetService<IOptionsMonitor<DataExtensionOptions>>().CurrentValue;

    /// <summary>
    /// 核心扩展选项。
    /// </summary>
    public CoreExtensionOptions CoreOptions
        => this.GetService<IOptionsMonitor<CoreExtensionOptions>>().CurrentValue;


    #region IConnectable<IAccessor>

    /// <summary>
    /// 关系连接接口。
    /// </summary>
    protected IRelationalConnection RelationalConnection
        => this.GetService<IRelationalConnection>();

    /// <summary>
    /// 当前连接字符串。
    /// </summary>
    public virtual string? CurrentConnectionString
        => RelationalConnection?.ConnectionString;

    /// <summary>
    /// 改变时动作。
    /// </summary>
    public Action<IAccessor>? ChangingAction { get; set; }

    /// <summary>
    /// 改变后动作（默认连接改变后会尝试创建数据库）。
    /// </summary>
    public Action<IAccessor>? ChangedAction { get; set; }


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
            ChangingAction?.Invoke(this);

            connection.ConnectionString = newConnectionString;

            ChangedAction?.Invoke(this);
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
            Database.EnsureDeleted();

        if (DataOptions.Access.EnsureDatabaseCreated)
            return Database.EnsureCreated();

        return false;
    }

    /// <summary>
    /// 异步尝试创建数据库（已集成是否需要先删除数据库功能）。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含布尔值的异步操作。</returns>
    public virtual async Task<bool> TryCreateDatabaseAsync(
        CancellationToken cancellationToken = default)
    {
        if (DataOptions.Access.EnsureDatabaseDeleted)
            await Database.EnsureDeletedAsync(cancellationToken);

        if (DataOptions.Access.EnsureDatabaseCreated)
            await Database.EnsureCreatedAsync(cancellationToken);

        return false;
    }

    #endregion


    #region IShardable

    /// <summary>
    /// 分片管理器。
    /// </summary>
    public IShardingManager ShardingManager
        => this.GetService<IShardingManager>();

    #endregion


    #region ISortable

    /// <summary>
    /// 排序优先级（数值越小越优先）。
    /// </summary>
    public virtual float Priority
        => 1;


    /// <summary>
    /// 与指定的 <see cref="ISortable"/> 比较大小。
    /// </summary>
    /// <param name="other">给定的 <see cref="ISortable"/>。</param>
    /// <returns>返回整数。</returns>
    public virtual int CompareTo(ISortable? other)
        => Priority.CompareTo(other?.Priority ?? 0);

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
        => base.Set<TEntity>().LocalOrDbAny(predicate, checkLocal);

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
        => base.Set<TEntity>().LocalOrDbAnyAsync(predicate, checkLocal, cancellationToken);

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
        => GetQueryable<TEntity>().EvaluateList(specification);

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
        => GetQueryable<TEntity>().EvaluateListAsync(specification, cancellationToken);


    /// <summary>
    /// 查找实体分页集合。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="pageAction">给定的分页动作。</param>
    /// <returns>返回 <see cref="IPagingList{TEntity}"/>。</returns>
    public virtual IPagingList<TEntity> FindPagingList<TEntity>(Action<IPagingList<TEntity>> pageAction)
        where TEntity : class
        => GetQueryable<TEntity>().AsPaging(pageAction);

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
        => GetQueryable<TEntity>().AsPagingAsync(pageAction, cancellationToken);


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
        => GetQueryable<TEntity>().EvaluatePagingList(pageAction, specification);

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
        => GetQueryable<TEntity>().EvaluatePagingListAsync(pageAction, specification, cancellationToken);

    #endregion


    #region GetQueryable

    /// <summary>
    /// 获取指定实体的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> GetQueryable<TEntity>()
        where TEntity : class
        => base.Set<TEntity>();

    /// <summary>
    /// 获取指定实体的可查询接口。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="name">要使用的共享类型实体类型的名称。</param>
    /// <returns>返回 <see cref="IQueryable{TEntity}"/>。</returns>
    public virtual IQueryable<TEntity> GetQueryable<TEntity>(string name)
        where TEntity : class
        => base.Set<TEntity>(name);

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
            base.Add(entity);

        return entity;
    }

    /// <summary>
    /// 添加实体对象。
    /// </summary>
    /// <param name="entity">给定要添加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual new object Add(object entity)
    {
        base.Add(entity);
        return entity;
    }

    /// <summary>
    /// 添加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要添加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual new TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
    {
        base.Add(entity);
        return entity;
    }

    #endregion


    #region Attach

    /// <summary>
    /// 附加实体对象。
    /// </summary>
    /// <param name="entity">给定要附加的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual new object Attach(object entity)
    {
        base.Attach(entity);
        return entity;
    }

    /// <summary>
    /// 附加实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要附加的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual new TEntity Attach<TEntity>(TEntity entity)
        where TEntity : class
    {
        base.Attach(entity);
        return entity;
    }

    #endregion


    #region Remove

    /// <summary>
    /// 移除实体对象。
    /// </summary>
    /// <param name="entity">给定要移除的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual new object Remove(object entity)
    {
        base.Remove(entity);
        return entity;
    }

    /// <summary>
    /// 移除实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体。</typeparam>
    /// <param name="entity">给定要移除的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual new TEntity Remove<TEntity>(TEntity entity)
        where TEntity : class
    {
        base.Remove(entity);
        return entity;
    }

    #endregion


    #region Update

    /// <summary>
    /// 更新实体对象。
    /// </summary>
    /// <param name="entity">给定要更新的实体对象。</param>
    /// <returns>返回实体对象。</returns>
    public virtual new object Update(object entity)
    {
        base.Update(entity);
        return entity;
    }

    /// <summary>
    /// 更新实体。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="entity">给定要更新的实体。</param>
    /// <returns>返回 <typeparamref name="TEntity"/>。</returns>
    public virtual new TEntity Update<TEntity>(TEntity entity)
        where TEntity : class
    {
        base.Update(entity);
        return entity;
    }

    #endregion

}

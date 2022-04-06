﻿#region License

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
using Librame.Extensions.Data.ValueConversion;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义抽象继承 <see cref="AbstractDbContextAccessor"/> 的数据库上下文存取器泛型实现。
/// </summary>
/// <typeparam name="TAccessor">指定实现 <see cref="AbstractDbContextAccessor"/> 的存取器类型。</typeparam>
public abstract class AbstractDbContextAccessor<TAccessor> : AbstractDbContextAccessor
    where TAccessor : AbstractDbContextAccessor
{
    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractDbContextAccessor{TAccessor}"/> 实例。
    /// </summary>
    /// <remarks>
    /// 备注：如果需要注册多个 <see cref="DbContext"/> 扩展，参数必须使用泛型 <see cref="DbContextOptions{TAccessor}"/> 形式，
    /// 不能使用非泛型 <see cref="DbContextOptions"/> 形式，因为 <paramref name="options"/> 参数也会注册到容器中以供使用。
    /// </remarks>
    /// <param name="options">给定的 <see cref="DbContextOptions{TAccessor}"/>。</param>
    protected AbstractDbContextAccessor(DbContextOptions<TAccessor> options)
        : base(options)
    {
    }


    /// <summary>
    /// 存取器类型。
    /// </summary>
    public override Type AccessorType
        => typeof(TAccessor);
}


/// <summary>
/// 定义抽象继承 <see cref="DbContext"/> 与实现 <see cref="IAccessor"/> 的数据库上下文存取器。
/// </summary>
public abstract class AbstractDbContextAccessor : DbContext, IAccessor
{
    private readonly AccessorDbContextOptionsExtension? _accessorExtension;
    private readonly RelationalOptionsExtension? _relationalExtension;


    /// <summary>
    /// 使用指定的数据库上下文选项构造一个 <see cref="AbstractDbContextAccessor"/> 实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="DbContextOptions"/>。</param>
    protected AbstractDbContextAccessor(DbContextOptions options)
        : base(options)
    {
        _accessorExtension = options.FindExtension<AccessorDbContextOptionsExtension>();
        _relationalExtension = options.Extensions.OfType<RelationalOptionsExtension>().FirstOrDefault();
        
        // 当启用分库功能时，需在切换到分库后尝试创建数据库
        ChangedAction = accessor => accessor.TryCreateDatabase();
    }


    /// <summary>
    /// 存取器标识。
    /// </summary>
    public virtual string AccessorId
        => ContextId.ToString();

    /// <summary>
    /// 存取器类型。
    /// </summary>
    public virtual Type AccessorType
        => GetType();

    /// <summary>
    /// 存取器描述符。
    /// </summary>
    public virtual AccessorDescriptor? AccessorDescriptor
        => _accessorExtension?.ToDescriptor(this);

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


    #region ModelCreating

    /// <summary>
    /// 模型创建后置动作。
    /// </summary>
    public Action<IMutableEntityType>? ModelCreatingPostAction { get; set; }


    /// <summary>
    /// 开始模型创建。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingCore(modelBuilder);

        // 启用对实体加密属性功能的支持
        var converterFactory = this.GetService<IEncryptionConverterFactory>();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.UseEncryption(converterFactory, this);
            entityType.UseQueryFilters(DataOptions.QueryFilters, this);

            ModelCreatingPostAction?.Invoke(entityType);
        }
    }

    /// <summary>
    /// 开始模型创建核心。
    /// </summary>
    /// <param name="modelBuilder">给定的 <see cref="ModelBuilder"/>。</param>
    protected virtual void OnModelCreatingCore(ModelBuilder modelBuilder)
    {
        if (!DataOptions.Access.AutomaticMapping)
            return;

        // 默认尝试创建迁移程序集的模型
        if (!string.IsNullOrEmpty(_relationalExtension?.MigrationsAssembly))
            modelBuilder.CreateAssembliesModels(_relationalExtension.MigrationsAssembly);
    }

    #endregion


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
        => DataOptions.Access.DefaultPriority;


    /// <summary>
    /// 与指定的 <see cref="ISortable"/> 比较大小。
    /// </summary>
    /// <param name="other">给定的 <see cref="ISortable"/>。</param>
    /// <returns>返回整数。</returns>
    public virtual int CompareTo(ISortable? other)
        => Priority.CompareTo(other?.Priority ?? 0);

    #endregion


    #region ExecuteCommand

    /// <summary>
    /// 执行 SQL 语句成功。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <returns>返回是否成功的布尔值。</returns>
    public virtual bool ExecuteSuccess(string sql,
        DbParameter[]? parameters = null)
        => ExecuteCommand(sql, cmd => cmd.ExecuteNonQuery() > 0, parameters);

    /// <summary>
    /// 通过执行 SQL 语句查询单行单例的单个标量对象。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <returns>返回对象。</returns>
    public virtual object? ExecuteScalar(string sql,
        DbParameter[]? parameters = null)
        => ExecuteCommand(sql, cmd => cmd.ExecuteScalar(), parameters);

    /// <summary>
    /// 通过执行 SQL 语句查询实体列表。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <returns>返回 <see cref="IList{TEntity}"/>。</returns>
    public virtual IList<TEntity>? ExecuteList<TEntity>(string sql,
        DbParameter[]? parameters = null)
        where TEntity : class
    {
        var entityType = Model.FindEntityType(typeof(TEntity));
        if (entityType is not null)
        {
            sql = DataOptions.Access.FormatSchema(sql, entityType.GetSchema());
            sql = DataOptions.Access.FormatTableName(sql, entityType.GetTableName());
        }

        return ExecuteCommand(sql, cmd =>
        {
            var list = new List<IDictionary<string, object>>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();

                    for (var i = 0; i < reader.FieldCount; i++)
                        row.Add(reader.GetName(i), reader.GetValue(i));

                    list.Add(row);
                }
            }

            return list.AsByJson<List<TEntity>>();
        },
        parameters);
    }

    /// <summary>
    /// 执行命令。
    /// </summary>
    /// <typeparam name="TResult">指定的返回类型。</typeparam>
    /// <param name="sql">给定要执行的 SQL 语句。</param>
    /// <param name="func">给定要执行的命令结果方法。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="DbCommand.Connection"/> is null.
    /// </exception>
    protected virtual TResult ExecuteCommand<TResult>(string sql,
        Func<DbCommand, TResult> func, DbParameter[]? parameters = null)
    {
        using (var cmd = Database.GetDbConnection().CreateCommand())
        {
            if (cmd.Connection is null)
                throw new ArgumentNullException(nameof(cmd.Connection));

            if (cmd.Connection.State == ConnectionState.Broken)
                cmd.Connection.Close();

            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();

            cmd.CommandText = sql;

            if (parameters is not null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            return func(cmd);
        }
    }


    /// <summary>
    /// 异步执行 SQL 语句成功。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含是否成功的布尔值的异步操作。</returns>
    public virtual Task<bool> ExecuteSuccessAsync(string sql,
        DbParameter[]? parameters = null, CancellationToken cancellationToken = default)
        => ExecuteCommandAsync(sql, async cmd => await cmd.ExecuteNonQueryAsync(cancellationToken) > 0,
            parameters, cancellationToken);

    /// <summary>
    /// 通过异步执行 SQL 语句查询单行单例的单个标量对象。
    /// </summary>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含对象的异步操作。</returns>
    public virtual Task<object?> ExecuteScalarAsync(string sql,
        DbParameter[]? parameters = null, CancellationToken cancellationToken = default)
        => ExecuteCommandAsync(sql, cmd => cmd.ExecuteScalarAsync(cancellationToken),
            parameters, cancellationToken);

    /// <summary>
    /// 通过异步执行 SQL 语句查询实体列表。
    /// </summary>
    /// <typeparam name="TEntity">指定的实体类型。</typeparam>
    /// <param name="sql">给定的 SQL 语句。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <see cref="List{TEntity}"/> 的异步操作。</returns>
    public virtual Task<List<TEntity>?> ExecuteListAsync<TEntity>(string sql,
        DbParameter[]? parameters = null, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        var entityType = Model.FindEntityType(typeof(TEntity));
        if (entityType is not null)
        {
            sql = DataOptions.Access.FormatSchema(sql, entityType.GetSchema());
            sql = DataOptions.Access.FormatTableName(sql, entityType.GetTableName());
        }

        return ExecuteCommandAsync(sql, async cmd =>
        {
            var list = new List<IDictionary<string, object>>();

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync(cancellationToken))
                {
                    var row = new Dictionary<string, object>();

                    for (var i = 0; i < reader.FieldCount; i++)
                        row.Add(reader.GetName(i), reader.GetValue(i));

                    list.Add(row);
                }
            }

            return list.AsByJson<List<TEntity>>();
        },
        parameters, cancellationToken);
    }

    /// <summary>
    /// 异步执行命令。
    /// </summary>
    /// <typeparam name="TResult">指定的返回类型。</typeparam>
    /// <param name="sql">给定要执行的 SQL 语句。</param>
    /// <param name="func">给定要执行的命令结果异步方法。</param>
    /// <param name="parameters">给定的参数数组（可选）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含 <typeparamref name="TResult"/> 的异步操作。</returns>
    /// <exception cref="ArgumentNullException">
    /// <see cref="DbCommand.Connection"/> is null.
    /// </exception>
    protected virtual async Task<TResult> ExecuteCommandAsync<TResult>(string sql,
        Func<DbCommand, Task<TResult>> func, DbParameter[]? parameters = null,
        CancellationToken cancellationToken = default)
    {
        using (var cmd = Database.GetDbConnection().CreateCommand())
        {
            if (cmd.Connection is null)
                throw new ArgumentNullException(nameof(cmd.Connection));

            if (cmd.Connection.State == ConnectionState.Broken)
                await cmd.Connection.CloseAsync();

            if (cmd.Connection.State != ConnectionState.Open)
                await cmd.Connection.OpenAsync(cancellationToken);

            cmd.CommandText = sql;

            if (parameters is not null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            return await func(cmd);
        }
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
        => base.Set<TEntity>().Exists(predicate, checkLocal);

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
        => base.Set<TEntity>().ExistsAsync(predicate, checkLocal, cancellationToken);

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

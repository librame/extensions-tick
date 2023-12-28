#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatchers;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

/// <summary>
/// 定义抽象实现 <see cref="IShardingContext"/> 的分片上下文。
/// </summary>
public abstract class AbstractShardingContext : IShardingContext
{
    private bool _isInitialized;


    /// <summary>
    /// 构造一个 <see cref="AbstractShardingContext"/>。
    /// </summary>
    /// <param name="finder">给定的 <see cref="IShardingFinder"/>。</param>
    /// <param name="settingProvider">给定的 <see cref="IShardingSettingProvider"/>。</param>
    /// <param name="strategyProvider">给定的 <see cref="IShardingStrategyProvider"/>。</param>
    /// <param name="dispatcherFactory">给定的 <see cref="IDispatcherFactory"/>。</param>
    protected AbstractShardingContext(IShardingFinder finder,
        IShardingSettingProvider settingProvider,
        IShardingStrategyProvider strategyProvider,
        IDispatcherFactory dispatcherFactory)
    {
        Finder = finder;
        SettingProvider = settingProvider;
        StrategyProvider = strategyProvider;
        DispatcherFactory = dispatcherFactory;
    }


    /// <summary>
    /// 分片查找器。
    /// </summary>
    public IShardingFinder Finder { get; init; }

    /// <summary>
    /// 设置提供程序。
    /// </summary>
    public IShardingSettingProvider SettingProvider { get; init; }

    /// <summary>
    /// 策略提供程序。
    /// </summary>
    public IShardingStrategyProvider StrategyProvider { get; init; }

    /// <summary>
    /// 调度器工厂。
    /// </summary>
    public IDispatcherFactory DispatcherFactory { get; init; }


    /// <summary>
    /// 初始化数据上下文。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    public virtual IShardingContext Initialize(IDataContext context)
    {
        if (!_isInitialized)
        {
            InitializeCore(context);

            _isInitialized = true;
        }

        return this;
    }

    /// <summary>
    /// 初始化分片上下文核心。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    protected virtual void InitializeCore(IDataContext context)
    {
        Finder.FindTables(context);
    }


    /// <summary>
    /// 对存取器的进行分库。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    /// <returns>返回分库的连接字符串。</returns>
    public abstract string? ShardingDatabase(IDataContext context);

    /// <summary>
    /// 对存取器的表集合分表。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDataContext"/>。</param>
    /// <returns>返回需要分表的字典集合。</returns>
    public abstract Dictionary<ShardingDescriptor, List<ShardingItemSetting>>? ShardingTables(IDataContext context);


    ///// <summary>
    ///// 对存取器的数据库分片。
    ///// </summary>
    ///// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    ///// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    ///// <param name="shardedAction">给定已分片的动作（可选）。</param>
    ///// <returns>返回 <see cref="ShardingDatabaseSetting"/>。</returns>
    //public virtual ShardingDatabaseSetting ShardDatabase(IAccessor accessor, out ShardingDescriptor descriptor,
    //    Action<ShardingDescriptor, ShardingDatabaseSetting>? shardedAction = null)
    //{
    //    //var sharded = accessor.AccessorDescriptor?.Sharded;
    //    //ArgumentNullException.ThrowIfNull(sharded, nameof(accessor));

    //    //descriptor = Tracker.GetOrAddDescriptor(accessor,
    //    //    key => new(sharded, StrategyProvider.GetStrategy));

    //    //descriptor = Finder.FindDatabase(accessor.CurrentContext)!;
    //    //var shardedName = descriptor.GenerateShardedName(accessor.CurrentContext);

    //    ShardingItemSetting? itemSetting = null;
    //    if (!SettingProvider.DatabaseRoot.TryGetDatabase(accessor.AccessorType, out var databaseSetting))
    //    {
    //        databaseSetting = ShardingDatabaseSetting.Create(descriptor, shardedName, string.Empty,
    //            accessor.CurrentConnectionString!, out itemSetting);

    //        SettingProvider.SaveDatabaseRoot();
    //    }
    //    else
    //    {
    //        itemSetting = databaseSetting.GetOrAddItem(shardedName, string.Empty,
    //            accessor.CurrentConnectionString!, () => SettingProvider.SaveDatabaseRoot());
    //    }

    //    // 绑定存取器
    //    itemSetting.Source = accessor;

    //    if (itemSetting.IsNeedSharding)
    //    {
    //        // 从数据库连接字符串提取数据库名称（不一定是原始名称）
    //        var connectionString = accessor.CurrentConnectionString!;
    //        var database = connectionString.ParseDatabaseFromConnectionString();

    //        // 切换为分片数据连接
    //        accessor.ChangeConnection(connectionString.Replace(database, shardedName));

    //        shardedAction?.Invoke(descriptor, databaseSetting);
    //    }

    //    return databaseSetting;
    //}

    ///// <summary>
    ///// 异步对存取器的数据库分片。
    ///// </summary>
    ///// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    ///// <param name="shardedAction">给定已分片的动作（可选）。</param>
    ///// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    ///// <returns>返回一个包含 <see cref="ShardingDatabaseSetting"/> 与 <see cref="ShardingDescriptor"/> 元组的异步操作。</returns>
    //public virtual async Task<(ShardingDatabaseSetting databaseSetting, ShardingDescriptor descriptor)> ShardDatabaseAsync(IAccessor accessor,
    //    Action<ShardingDescriptor, ShardingDatabaseSetting>? shardedAction = null, CancellationToken cancellationToken = default)
    //{
    //    //var sharded = accessor.AccessorDescriptor?.Sharded;
    //    //ArgumentNullException.ThrowIfNull(sharded, nameof(accessor));

    //    //var descriptor = Tracker.GetOrAddDescriptor(accessor,
    //    //    key => new(sharded, StrategyProvider.GetStrategy));

    //    //var descriptor = accessor.ShardingDescriptor!;
    //    //var shardedName = descriptor.GenerateShardedName(accessor.CurrentContext);

    //    //ShardingItemSetting? itemSetting = null;
    //    //if (!SettingProvider.DatabaseRoot.TryGet(accessor.AccessorType, out var databaseSetting))
    //    //{
    //    //    databaseSetting = ShardingDatabaseSetting.Create(descriptor, shardedName, string.Empty,
    //    //        accessor.CurrentConnectionString!, out itemSetting);

    //    //    SettingProvider.SaveDatabaseRoot();
    //    //}
    //    //else
    //    //{
    //    //    itemSetting = databaseSetting.GetOrAddItem(shardedName, string.Empty,
    //    //        accessor.CurrentConnectionString!, () => SettingProvider.SaveDatabaseRoot());
    //    //}

    //    //// 绑定存取器
    //    //itemSetting.Source = accessor;

    //    //if (itemSetting.IsNeedSharding)
    //    //{
    //    //    // 从数据库连接字符串提取数据库名称（不一定是原始名称）
    //    //    var connectionString = accessor.CurrentConnectionString!;
    //    //    var database = connectionString.ParseDatabaseFromConnectionString();

    //    //    // 切换为分片数据连接
    //    //    await accessor.ChangeConnectionAsync(connectionString.Replace(database, shardedName), cancellationToken);

    //    //    shardedAction?.Invoke(descriptor, databaseSetting);
    //    //}

    //    return (default, default);
    //}


    ///// <summary>
    ///// 对实体的数据表分片。
    ///// </summary>
    ///// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    ///// <param name="entityType">给定的实体类型。</param>
    ///// <param name="entity">给定的实体对象。</param>
    ///// <param name="originalName">给定的原始名称。</param>
    ///// <param name="descriptor">输出 <see cref="ShardingDescriptor"/>。</param>
    ///// <param name="shardedAction">给定已分片的动作（可选）。</param>
    ///// <returns>返回 <see cref="ShardingTableSetting"/>。</returns>
    //public virtual ShardingTableSetting ShardTable(IAccessor accessor, Type entityType, object? entity, string originalName,
    //    out ShardingDescriptor descriptor, Action<ShardingDescriptor, ShardingTableSetting>? shardedAction = null)
    //{
    //    //descriptor = Tracker.GetOrAddDescriptor(entityType, key =>
    //    //{
    //    //    var attribute = ShardingAttribute.Get(entityType);
    //    //    ArgumentNullException.ThrowIfNull(attribute, nameof(entityType));

    //    //    var descr = new ShardingDescriptor(attribute, StrategyProvider.GetStrategy);
    //    //    return descr; 
    //    //});

    //    //ArgumentNullException.ThrowIfNull(entity);

    //    var descriptors = Finder.FindTables(accessor.CurrentContext)!;
    //    descriptor = descriptors.Single(p => p.ShardedType == entityType);

    //    //var identifier = entity as IObjectIdentifier;

    //    //// 解析实体对象的分片值
    //    ////var value = ParseShardingValue(entityType, entity);
    //    //var shardedName = descriptor.GenerateShardedName(entity);

    //    //ShardingItemSetting? itemSetting = null;
    //    //if (!SettingProvider.TableRoot.TryGet(entityType, out var tableSetting))
    //    //{
    //    //    tableSetting = ShardingTableSetting.Create(descriptor, shardedName, originalName,
    //    //        identifier, out itemSetting);

    //    //    SettingProvider.SaveDatabaseRoot();
    //    //}
    //    //else
    //    //{
    //    //    itemSetting = tableSetting.GetOrAddItem(shardedName, originalName, identifier,
    //    //        () => SettingProvider.SaveDatabaseRoot());
    //    //}

    //    //// 绑定实体对象
    //    //itemSetting.Source = entity;

    //    //if (itemSetting.IsNeedSharding)
    //    //{
    //    //    shardedAction?.Invoke(descriptor, tableSetting);
    //    //}

    //    return default;
    //}

    ///// <summary>
    ///// 解析实体对象的分片值。
    ///// </summary>
    ///// <param name="entityType">给定的实体类型。</param>
    ///// <param name="entity">给定的实体对象。</param>
    ///// <returns>返回 <see cref="IShardingValue"/>。</returns>
    ///// <exception cref="NotImplementedException">
    ///// The entity type is not implemented sharding value.
    ///// </exception>
    //protected virtual IShardingValue ParseShardingValue(Type entityType, object entity)
    //{
    //    // 如果使用继承接口实现
    //    if (entity is IShardingValue value)
    //        return value;

    //    // 如果使用 DbContext 实体配置实现
    //    if (Tracker.TryGetEntityValue(entityType, out var values))
    //        return new CompositeShardingValue(values.AsShardingValues(entity));

    //    throw new NotImplementedException($"The entity type '{entityType}' is not implemented sharding value.");
    //}

}

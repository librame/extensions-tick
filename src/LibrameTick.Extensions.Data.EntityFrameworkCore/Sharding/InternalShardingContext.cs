#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatching;
using Librame.Extensions.Mapping;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

internal sealed class InternalShardingContext(IShardingFinder finder,
    IShardingSettingManager settingProvider,
    IShardingStrategyProvider strategyProvider,
    IDispatcherFactory dispatcherFactory)
    : AbstractShardingContext(finder, settingProvider, strategyProvider, dispatcherFactory)
{
    private static Func<StateManager, IKey?, IIdentityMap?>? _findIdentityMapFunc;


    public override string? ShardingDatabase(IDataContext context)
    {
        var descriptor = context.Services.InitialShardingDescriptor;
        if (descriptor is null) return null;

        var initialConnectionString = context.Services.InitialConnectionString;
        ArgumentException.ThrowIfNullOrWhiteSpace(initialConnectionString);

        var initialDatabaseName = context.Services.InitialDatabaseName;
        ArgumentException.ThrowIfNullOrWhiteSpace(initialDatabaseName);

        var shardingDatabaseName = descriptor.GenerateShardingName(newShardingValues: null);
        var newConnectionString = initialConnectionString.Replace(initialDatabaseName, shardingDatabaseName);

        SettingManager.GetOrCreateDatabase(descriptor, shardingDatabaseName, newConnectionString, context);

        return newConnectionString;
    }


    public override Dictionary<ShardingDescriptor, List<ShardingItemSetting>>? ShardingTables(IDataContext context)
    {
        var descriptors = Finder.FindTables(context);
        if (descriptors is null) return null;

        var descriptorEntities = GetShardingEntities(context.Services, descriptors);
        if (descriptorEntities is null || descriptorEntities.Count < 1) return null;

        return SaveShardingSettings(context, descriptorEntities);
    }

    private Dictionary<ShardingDescriptor, List<ShardingItemSetting>> SaveShardingSettings(
        IDataContext context, Dictionary<ShardingDescriptor, List<object>> descriptorEntities)
    {
        ModuleBuilder? moduleBuilder = null;
        var tableSetting = SettingManager.TableProvider.CurrentSetting;

        var descriptorSettings = new Dictionary<ShardingDescriptor, List<ShardingItemSetting>>();
        foreach (var (descriptor, entities) in descriptorEntities)
        {
            var settings = new List<ShardingItemSetting>();

            foreach (var entity in entities)
            {
                var isSharded = false;

                var shardingName = descriptor.GenerateShardingName(entity);
                var identifier = (entity as IObjectIdentifier)?.GetObjectId().ToString();

                var (_, setting) = tableSetting.GetOrCreate(descriptor,
                    shardingName, identifier, entity, () => isSharded = true);

                if (isSharded)
                {
                    moduleBuilder ??= CreateShardingModuleBuilder(context);

                    var shardedType = moduleBuilder.DeriveType(descriptor.SourceType, shardingName);
                    var sharded = ObjectMapper.NewByMapAllPublicProperties(entity, shardedType);

                    setting.ChangeSharded(sharded, shardedType);

                    settings.Add(setting);
                }
            }

            if (settings.Count > 0)
            {
                descriptorSettings.Add(descriptor, settings);
            }
        }

        if (descriptorSettings.Count > 0)
        {
            SettingManager.TableProvider.SaveChanges();
        }

        return descriptorSettings;
    }

    private static Dictionary<ShardingDescriptor, List<object>>? GetShardingEntities(
        IDataContextServices services, IReadOnlyList<ShardingDescriptor> descriptors)
    {

#pragma warning disable EF1001 // Internal EF Core API usage.

        _findIdentityMapFunc ??= "FindIdentityMap".GetMethodFuncByExpression<StateManager, IKey?, IIdentityMap?>();

        var stateManager = (StateManager)services.GetContextService<IStateManager>();

        // 只针对新增数据处理分表
        var addedEntities = stateManager.GetEntriesForState(added: true).ToList();
        if (addedEntities.Count < 1) return null;

        var descriptorEntities = new Dictionary<ShardingDescriptor, List<object>>(
            KeyEqualityComparer<ShardingDescriptor>.CreateBy(p => p.Attribute.ToString()));

        foreach (var descriptor in descriptors)
        {
            var shardingEntities = addedEntities.Where(p => p.EntityType.ClrType == descriptor.SourceType).ToList();
            if (shardingEntities.Count > 0)
            {
                descriptorEntities.Add(descriptor, shardingEntities.Select(s => s.Entity).ToList());

                // 从上下文中移除要分表的新增数据的标识集合
                var entityType = shardingEntities.First().EntityType;
                foreach (var key in entityType.GetKeys())
                {
                    var identityMap = _findIdentityMapFunc(stateManager, key);
                    identityMap?.Clear();
                }

                // 从上下文中移除要分表的新增数据集合
                foreach (var entity in shardingEntities)
                {
                    entity.SetEntityState(EntityState.Detached);
                }
            }
        }

        return descriptorEntities;

#pragma warning restore EF1001 // Internal EF Core API usage.

    }

    private static ModuleBuilder CreateShardingModuleBuilder(IDataContext context)
    {
        string shardingAssemblyName;

        if (context.Services is DataContextServices services)
        {
            shardingAssemblyName = services.DataOptions.Sharding.DefaultShardingAssemblyNameFactory(context.ContextType);
        }
        else
        {
            shardingAssemblyName = $"{context.ContextType.Assembly.GetName().Name}_Sharding_{DateTime.Now.Ticks}";
        }

        return shardingAssemblyName.DefineDynamicModule();
    }

}

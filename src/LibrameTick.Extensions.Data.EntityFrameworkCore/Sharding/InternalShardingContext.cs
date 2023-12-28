#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Dispatchers;
using Librame.Extensions.Setting;

namespace Librame.Extensions.Data.Sharding;

internal sealed class InternalShardingContext(IShardingFinder finder,
    IShardingSettingProvider settingProvider,
    IShardingStrategyProvider strategyProvider,
    IDispatcherFactory dispatcherFactory)
    : AbstractShardingContext(finder, settingProvider, strategyProvider, dispatcherFactory)
{

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

        SettingProvider.GetOrCreateDatabase(descriptor, shardingDatabaseName, newConnectionString, context);

        return newConnectionString;
    }


    public override Dictionary<ShardingDescriptor, List<ShardingItemSetting>>? ShardingTables(IDataContext context)
    {
        var descriptors = Finder.FindTables(context);
        if (descriptors is null) return null;

        var descriptorEntities = GetShardingEntities(context.Services, descriptors);
        if (descriptorEntities is null || descriptorEntities.Count < 1) return null;

        var setting = SettingProvider.TableRoot;

        var descriptorSettings = new Dictionary<ShardingDescriptor, List<ShardingItemSetting>>();
        foreach (var descriptor in descriptorEntities)
        {
            var itemSettings = new List<ShardingItemSetting>();

            foreach (var entity in descriptor.Value)
            {
                var isSharded = false;

                var shardingName = descriptor.Key.GenerateShardingName(entity);
                var identifier = (entity as IObjectIdentifier)?.GetObjectId().ToString();

                var (_, itemSetting) = setting.GetOrCreate(descriptor.Key,
                    shardingName, identifier, entity, () => isSharded = true);

                if (isSharded)
                {
                    itemSettings.Add(itemSetting);
                }
            }

            if (itemSettings.Count > 0)
            {
                descriptorSettings.Add(descriptor.Key, itemSettings);
            }
        }

        if (descriptorSettings.Count > 0)
        {
            SettingProvider.SaveTableRoot();
        }

        return descriptorSettings;
    }


    private static Dictionary<ShardingDescriptor, List<object>>? GetShardingEntities(
        IDataContextServices services, IReadOnlyList<ShardingDescriptor> descriptors)
    {

#pragma warning disable EF1001 // Internal EF Core API usage.

        var stateManager = services.GetContextService<IStateManager>();

        // 只针对新增数据处理分表
        var addedEntities = stateManager.GetEntriesForState(added: true).ToList();
        if (addedEntities.Count < 1) return null;

        var descriptorEntities = new Dictionary<ShardingDescriptor, List<object>>(
            PropertyEqualityComparer<ShardingDescriptor>.Create(p => p.Attribute.ToString()));

        foreach (var descriptor in descriptors)
        {
            var shardingEntities = addedEntities.Where(p => p.EntityType.ClrType == descriptor.SourceType)
                .Select(s => s.Entity)
                .ToList();

            if (shardingEntities.Count > 0)
            {
                descriptorEntities.Add(descriptor, shardingEntities);
            }
        }

        return descriptorEntities;

#pragma warning restore EF1001 // Internal EF Core API usage.

    }

}

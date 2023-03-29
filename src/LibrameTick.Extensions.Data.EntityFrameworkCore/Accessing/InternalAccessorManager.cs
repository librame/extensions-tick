#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

class InternalAccessorManager : IAccessorManager
{
    private readonly IOptionsMonitor<DataExtensionOptions> _optionsMonitor;


    public InternalAccessorManager(IOptionsMonitor<DataExtensionOptions> optionsMonitor,
        IAccessorMigrator migrator, IAccessorResolver resolver, IShardingManager shardingManager)
    {
        _optionsMonitor = optionsMonitor;
        Migrator = migrator;
        ShardingManager = shardingManager;

        var accessors = resolver.ResolveAccessors();
        if (accessors.Count < 1)
            throw new ArgumentNullException($"The accessors not found, verify that accessor extensions are registered. ex. \"services.AddDbContext<TContext>(opts => opts.UseXXX<Database>().UseAccessor());\"");

        ResolvedAccessors = accessors;
    }


    public DataExtensionOptions Options
        => _optionsMonitor.CurrentValue;


    public IAccessorMigrator Migrator { get; init; }

    public IShardingManager ShardingManager { get; init; }


    public IReadOnlyList<IAccessor> ResolvedAccessors { get; init; }

    public IReadOnlyDictionary<IAccessor, ShardedDescriptor?>? CurrentAccessors { get; private set; }


    public IAccessor GetAccessor(AccessorSpec specification)
    {
        if (Options.Access.AutoMigration)
            Migrator.Migrate(ResolvedAccessors);

        var filterAccessors = ResolvedAccessors.Where(specification.IsSatisfiedBy);
        if (!filterAccessors.Any())
            throw new ArgumentNullException($"The filter accessors not found.");

        // 尝试对过滤存取器分库
        var currentAccessors = new ConcurrentDictionary<IAccessor, ShardedDescriptor?>();
        foreach (var filterAccessor in filterAccessors)
        {
            ShardingManager.ShardDatabase(filterAccessor, out var descriptor);

            currentAccessors.TryAdd(filterAccessor, descriptor);
        }
        CurrentAccessors = currentAccessors;

        var mirroringAccessors = filterAccessors.Where(specification.IsMirroringRedundancyMode);
        var stripingAccessors = filterAccessors.Where(specification.IsStripingRedundancyMode);

        var allAccessors = new List<IAccessor>();

        if (mirroringAccessors.Any())
            allAccessors.Add(new MirroringAccessors(mirroringAccessors, Options.Access.Dispatcher));

        if (stripingAccessors.Any())
            allAccessors.Add(new StripingAccessors(stripingAccessors, Options.Access.Dispatcher));

        return new CompositingAccessors(allAccessors, Options.Access.Dispatcher);
    }

    public IAccessor GetReadAccessor(AccessorSpec? specification = null)
        => GetAccessor(specification ?? new ReadAccessorSpec());

    public IAccessor GetWriteAccessor(AccessorSpec? specification = null)
        => GetAccessor(specification ?? new WriteAccessorSpec());

}

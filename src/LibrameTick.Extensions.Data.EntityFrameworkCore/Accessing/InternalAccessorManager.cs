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


    private ConcurrentDictionary<IAccessor, ShardedDescriptor?> AddAccessor(IAccessor accessor,
        ConcurrentDictionary<IAccessor, ShardedDescriptor?> dictionary)
    {
        // 尝试对存取器分库
        ShardingManager.ShardDatabase(accessor, out var descriptor);

        // 如果分库，则尝试迁移表
        if (Options.Access.AutoMigration)
            Migrator.Migrate(accessor);

        dictionary.TryAdd(accessor, descriptor);

        return dictionary;
    }

    public IDispatchableAccessors GetAccessor(ISpecification<IAccessor> specification)
    {
        // 尝试对过滤存取器分库
        var currentAccessors = new ConcurrentDictionary<IAccessor, ShardedDescriptor?>();

        // 如果使用命名规约，则取唯一名称存取器，非唯一抛出异常
        if (specification is NamedAccessorSpecification namedAccessorSpecification)
        {
            var singleAccessor = ResolvedAccessors.Single(namedAccessorSpecification.IsSatisfiedBy);

            CurrentAccessors = AddAccessor(singleAccessor, currentAccessors);

            return new CompositingDispatchableAccessors(CurrentAccessors.Keys, ShardingManager.DispatcherFactory);
        }

        var filterAccessors = ResolvedAccessors.Where(specification.IsSatisfiedBy);
        if (!filterAccessors.Any())
            throw new ArgumentNullException($"The filter accessors not found.");

        foreach (var filterAccessor in filterAccessors)
        {
            AddAccessor(filterAccessor, currentAccessors);
        }

        CurrentAccessors = currentAccessors;

        if (specification is AccessAccessorSpecification accessorSpecification)
        {
            var mirroringAccessors = filterAccessors.Where(accessorSpecification.IsMirroringRedundancyMode);
            var stripingAccessors = filterAccessors.Where(accessorSpecification.IsStripingRedundancyMode);

            var allAccessors = new List<IAccessor>();

            if (mirroringAccessors.Any())
                allAccessors.Add(new CompositingDispatchableAccessors(mirroringAccessors, ShardingManager.DispatcherFactory));

            if (stripingAccessors.Any())
                allAccessors.Add(new CompositingDispatchableAccessors(stripingAccessors, ShardingManager.DispatcherFactory));

            return new CompositingDispatchableAccessors(allAccessors, ShardingManager.DispatcherFactory);
        }
        else
        {
            return new CompositingDispatchableAccessors(CurrentAccessors.Keys, ShardingManager.DispatcherFactory);
        }
    }

    public IDispatchableAccessors GetReadAccessor(ISpecification<IAccessor>? specification = null)
        => GetAccessor(specification ?? new ReadAccessAccessorSpecification());

    public IDispatchableAccessors GetWriteAccessor(ISpecification<IAccessor>? specification = null)
        => GetAccessor(specification ?? new WriteAccessAccessorSpecification());

}

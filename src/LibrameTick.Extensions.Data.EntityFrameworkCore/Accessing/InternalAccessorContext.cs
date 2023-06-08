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
using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Device;
using Librame.Extensions.Dispatchers;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

sealed internal class InternalAccessorContext : IAccessorContext
{
    private readonly IOptionsMonitor<DataExtensionOptions> _options;
    private readonly ConcurrentDictionary<string, IDeviceLoader> _deviceLoaders;


    public InternalAccessorContext(IOptionsMonitor<DataExtensionOptions> options,
        IAccessorMigrator migrator, IAccessorResolver resolver, IShardingContext shardingContext)
    {
        _deviceLoaders = new();
        _options = options;
        Migrator = migrator;
        ShardingContext = shardingContext;

        var accessors = resolver.ResolveAccessors();
        if (accessors.Count < 1)
            throw new ArgumentNullException($"The accessors not found, verify that accessor extensions are registered. ex. \"services.AddDbContext<TContext>(opts => opts.UseXXX<Database>().UseAccessor());\"");

        ResolvedAccessors = accessors;
    }


    public DataExtensionOptions Options
        => _options.CurrentValue;


    public IAccessorMigrator Migrator { get; init; }

    public IShardingContext ShardingContext { get; init; }


    public IReadOnlyList<IAccessor> ResolvedAccessors { get; init; }

    public IReadOnlyDictionary<IAccessor, ShardingDescriptor?>? CurrentAccessors { get; private set; }


    public IDispatcherAccessors GetReadAccessors(ISpecification<IAccessor>? specification = null)
        => GetAccessors(specification ?? new ReadAccessAccessorSpecification());

    public IDispatcherAccessors GetWriteAccessors(ISpecification<IAccessor>? specification = null)
        => GetAccessors(specification ?? new WriteAccessAccessorSpecification());

    public IDispatcherAccessors GetAccessors(ISpecification<IAccessor> specification)
    {
        // 尝试对过滤存取器分库
        var currentAccessors = new ConcurrentDictionary<IAccessor, ShardingDescriptor?>(
            PropertyEqualityComparer<IAccessor>.Create(s => s.AccessorId));

        // 如果使用命名规约，则取唯一名称存取器，非唯一抛出异常
        if (specification is NamedAccessorSpecification namedAccessorSpecification)
        {
            var singleAccessor = ResolvedAccessors.Single(namedAccessorSpecification.IsSatisfiedBy);

            CurrentAccessors = AddAccessor(singleAccessor, currentAccessors);

            return new DefaultDispatcherAccessors(CurrentAccessors.Keys, ShardingContext.DispatcherFactory);
        }

        // 使用筛选规约集合
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
            var mirroringAccessors = filterAccessors.Where(accessorSpecification.IsMirroringDispatching);
            var stripingAccessors = filterAccessors.Where(accessorSpecification.IsStripingDispatching);

            var allAccessors = new List<IDispatcherAccessors>();

            if (mirroringAccessors.Any())
            {
                // 镜像模式支持负载调度
                if (Options.Access.AutoLoad)
                {
                    allAccessors.Add(new MirroringDispatcherAccessors(UpdateHostLoad(mirroringAccessors),
                        ShardingContext.DispatcherFactory));
                }
                else
                {
                    allAccessors.Add(new MirroringDispatcherAccessors(mirroringAccessors, ShardingContext.DispatcherFactory));
                }
            }

            if (stripingAccessors.Any())
                allAccessors.Add(new StripingDispatcherAccessors(stripingAccessors, ShardingContext.DispatcherFactory));

            return new CompositeDispatcherAccessors(allAccessors, ShardingContext.DispatcherFactory);
        }
        else
        {
            return new DefaultDispatcherAccessors(CurrentAccessors.Keys, ShardingContext.DispatcherFactory);
        }
    }

    private ConcurrentDictionary<IAccessor, ShardingDescriptor?> AddAccessor(IAccessor accessor,
        ConcurrentDictionary<IAccessor, ShardingDescriptor?> dictionary)
    {
        if (accessor.AccessorDescriptor?.Sharded is null)
            return dictionary;

        // 尝试对存取器分库
        ShardingContext.ShardDatabase(accessor, out var descriptor);

        // 如果分库，则尝试迁移表
        if (Options.Access.AutoMigration)
            Migrator.Migrate(accessor);

        dictionary.TryAdd(accessor, descriptor);

        return dictionary;
    }

    private IEnumerable<IAccessor> UpdateHostLoad(IEnumerable<IAccessor> hostAccessors)
    {
        if (hostAccessors.NonEnumeratedCount() < 1)
            return hostAccessors;

        var hosts = hostAccessors
            .Select(s => s.AccessorDescriptor!.LoaderHost!)
            .Distinct()
            .AsEnumerable();

        var hostsKey = Options.CreateDeviceLoadHostsKeyFunc(hosts);

        var deviceLoader = _deviceLoaders.GetOrAdd(hostsKey,
            key => Options.CreateDeviceLoaderFunc(Options, hosts));

        var usages = deviceLoader.GetUsages(Options.DeviceLoadRealtimeForEverytime);
        foreach (var usage in usages)
        {
            var priority = usage.CalculateLoad();
            if (priority <= 0)
                continue;

            foreach (var accessor in hostAccessors.Where(p => p.AccessorDescriptor!.LoaderHost == usage.Host))
            {
                // 将原始配置优先级做为权重附加到计算的负载值中
                accessor.SetPriority(accessor.AccessorDescriptor!.Priority + priority);
            }
        }

        return hostAccessors.OrderBy(static ks => ks.GetPriority()); // 越小越优先
    }

}

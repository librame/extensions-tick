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

internal sealed class InternalAccessorContext : IAccessorContext
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


    //public IDispatcherAccessors GetReadAccessors(ISpecification<IAccessor>? specification = null)
    //    => GetAccessors(specification ?? new ReadAccessAccessorSpecification());

    //public IDispatcherAccessors GetWriteAccessors(ISpecification<IAccessor>? specification = null)
    //    => GetAccessors(specification ?? new WriteAccessAccessorSpecification());

    public IDispatcherAccessors GetAccessors(ISpecification<IAccessor> specification)
    {
        IEnumerable<IAccessor> currentAccessors;

        if (ResolvedAccessors.Count == 1)
        {
            currentAccessors = ResolvedAccessors;
        }
        else if (specification is NamedAccessorSpecification namedAccessorSpecification)
        {
            // 如果使用命名规约，则取唯一名称存取器，非唯一抛出异常
            var singleAccessor = ResolvedAccessors.Single(namedAccessorSpecification.IsSatisfiedBy);
            currentAccessors = EnumerableExtensions.AsEnumerable(singleAccessor);
        }
        else
        {
            // 使用自定义规约筛选多个存取器的集合
            var filterAccessors = ResolvedAccessors.Where(specification.IsSatisfiedBy);
            if (!filterAccessors.Any())
                throw new ArgumentNullException($"The filter accessors not found.");

            currentAccessors = filterAccessors;
        }

        CurrentAccessors = ShardingAccessors(currentAccessors);

        if (CurrentAccessors.Count > 1 && specification is AccessAccessorSpecification accessSpecification)
            return ComposeDispatcherAccessors(currentAccessors, accessSpecification);

        return new DefaultDispatcherAccessors(currentAccessors, ShardingContext.DispatcherFactory);
    }

    private IDispatcherAccessors ComposeDispatcherAccessors(IEnumerable<IAccessor> currentAccessors,
        AccessAccessorSpecification accessSpecification)
    {
        var mirroringAccessors = currentAccessors.Where(accessSpecification.IsMirroringDispatching);
        var stripingAccessors = currentAccessors.Where(accessSpecification.IsStripingDispatching);

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

    private IReadOnlyDictionary<IAccessor, ShardingDescriptor?> ShardingAccessors(IEnumerable<IAccessor> accessors)
    {
        var shardingAccessors = new Dictionary<IAccessor, ShardingDescriptor?>(
            PropertyEqualityComparer<IAccessor>.Create(s => s.AccessorId));

        foreach (var accessor in accessors)
        {
            if (accessor.ShardingDescriptor is null)
            {
                shardingAccessors.Add(accessor, null);
            }
            else
            {
                // 尝试对存取器分库
                //ShardingContext.ShardDatabase(accessor, out var descriptor);

                // 如果分库，则尝试迁移表
                if (Options.Access.AutoMigration)
                    Migrator.Migrate(accessor);

                shardingAccessors.TryAdd(accessor, accessor.ShardingDescriptor);
            }
        }

        return shardingAccessors;
    }

    private IEnumerable<IAccessor> UpdateHostLoad(IEnumerable<IAccessor> hostAccessors)
    {
        if (hostAccessors.NonEnumeratedCount() < 1)
            return hostAccessors;

        var hosts = hostAccessors
            .Select(s => s.AccessorDescriptor!.LoaderHost!)
            .Distinct()
            .ToList();

        var hostsKey = string.Join(',', hosts.Order());

        var deviceLoader = _deviceLoaders.GetOrAdd(hostsKey, key => Options.DeviceLoaderFactory(Options, hosts));

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

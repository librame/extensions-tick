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
using Librame.Extensions.Data.Specifications;

namespace Librame.Extensions.Data.Accessing;

class InternalAccessorManager : IAccessorManager
{
    private readonly IOptionsMonitor<DataExtensionOptions> _optionsMonitor;
    private readonly IAccessorMigrator _migrator;
    private readonly IShardingManager _shardingManager;

    private IAccessor? _defaultReadAccessor;
    private IAccessor? _defaultWriteAccessor;


    public InternalAccessorManager(IOptionsMonitor<DataExtensionOptions> optionsMonitor,
        IAccessorMigrator migrator, IAccessorResolver resolver, IShardingManager shardingManager)
    {
        _optionsMonitor = optionsMonitor;
        _migrator = migrator;
        _shardingManager = shardingManager;

        Accessors = resolver.ResolveAccessors();
        if (Accessors.Count < 1)
            throw new ArgumentNullException($"The accessors not found, verify that accessor extensions are registered. ex. \"services.AddDbContext<TContext>(opts => opts.UseXXX<Database>().UseAccessor());\"");
    }


    public DataExtensionOptions Options
        => _optionsMonitor.CurrentValue;


    public IReadOnlyList<IAccessor> Accessors { get; init; }


    public IAccessor GetAccessor(IAccessorSpecification specification)
    {
        if (Options.Access.AutomaticMigration)
            _migrator.Migrate(Accessors);

        var accessor = specification.IssueEvaluate(Accessors);

        return _shardingManager.ShardDatabase(accessor);
    }

    public IAccessor GetReadAccessor(IAccessorSpecification? specification = null)
    {
        if (specification is null)
        {
            if (_defaultReadAccessor is null)
                _defaultReadAccessor = GetAccessor(AccessorSpecifications.Read);

            return _defaultReadAccessor;
        }

        return GetAccessor(specification);
    }

    public IAccessor GetWriteAccessor(IAccessorSpecification? specification = null)
    {
        if (specification is null)
        {
            if (_defaultWriteAccessor is null)
                _defaultWriteAccessor = GetAccessor(AccessorSpecifications.Write);

            return _defaultWriteAccessor;
        }

        return GetAccessor(specification);
    }

}

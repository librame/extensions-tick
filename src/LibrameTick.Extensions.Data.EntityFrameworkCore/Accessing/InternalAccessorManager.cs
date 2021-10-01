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
using Librame.Extensions.Data.Specification;

namespace Librame.Extensions.Data.Accessing;

class InternalAccessorManager : IAccessorManager
{
    private readonly AccessOptions _options;
    private readonly IAccessorMigrator _migrator;
    private readonly IShardingManager _shardingManager;

    private IAccessor? _defaultReadAccessor;
    private IAccessor? _defaultWriteAccessor;


    public InternalAccessorManager(DataExtensionOptions options,
        IAccessorMigrator migrator, IAccessorResolver resolver,
        IShardingManager shardingManager)
    {
        _migrator = migrator;
        _shardingManager = shardingManager;
        _options = options.Access;

        Accessors = resolver.ResolveAccessors();
        if (Accessors.Count < 1)
            throw new ArgumentNullException($"The accessors not found, verify that accessor extensions are registered. ex. \"services.AddDbContext<TContext>(opts => opts.UseXXX<Database>().UseAccessor());\"");
    }


    public IReadOnlyList<IAccessor> Accessors { get; init; }


    public IAccessor GetAccessor(IAccessorSpecification specification)
    {
        if (_options.AutomaticMigration)
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

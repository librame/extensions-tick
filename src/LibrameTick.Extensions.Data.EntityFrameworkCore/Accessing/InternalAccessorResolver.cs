#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

class InternalAccessorResolver : IAccessorResolver
{
    private static readonly Type _accessorType
        = typeof(IAccessor);
    private static readonly Type _dbContextType
        = typeof(DbContext);

    // 限定存取器必需实现 DbContext 和 IAccessor
    private static readonly Func<ServiceDescriptor, bool> _accessorServicePredicate
        = sd => sd.ServiceType.IsAssignableTo(_dbContextType) && sd.ServiceType.IsAssignableTo(_accessorType);

    private readonly DataExtensionBuilder _builder;
    private readonly IServiceProvider _provider;

    private List<IAccessor>? _accessors;


    public InternalAccessorResolver(DataExtensionBuilder builder, IServiceProvider provider)
    {
        _builder = builder;
        _provider = provider;
    }


    public IReadOnlyList<IAccessor> ResolveAccessors()
    {
        if (_accessors is null)
        {
            _accessors = _builder.Services
                .Where(_accessorServicePredicate)
                .Select(s => (IAccessor)_provider.GetRequiredService(s.ServiceType))
                .ToList();
        }
        
        return _accessors;
    }

}

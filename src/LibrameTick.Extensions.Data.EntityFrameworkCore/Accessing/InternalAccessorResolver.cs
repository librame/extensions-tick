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

internal sealed class InternalAccessorResolver : IAccessorResolver
{
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
                .Where(static sd => sd.ServiceType.IsAssignableTo(DataExtensionBuilderExtensions.IAccessorType))
                .Select(s => (IAccessor)_provider.GetRequiredService(s.ServiceType))
                .OrderBy(static a => a.GetPriority())
                .ToList();
        }
        
        return _accessors;
    }

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependency.Internal;

internal sealed class DependencyInitializer<TDependency>(
    Func<IDependencyContext, DependencyCharacteristic, TDependency> initialFunc)
    : IDependencyInitializer<TDependency>
    where TDependency : IDependency
{

    public TDependency Initialize(IDependencyContext context, DependencyCharacteristic characteristic)
    {
        var dependency = initialFunc(context, characteristic);
        return dependency;
    }

}

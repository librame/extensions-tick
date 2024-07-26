#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure.Dependency;

namespace Librame.Extensions.Serialization.Internal;

internal sealed class JsonDependencyInitializer : IDependencyInitializer<IJsonDependency>
{

    public IJsonDependency Initialize(IDependencyContext context, DependencyCharacteristic characteristic)
        => new JsonDependency();

}

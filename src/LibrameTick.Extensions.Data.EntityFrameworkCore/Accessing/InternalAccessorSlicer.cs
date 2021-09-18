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

class InternalAccessorSlicer : AbstractAccessorSlicer<IAccessor>
{
    protected override IAccessor CreateSharding(IEnumerable<IAccessor> accessors)
        => accessors.First();

}

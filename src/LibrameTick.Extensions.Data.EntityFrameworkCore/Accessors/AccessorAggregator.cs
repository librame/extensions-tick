#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data.Accessors
{
    class AccessorAggregator : AbstractAccessorAggregator<IAccessor>
    {
        protected override IAccessor CreateChain(IEnumerable<IAccessor> accessors)
            => new CompositeAccessor(accessors);

    }
}

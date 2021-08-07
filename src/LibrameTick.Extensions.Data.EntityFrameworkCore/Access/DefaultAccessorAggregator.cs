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

namespace Librame.Extensions.Data.Access
{
    class DefaultAccessorAggregator : AbstractAccessorAggregator<IAccessor>
    {
        protected override IAccessor CreateChain(IEnumerable<IAccessor> accessors,
            AccessorInteraction interaction)
            => new CompositeAccessor(accessors, interaction);

    }
}

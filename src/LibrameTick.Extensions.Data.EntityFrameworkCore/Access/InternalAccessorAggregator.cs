#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Access
{
    class InternalAccessorAggregator : AbstractAccessorAggregator<IAccessor>
    {
        protected override IAccessor CreateChain(IEnumerable<IAccessor> accessors,
            AccessorInteraction interaction)
            => new InternalCompositeAccessor(accessors, interaction);

    }
}

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
using System.Linq;

namespace Librame.Extensions.Data.Accessors
{
    class DefaultAccessorSlicer : AbstractAccessorSlicer<IAccessor>
    {
        protected override IAccessor CreateSharding(IEnumerable<IAccessor> accessors)
            => accessors.First();

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Builders
{
    using Options;

    public class CoreExtensionBuilder : AbstractExtensionBuilder<CoreExtensionOptions>
    {
        public CoreExtensionBuilder(CoreExtensionOptions options)
            : base(options)
        {
        }


    }
}

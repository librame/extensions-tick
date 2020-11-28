using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Builders
{
    using Options;

    public abstract class AbstractExtensionBuilder<TOptions> : AbstractExtensionBuilder, IExtensionBuilder<TOptions>
        where TOptions : IExtensionOptions
    {
        public AbstractExtensionBuilder(TOptions options)
            : base(options)
        {
            Options = options;
        }


        public new TOptions Options { get; }
    }


    public abstract class AbstractExtensionBuilder : IExtensionBuilder
    {
        public AbstractExtensionBuilder(IExtensionOptions options)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            Options = options;
        }


        public IExtensionOptions Options { get; }
    }
}

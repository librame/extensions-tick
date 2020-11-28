using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Builders
{
    using Options;

    public interface IExtensionBuilder<TOptions> : IExtensionBuilder
        where TOptions : IExtensionOptions
    {
        new TOptions Options { get; }
    }


    public interface IExtensionBuilder
    {
        IExtensionOptions Options { get; }
    }
}

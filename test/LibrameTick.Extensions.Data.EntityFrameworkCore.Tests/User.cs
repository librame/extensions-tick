using System;

namespace Librame.Extensions.Data
{
    public class User : AbstractCreationIdentifier<long, long>
    {
        public virtual string? Name { get; set; }

        public virtual string? Passwd { get; set; }
    }
}

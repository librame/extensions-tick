using System;

namespace Librame.Extensions.Proxy
{
    public interface ITestCreation
    {
        string? CurrentName { get; set; }

        string Create(string name);
    }
}

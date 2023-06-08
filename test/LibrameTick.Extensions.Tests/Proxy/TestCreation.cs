using System;

namespace Librame.Extensions.Proxy
{
    public class TestCreation : ITestCreation
    {
        public string? CurrentName { get; set; }

        public string Create(string name)
            => $"Hello {name}!";

    }

}

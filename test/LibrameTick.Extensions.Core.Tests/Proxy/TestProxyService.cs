using System;

namespace Librame.Extensions.Proxy
{
    public interface ITestProxyService
    {
        string? GetName();
    }

    public class TestProxyService : ITestProxyService
    {

        [TestInterception]
        public string? GetName()
            => nameof(TestProxyService);

    }
}

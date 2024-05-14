using System;
using Librame.Extensions.Infrastructure.Proxy;

namespace Librame.Extensions.Proxy
{
    public class TestCreationInterceptor : MethodInterceptor
    {
        public ITestCreation? Creation { get; set; }


        protected override void BeforeInvoke(IInvocation invocation)
        {
            Creation = (ITestCreation)Source!;

            Creation.CurrentName = invocation.Method.Args?[0]?.ToString() + " Peng";
        }

        protected override void ExceptionInvoke(IInvocation invocation, Exception exception)
        {
        }

        protected override void AfterInvoke(IInvocation invocation)
        {
        }

    }
}

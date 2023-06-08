using System;

namespace Librame.Extensions.Proxy
{
    public class TestCreationInterceptor : MethodInterceptor
    {
        public ITestCreation? Creation { get; set; }


        protected override void PreProceed(IInvocation invocation)
        {
            Creation = (ITestCreation)Source!;

            Creation.CurrentName = invocation.Method.Args?[0]?.ToString() + " Peng";
        }

        protected override void ExceptionProceed(IInvocation invocation, Exception exception)
        {
        }

        protected override void PostProceed(IInvocation invocation)
        {
        }

    }
}

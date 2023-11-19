using System;

namespace Librame.Extensions.Proxy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestInterceptionAttribute : InterceptionAttribute
    {
        public override void PreProcess(IInvocation invocation)
            => ProxyDecoratorInjectionTests.PreActionMessage = "This is pre-process.";

        public override void PostProcess(IInvocation invocation)
            => ProxyDecoratorInjectionTests.PostActionMessage = "This is post-process.";

        public override void ExceptionProcess(IInvocation invocation)
        {
            //
        }

    }
}

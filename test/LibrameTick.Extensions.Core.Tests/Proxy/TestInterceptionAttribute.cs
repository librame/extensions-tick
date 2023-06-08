using System;

namespace Librame.Extensions.Proxy
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestInterceptionAttribute : InterceptionAttribute
    {
        public override void PreAction(IInvocation invocation)
            => ProxyDecoratorInjectionTests.PreActionMessage = "This is pre invoked.";

        public override void PostAction(IInvocation invocation)
            => ProxyDecoratorInjectionTests.PostActionMessage = "This is post invoked.";

        public override void ExceptionAction(IInvocation invocation)
        {
            //
        }

    }
}

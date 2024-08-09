#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Proxy;

internal class ProxyDecoratorInjection<TInterface> : IProxyDecorator<TInterface>
{
    private IProxyDecorator<TInterface> _decorator;


    public ProxyDecoratorInjection(IProxyGenerator proxy, IInterceptor interceptor, TInterface source)
    {
        ArgumentNullException.ThrowIfNull(source);

        try
        {
            var method = proxy.GetType().GetMethod(nameof(IProxyGenerator.CreateInterfaceProxyDecorator));
            method = method?.MakeGenericMethod(new Type[] { typeof(TInterface), interceptor.GetType() });

            _decorator = (IProxyDecorator<TInterface>)method?.Invoke(proxy, new object[] { source })!;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public TInterface ProxySource => _decorator.ProxySource;

    public IInterceptor Interceptor => _decorator.Interceptor;

    public IInvocation? Invocation => _decorator.Invocation;

    public TInterface CurrentSource => _decorator.CurrentSource;


    public TResult Only<TResult>(System.Linq.Expressions.Expression<Func<TInterface, TResult>> invokeExpression)
        => _decorator.Only(invokeExpression);

}

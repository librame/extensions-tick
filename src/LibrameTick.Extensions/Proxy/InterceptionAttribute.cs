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

/// <summary>
/// 定义一个用于拦截的特性。
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
public abstract class InterceptionAttribute : Attribute
{
    /// <summary>
    /// 预处理。
    /// </summary>
    /// <param name="invocation">给定的 <see cref="IInvocation"/>。</param>
    public abstract void PreAction(IInvocation invocation);

    /// <summary>
    /// 后置处理。
    /// </summary>
    /// <param name="invocation">给定的 <see cref="IInvocation"/>。</param>
    public abstract void PostAction(IInvocation invocation);

    /// <summary>
    /// 异常处理。
    /// </summary>
    /// <param name="invocation">给定的 <see cref="IInvocation"/>。</param>
    public virtual void ExceptionAction(IInvocation invocation)
    {
        //
    }

}

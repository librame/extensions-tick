#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatching;
using Librame.Extensions.Infrastructure.Specification;
using Librame.Extensions.Specification;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义 <see cref="IAccessorContext"/> 静态扩展。
/// </summary>
public static class AccessorContextExtensions
{

    /// <summary>
    /// 获取指定规约的第一个读取存取器。
    /// </summary>
    /// <param name="context">给定的 <see cref="IAccessorContext"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public static IAccessor GetFirstReadAccessor(this IAccessorContext context, ISpecification<IAccessor>? specification = null)
        => context.GetReadAccessors(specification).ReadingDispatcher.FirstSource;

    /// <summary>
    /// 获取指定规约的第一个写入存取器。
    /// </summary>
    /// <param name="context">给定的 <see cref="IAccessorContext"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public static IAccessor GetFirstWriteAccessor(this IAccessorContext context, ISpecification<IAccessor>? specification = null)
        => context.GetWriteAccessors(specification).WritingDispatcher.FirstSource;


    /// <summary>
    /// 获取指定规约的读取调度器存取器集合。
    /// </summary>
    /// <param name="context">给定的 <see cref="IAccessorContext"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IDispatcherAccessors"/>。</returns>
    public static IDispatcherAccessors GetReadAccessors(this IAccessorContext context, ISpecification<IAccessor>? specification = null)
        => context.GetAccessors(specification ?? new ReadAccessAccessorSpecification());

    /// <summary>
    /// 获取指定规约的写入调度器存取器集合。
    /// </summary>
    /// <param name="context">给定的 <see cref="IAccessorContext"/>。</param>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IDispatcherAccessors"/>。</returns>
    public static IDispatcherAccessors GetWriteAccessors(this IAccessorContext context, ISpecification<IAccessor>? specification = null)
        => context.GetAccessors(specification ?? new WriteAccessAccessorSpecification());

}

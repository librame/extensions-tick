#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure.Dispatching;

namespace Librame.Extensions.Dispatching;

/// <summary>
/// 定义一个 <see cref="IDispatcher{TSource}"/> 工厂。
/// </summary>
public interface IDispatcherFactory
{
    /// <summary>
    /// 创建基础调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    IDispatcher<TSource> CreateBase<TSource>(IEnumerable<TSource> sources,
        DispatchingMode mode, DispatchingOptions? options = null)
        where TSource : IEquatable<TSource>;

    /// <summary>
    /// 创建事务调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    IDispatcher<TSource> CreateTransaction<TSource>(IEnumerable<TSource> sources,
        DispatchingMode mode, DispatchingOptions? options = null)
        where TSource : IEquatable<TSource>;

    /// <summary>
    /// 创建复合调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="dispatchers">给定的调度器集合。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    IDispatcher<TSource> CreateCompositing<TSource>(IEnumerable<IDispatcher<TSource>> dispatchers,
        DispatchingOptions? options = null)
        where TSource : IEquatable<TSource>;
}

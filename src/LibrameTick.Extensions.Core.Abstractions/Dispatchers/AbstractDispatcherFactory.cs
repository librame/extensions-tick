#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义抽象实现 <see cref="IDispatcherFactory"/> 的调度器工厂。
/// </summary>
public abstract class AbstractDispatcherFactory : IDispatcherFactory
{
    /// <summary>
    /// 构造一个 <see cref="AbstractDispatcherFactory"/> 实例。
    /// </summary>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>。</param>
    protected AbstractDispatcherFactory(DispatchingOptions options)
    {
        Options = options;
    }


    /// <summary>
    /// 调度选项。
    /// </summary>
    public DispatchingOptions Options { get; init; }


    /// <summary>
    /// 创建基础调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    public virtual IDispatcher<TSource> CreateBase<TSource>(IEnumerable<TSource> sources,
        DispatchingMode mode, DispatchingOptions? options = null)
        where TSource : IEquatable<TSource>
        => new BaseDispatcher<TSource>(sources, mode, options ?? Options);

    /// <summary>
    /// 创建事务调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="mode">给定的 <see cref="DispatchingMode"/>。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    public virtual IDispatcher<TSource> CreateTransaction<TSource>(IEnumerable<TSource> sources,
        DispatchingMode mode, DispatchingOptions? options = null)
        where TSource : IEquatable<TSource>
        => new TransactionDispatcher<TSource>(sources, mode, options ?? Options);

    /// <summary>
    /// 创建复合调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="dispatchers">给定的调度器集合。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    public virtual IDispatcher<TSource> CreateCompositing<TSource>(IEnumerable<IDispatcher<TSource>> dispatchers,
        DispatchingOptions? options = null)
        where TSource : IEquatable<TSource>
        => new CompositeDispatcher<TSource>(dispatchers, options ?? Options);

}

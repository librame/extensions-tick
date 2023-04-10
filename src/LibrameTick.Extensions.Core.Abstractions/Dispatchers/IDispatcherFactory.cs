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
/// 定义一个 <see cref="IDispatcher{TSource}"/> 工厂。
/// </summary>
public interface IDispatcherFactory
{
    /// <summary>
    /// 创建基础调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    IDispatcher<TSource> CreateBase<TSource>(IEnumerable<TSource> sources,
        DispatchingOptions? options = null);

    /// <summary>
    /// 创建事务调度器。
    /// </summary>
    /// <typeparam name="TSource">指定的来源类型。</typeparam>
    /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
    /// <param name="options">给定的 <see cref="DispatchingOptions"/>（可选）。</param>
    /// <returns>返回 <see cref="IDispatcher{TSource}"/>。</returns>
    IDispatcher<TSource> CreateTransaction<TSource>(IEnumerable<TSource> sources,
        DispatchingOptions? options = null);
}

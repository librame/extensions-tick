#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义 <see cref="IDispatcher{IAccessor}"/> 静态扩展。
/// </summary>
public static class DispatcherAccessorExtensions
{

    /// <summary>
    /// 获取必要指定分区的存取器，不存在将抛出异常。
    /// </summary>
    /// <param name="dispatcher">给定的 <see cref="IDispatcher{TSource}"/>。</param>
    /// <param name="partition">给定的分区。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    /// <exception cref="ArgumentException">
    /// The accessor for the given partition partition does not exist.
    /// </exception>
    public static IAccessor GetRequiredPartitionAccessor(this IDispatcher<IAccessor> dispatcher, int partition)
        => dispatcher.GetPartitionAccessor(partition)
            ?? throw new ArgumentException($"The accessor for the given partition '{partition}' does not exist.");

    /// <summary>
    /// 获取指定分区的存取器。
    /// </summary>
    /// <param name="dispatcher">给定的 <see cref="IDispatcher{TSource}"/>。</param>
    /// <param name="partition">给定的分区。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public static IAccessor? GetPartitionAccessor(this IDispatcher<IAccessor> dispatcher, int partition)
        => dispatcher.CurrentSources?.First(p => p.AccessorDescriptor?.Partition == partition);

}

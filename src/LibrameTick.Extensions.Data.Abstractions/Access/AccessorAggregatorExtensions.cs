#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Data.Access
{
    /// <summary>
    /// <see cref="IAccessorAggregator"/> 静态扩展。
    /// </summary>
    public static class AccessorAggregatorExtensions
    {
        /// <summary>
        /// 聚合读取访问器集合。
        /// </summary>
        /// <param name="aggregator">给定的 <see cref="IAccessorAggregator"/>。</param>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public static IAccessor? AggregateReadAccessors(this IAccessorAggregator aggregator,
            IReadOnlyList<AccessorDescriptor> descriptors)
            => aggregator.AggregateAccessors(descriptors, AccessorInteraction.Read | AccessorInteraction.ReadWrite);

        /// <summary>
        /// 聚合写入访问器集合。
        /// </summary>
        /// <param name="aggregator">给定的 <see cref="IAccessorAggregator"/>。</param>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public static IAccessor? AggregateWriteAccessors(this IAccessorAggregator aggregator,
            IReadOnlyList<AccessorDescriptor> descriptors)
            => aggregator.AggregateAccessors(descriptors, AccessorInteraction.Write | AccessorInteraction.ReadWrite);

    }
}

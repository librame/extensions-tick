#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// <see cref="IAccessorSlicer"/> 静态扩展。
    /// </summary>
    public static class AccessorSlicerExtensions
    {
        /// <summary>
        /// 切片读取访问器集合（可用于读/写分库）。
        /// </summary>
        /// <param name="slicer">给定的 <see cref="IAccessorSlicer"/>。</param>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
        /// <param name="customSliceFunc">给定的自定义切片方法（可选；支持参数分库）。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public static IAccessor? SliceReadAccessors(this IAccessorSlicer slicer,
            IReadOnlyList<AccessorDescriptor> descriptors, Func<IAccessor, bool>? customSliceFunc = null)
            => slicer.SliceAccessors(descriptors, AccessorInteraction.Read | AccessorInteraction.ReadWrite, customSliceFunc);

        /// <summary>
        /// 切片写入访问器集合（可用于读/写分库）。
        /// </summary>
        /// <param name="slicer">给定的 <see cref="IAccessorSlicer"/>。</param>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
        /// <param name="customSliceFunc">给定的自定义切片方法（可选；支持参数分库）。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public static IAccessor? SliceWriteAccessors(this IAccessorSlicer slicer,
            IReadOnlyList<AccessorDescriptor> descriptors, Func<IAccessor, bool>? customSliceFunc = null)
            => slicer.SliceAccessors(descriptors, AccessorInteraction.Write | AccessorInteraction.ReadWrite, customSliceFunc);

    }
}

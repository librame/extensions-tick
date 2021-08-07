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
using System.Linq;

namespace Librame.Extensions.Data.Access
{
    /// <summary>
    /// 抽象实现 <see cref="IAccessorAggregator"/>。
    /// </summary>
    /// <typeparam name="TAccessor">指定实现 <see cref="IAccessor"/> 的访问器类型。</typeparam>
    public abstract class AbstractAccessorSlicer<TAccessor> : IAccessorSlicer
        where TAccessor : class, IAccessor
    {
        /// <summary>
        /// 访问器类型。
        /// </summary>
        public virtual Type AccessorType
            => typeof(TAccessor);


        /// <summary>
        /// 创建访问器集合分片。
        /// </summary>
        /// <param name="accessors">给定的 <see cref="IEnumerable{TAccessor}"/>。</param>
        /// <returns>返回 <typeparamref name="TAccessor"/>。</returns>
        protected abstract TAccessor CreateSharding(IEnumerable<TAccessor> accessors);


        /// <summary>
        /// 切片访问器集合。
        /// </summary>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
        /// <param name="interaction">给定的 <see cref="AccessorInteraction"/>（支持读/写分库）。</param>
        /// <param name="customSliceFunc">给定的自定义切片方法（可选；支持参数分库）。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        public virtual IAccessor? SliceAccessors(IReadOnlyList<AccessorDescriptor> descriptors,
            AccessorInteraction interaction, Func<IAccessor, bool>? customSliceFunc = null)
        {
            if (descriptors.Count == 1)
            {
                // 只有单个访问器时不进行切片
                return descriptors[0].Accessor;
            }
            else if (descriptors.Count > 1)
            {
                var accessors = descriptors.SelectAccessors(interaction);

                // 优先进行自定义参数切片
                if (customSliceFunc != null)
                    return accessors.First(customSliceFunc);

                return CreateSharding(accessors.OfType<TAccessor>());
            }
            else
            {
                return null;
            }
        }

    }
}

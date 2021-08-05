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
using System.Linq;

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// <see cref="AccessorDescriptor"/> 静态扩展。
    /// </summary>
    public static class AccessorDescriptorExtensions
    {

        /// <summary>
        /// 选择访问器集合。
        /// </summary>
        /// <param name="descriptors">给定的 <see cref="IEnumerable{AccessorDescriptor}"/>。</param>
        /// <param name="interaction">给定的 <see cref="AccessorInteraction"/>。</param>
        /// <returns>返回 <see cref="IEnumerable{IAccessor}"/>。</returns>
        public static IEnumerable<IAccessor> SelectAccessors(this IEnumerable<AccessorDescriptor> descriptors,
            AccessorInteraction interaction)
        {
            // 支持交互形式的位与运算
            return descriptors.Where(p => (interaction & p.Interaction) == p.Interaction)
                .Select(s => s.Accessor);
        }

    }
}

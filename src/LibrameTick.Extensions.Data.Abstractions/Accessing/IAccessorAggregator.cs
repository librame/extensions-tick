#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing
{
    /// <summary>
    /// <see cref="IAccessor"/> 聚合器接口。
    /// </summary>
    public interface IAccessorAggregator
    {
        /// <summary>
        /// 访问器类型。
        /// </summary>
        Type AccessorType { get; }


        /// <summary>
        /// 聚合访问器集合。
        /// </summary>
        /// <param name="descriptors">给定的 <see cref="IReadOnlyList{IAccessor}"/>。</param>
        /// <param name="interaction">给定的 <see cref="AccessorInteraction"/>。</param>
        /// <returns>返回 <see cref="IAccessor"/>。</returns>
        IAccessor? AggregateAccessors(IReadOnlyList<AccessorDescriptor> descriptors,
            AccessorInteraction interaction);
    }
}

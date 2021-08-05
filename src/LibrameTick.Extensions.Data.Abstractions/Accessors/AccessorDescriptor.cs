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

namespace Librame.Extensions.Data.Accessors
{
    /// <summary>
    /// 定义访问器描述符。
    /// </summary>
    public class AccessorDescriptor : IEquatable<AccessorDescriptor>
    {
        /// <summary>
        /// 构造一个 <see cref="AccessorDescriptor"/>。
        /// </summary>
        /// <param name="serviceType">给定的访问器服务类型。</param>
        /// <param name="interaction">给定的访问器交互方式。</param>
        /// <param name="isPooled">访问器是否已池化。</param>
        /// <param name="priority">给定的访问器优先级。</param>
        /// <param name="accessor">给定的访问器。</param>
        public AccessorDescriptor(Type serviceType, AccessorInteraction interaction,
            bool isPooled, float priority, IAccessor accessor)
        {
            Accessor = accessor;
            ServiceType = serviceType;
            Interaction = interaction;
            IsPooled = isPooled;
            Priority = priority;
        }


        /// <summary>
        /// 访问器服务描述符。
        /// </summary>
        public Type ServiceType { get; init; }

        /// <summary>
        /// 访问器交互方式。
        /// </summary>
        public AccessorInteraction Interaction { get; init; }

        /// <summary>
        /// 访问器是否已池化。
        /// </summary>
        public bool IsPooled { get; init; }

        /// <summary>
        /// 访问器优先级。
        /// </summary>
        public float Priority {  get; init; }

        /// <summary>
        /// 访问器。
        /// </summary>
        public IAccessor Accessor { get; init; }

        ///// <summary>
        ///// <see cref="Guid"/> 标识生成器。
        ///// </summary>
        //public IIdentificationGenerator<Guid>? GuidGenerator { get; init; }


        /// <summary>
        /// 比较访问器描述符的服务类型相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="AccessorDescriptor"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(AccessorDescriptor? other)
            => ServiceType.Equals(other?.ServiceType);

        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回服务类型哈希码。</returns>
        public override int GetHashCode()
            => ServiceType.GetHashCode();

    }
}

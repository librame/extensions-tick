#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core.Cryptography;
using System.Text;

namespace Librame.Extensions.Data.Access
{
    /// <summary>
    /// 定义访问器描述符。
    /// </summary>
    public class AccessorDescriptor : IEquatable<AccessorDescriptor>
    {
        /// <summary>
        /// 构造一个 <see cref="AccessorDescriptor"/>。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <param name="accessor">给定的访问器实例。</param>
        /// <param name="group">给定的所属群组。</param>
        /// <param name="interaction">给定的交互方式。</param>
        /// <param name="pooling">是否池化。</param>
        /// <param name="priority">给定的优先级。</param>
        /// <param name="algorithms">给定的算法选项。</param>
        /// <param name="encoding">给定的字符编码。</param>
        public AccessorDescriptor(Type serviceType,
            IAccessor accessor,
            int group,
            AccessorInteraction interaction,
            bool pooling,
            float priority,
            AlgorithmOptions algorithms,
            Encoding encoding)
        {
            ServiceType = serviceType;
            Accessor = accessor;
            Group = group;
            Interaction = interaction;
            Pooling = pooling;
            Priority = priority;
            Algorithms = algorithms;
            Encoding = encoding;
        }


        /// <summary>
        /// 服务类型。
        /// </summary>
        public Type ServiceType { get; init; }

        /// <summary>
        /// 访问器实例。
        /// </summary>
        public IAccessor Accessor { get; init; }

        /// <summary>
        /// 所属群组。
        /// </summary>
        public int Group { get; init; }

        /// <summary>
        /// 交互方式。
        /// </summary>
        public AccessorInteraction Interaction { get; init; }

        /// <summary>
        /// 是否池化。
        /// </summary>
        public bool Pooling { get; init; }

        /// <summary>
        /// 优先级。
        /// </summary>
        public float Priority {  get; init; }

        /// <summary>
        /// 算法选项。
        /// </summary>
        public AlgorithmOptions Algorithms { get; init; }

        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding { get; init; }


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

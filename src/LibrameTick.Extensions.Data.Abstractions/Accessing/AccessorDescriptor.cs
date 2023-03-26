#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Cryptography;
using Librame.Extensions.Data.Sharding;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义存取器描述符。
/// </summary>
public class AccessorDescriptor : IEquatable<AccessorDescriptor>
{
    /// <summary>
    /// 构造一个 <see cref="AccessorDescriptor"/>。
    /// </summary>
    /// <param name="accessor">给定的存取器实例。</param>
    /// <param name="serviceType">给定的服务类型。</param>
    /// <param name="group">给定的所属群组。</param>
    /// <param name="access">给定的访问模式。</param>
    /// <param name="redundancy">给定的冗余模式。</param>
    /// <param name="priority">给定的优先级。</param>
    /// <param name="algorithm">给定的算法选项。</param>
    /// <param name="sharded">给定的分库特性。</param>
    public AccessorDescriptor(IAccessor accessor,
        Type serviceType,
        int group,
        AccessMode access,
        RedundancyMode redundancy,
        //bool pooling,
        float priority,
        AlgorithmOptions algorithm,
        ShardedAttribute? sharded)
    {
        Accessor = accessor;
        ServiceType = serviceType;
        Group = group;
        Access = access;
        Redundancy = redundancy;
        //Pooling = pooling;
        Priority = priority;
        Algorithm = algorithm;
        Sharded = sharded;
    }


    /// <summary>
    /// 服务类型。
    /// </summary>
    public Type ServiceType { get; init; }

    /// <summary>
    /// 存取器实例。
    /// </summary>
    public IAccessor Accessor { get; init; }

    /// <summary>
    /// 所属群组。
    /// </summary>
    public int Group { get; init; }

    /// <summary>
    /// 访问模式。
    /// </summary>
    public AccessMode Access { get; init; }

    /// <summary>
    /// 冗余模式。
    /// </summary>
    public RedundancyMode Redundancy { get; init; }

    ///// <summary>
    ///// 是否池化。
    ///// </summary>
    //public bool Pooling { get; init; }

    /// <summary>
    /// 优先级。
    /// </summary>
    public float Priority {  get; init; }

    /// <summary>
    /// 算法选项。
    /// </summary>
    public AlgorithmOptions Algorithm { get; init; }

    /// <summary>
    /// 分库特性。
    /// </summary>
    public ShardedAttribute? Sharded { get; init; }


    /// <summary>
    /// 比较存取器描述符的服务类型相等。
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

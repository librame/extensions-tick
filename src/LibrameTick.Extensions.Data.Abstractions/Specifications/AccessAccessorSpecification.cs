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

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义一个实现 <see cref="AbstractSpecification{IAccessor}"/> 的数据访问存取器规约。
/// </summary>
public class AccessAccessorSpecification : AbstractSpecification<IAccessor>
{
    /// <summary>
    /// 构造一个 <see cref="AccessAccessorSpecification"/>。
    /// </summary>
    /// <param name="group">给定的分组。</param>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    public AccessAccessorSpecification(int group, AccessMode access)
    {
        Access = access;
        Group = group;
    }


    /// <summary>
    /// 分组。
    /// </summary>
    public int Group { get; init; }

    /// <summary>
    /// 访问模式。
    /// </summary>
    public AccessMode Access { get; init; }


    /// <summary>
    /// 是否满足规约。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public override bool IsSatisfiedBy(IAccessor instance)
        => Group == instance.AccessorDescriptor?.Group
        && IsAccessMode(instance.AccessorDescriptor.Access);

    /// <summary>
    /// 是指定的访问模式。
    /// </summary>
    /// <param name="instanceAccess">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回布尔值。</returns>
    protected virtual bool IsAccessMode(AccessMode instanceAccess)
        => (Access & instanceAccess) == instanceAccess; // 使用位移计算


    /// <summary>
    /// 是镜像冗余模式。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool IsMirroringRedundancyMode(IAccessor instance)
        => instance.AccessorDescriptor?.Redundancy == RedundancyMode.Mirroring;

    /// <summary>
    /// 是分割冗余模式。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public virtual bool IsStripingRedundancyMode(IAccessor instance)
        => instance.AccessorDescriptor?.Redundancy == RedundancyMode.Striping;

}

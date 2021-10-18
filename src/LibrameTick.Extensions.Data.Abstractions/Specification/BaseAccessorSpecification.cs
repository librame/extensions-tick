#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Data.Specification;

/// <summary>
/// 定义实现 <see cref="IAccessorSpecification"/> 的基础访问器规约（默认按优先级进行升序排列）。
/// </summary>
public class BaseAccessorSpecification : Specification<IAccessor>, IAccessorSpecification
{
    /// <summary>
    /// 构造一个默认 <see cref="BaseAccessorSpecification"/>。
    /// </summary>
    public BaseAccessorSpecification()
        : base()
    {
        // 默认按优先级升序排列
        SetOrderBy(a => a.Priority);
    }

    /// <summary>
    /// 使用指定的判断依据构造一个 <see cref="BaseAccessorSpecification"/>。
    /// </summary>
    /// <param name="criterion">给定的判断依据。</param>
    public BaseAccessorSpecification(Func<IAccessor, bool> criterion)
        : base(criterion)
    {
        // 默认按优先级升序排列
        SetOrderBy(a => a.Priority);
    }


    /// <summary>
    /// 访问模式。
    /// </summary>
    public AccessMode? Access { get; private set; }

    /// <summary>
    /// 分组。
    /// </summary>
    public int? Group { get; private set; }

    /// <summary>
    /// 冗余模式（默认为聚合模式）。
    /// </summary>
    public RedundancyMode? Redundancy { get; private set; }
        = RedundancyMode.Aggregation;


    /// <summary>
    /// 评估可枚举访问器。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IEnumerable{IAccessor}"/>。</returns>
    public override IEnumerable<IAccessor> Evaluate(IEnumerable<IAccessor> enumerable)
    {
        // 优先分组
        if (Group is not null)
            enumerable = enumerable.Where(p => p.AccessorDescriptor?.Group == Group);

        // 计算访问模式
        if (Access is not null)
        {
            // 使用位移运算
            enumerable = enumerable.Where(p =>
                (Access & p.AccessorDescriptor?.Access) == p.AccessorDescriptor?.Access);
        }

        return base.Evaluate(enumerable);
    }

    /// <summary>
    /// 出具可枚举访问器。
    /// </summary>
    /// <param name="enumerable">给定的 <see cref="IEnumerable{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    public override IAccessor Issue(IEnumerable<IAccessor> enumerable)
    {
        // 优先出具自定义提供程序
        if (Provider is not null)
            return Provider(enumerable);

        if (Redundancy is not null && enumerable.Count() > 1)
        {
            if (Redundancy == RedundancyMode.Aggregation)
                return new InternalCompositeAccessor(enumerable);
            else
                return enumerable.First(); // Default Slicing
        }

        return enumerable.First();
    }


    /// <summary>
    /// 设置访问模式。
    /// </summary>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    public IAccessorSpecification SetAccess(AccessMode access)
    {
        Access = access;
        return this;
    }

    /// <summary>
    /// 设置分组。
    /// </summary>
    /// <param name="group">给定的分组。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    public IAccessorSpecification SetGroup(int group)
    {
        Group = group;
        return this;
    }

    /// <summary>
    /// 设置冗余模式。
    /// </summary>
    /// <param name="redundancy">给定的冗余模式。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    public IAccessorSpecification SetRedundancy(RedundancyMode redundancy)
    {
        Redundancy = redundancy;
        return this;
    }

}

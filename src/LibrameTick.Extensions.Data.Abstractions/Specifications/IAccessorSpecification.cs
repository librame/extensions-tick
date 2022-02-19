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
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Specifications;

/// <summary>
/// 定义一个访问器规约接口。
/// </summary>
public interface IAccessorSpecification : ISpecification<IAccessor>
{
    /// <summary>
    /// 访问模式。
    /// </summary>
    AccessMode? Access { get; }

    /// <summary>
    /// 分组。
    /// </summary>
    int? Group { get; }

    /// <summary>
    /// 冗余模式。
    /// </summary>
    RedundancyMode? Redundancy { get; }


    /// <summary>
    /// 设置访问模式。
    /// </summary>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetAccess(AccessMode access);

    /// <summary>
    /// 设置分组。
    /// </summary>
    /// <param name="group">给定的分组。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetGroup(int group);

    /// <summary>
    /// 设置冗余模式。
    /// </summary>
    /// <param name="redundancy">给定的冗余模式。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetRedundancy(RedundancyMode redundancy);
}

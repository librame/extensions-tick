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
using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义一个存取器规约接口。
/// </summary>
public interface IAccessorSpecification : ISpecification<IAccessor>
{
    /// <summary>
    /// 规约访问模式。
    /// </summary>
    AccessMode? Access { get; }

    /// <summary>
    /// 规约分组。
    /// </summary>
    int? Group { get; }

    /// <summary>
    /// 调度器选项。
    /// </summary>
    DispatcherOptions? DispatcherOptions { get; }

    /// <summary>
    /// 规约冗余模式。
    /// </summary>
    RedundancyMode Redundancy { get; }

    /// <summary>
    /// 规约冗余存取器方法。
    /// </summary>
    Func<IEnumerable<IAccessor>, RedundancyMode, DispatcherOptions, IAccessor> RedundancyAccessorFunc { get; }


    /// <summary>
    /// 设置规约访问模式。
    /// </summary>
    /// <param name="access">给定的 <see cref="AccessMode"/>。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetAccess(AccessMode access);

    /// <summary>
    /// 设置规约分组。
    /// </summary>
    /// <param name="group">给定的分组。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetGroup(int group);

    /// <summary>
    /// 设置调度器选项。
    /// </summary>
    /// <param name="options">给定的 <see cref="DispatcherOptions"/>。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetDispatcherOptions(DispatcherOptions options);

    /// <summary>
    /// 如果调度器选项为空则设置。
    /// </summary>
    /// <param name="options">给定的 <see cref="DispatcherOptions"/>。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetDispatcherOptionsIfNull(DispatcherOptions options);

    /// <summary>
    /// 设置规约冗余模式。
    /// </summary>
    /// <param name="redundancy">给定的冗余模式。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetRedundancy(RedundancyMode redundancy);

    /// <summary>
    /// 设置规约冗余存取器方法。
    /// </summary>
    /// <param name="func">给定的冗余存取器方法。</param>
    /// <returns>返回 <see cref="IAccessorSpecification"/>。</returns>
    IAccessorSpecification SetRedundancyAccessorFunc(
        Func<IEnumerable<IAccessor>, RedundancyMode, DispatcherOptions, IAccessor> func);
}

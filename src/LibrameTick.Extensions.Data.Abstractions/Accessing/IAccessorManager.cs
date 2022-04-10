#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="IAccessor"/> 管理器。
/// </summary>
public interface IAccessorManager
{
    /// <summary>
    /// 已注册的存取器列表。
    /// </summary>
    IReadOnlyList<IAccessor> Accessors { get; }


    /// <summary>
    /// 获取指定规约的存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetAccessor(IAccessorSpecification specification);

    /// <summary>
    /// 获取读取存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="ReadAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetReadAccessor(IAccessorSpecification? specification = null);

    /// <summary>
    /// 获取写入存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="IAccessorSpecification"/>（可选；默认使用 <see cref="WriteAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetWriteAccessor(IAccessorSpecification? specification = null);
}

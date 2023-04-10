#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Sharding;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="IAccessor"/> 管理器。
/// </summary>
public interface IAccessorManager
{
    /// <summary>
    /// 存取器移植器。
    /// </summary>
    IAccessorMigrator Migrator { get; }

    /// <summary>
    /// 分片管理器。
    /// </summary>
    IShardingManager ShardingManager { get; }

    /// <summary>
    /// 已注册的存取器集合。
    /// </summary>
    IReadOnlyList<IAccessor> ResolvedAccessors { get; }

    /// <summary>
    /// 当前获取的规约存取器集合。
    /// </summary>
    IReadOnlyDictionary<IAccessor, ShardedDescriptor?>? CurrentAccessors { get; }


    /// <summary>
    /// 获取指定规约的可冗余存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IDispatchableAccessors"/>。</returns>
    IDispatchableAccessors GetAccessor(ISpecification<IAccessor> specification);

    /// <summary>
    /// 获取指定规约的读取可冗余存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IDispatchableAccessors"/>。</returns>
    IDispatchableAccessors GetReadAccessor(ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 获取指定规约的写入可冗余存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    /// <returns>返回 <see cref="IDispatchableAccessors"/>。</returns>
    IDispatchableAccessors GetWriteAccessor(ISpecification<IAccessor>? specification = null);
}

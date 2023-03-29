﻿#region License

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
    /// 获取指定规约的存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="AccessorSpec"/>。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetAccessor(AccessorSpec specification);

    /// <summary>
    /// 获取读取存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="AccessorSpec"/>（可选；默认使用 <see cref="ReadAccessorSpec"/> 规约）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetReadAccessor(AccessorSpec? specification = null);

    /// <summary>
    /// 获取写入存取器。
    /// </summary>
    /// <param name="specification">给定的 <see cref="AccessorSpec"/>（可选；默认使用 <see cref="WriteAccessorSpec"/> 规约）。</param>
    /// <returns>返回 <see cref="IAccessor"/>。</returns>
    IAccessor GetWriteAccessor(AccessorSpec? specification = null);
}

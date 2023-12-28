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
using Librame.Extensions.Dispatchers;
using Librame.Extensions.Specifications;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// <see cref="IAccessor"/> 上下文。
/// </summary>
public interface IAccessorContext
{
    /// <summary>
    /// 存取器移植器。
    /// </summary>
    IAccessorMigrator Migrator { get; }

    /// <summary>
    /// 分片上下文。
    /// </summary>
    IShardingContext ShardingContext { get; }

    /// <summary>
    /// 已注册的存取器集合。
    /// </summary>
    IReadOnlyList<IAccessor> ResolvedAccessors { get; }

    /// <summary>
    /// 当前获取或写入规约的存取器集合（值为是否分片的描述符）。
    /// </summary>
    IReadOnlyDictionary<IAccessor, ShardingDescriptor?>? CurrentAccessors { get; }


    ///// <summary>
    ///// 获取指定规约的读取调度器存取器集合。
    ///// </summary>
    ///// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="ReadAccessAccessorSpecification"/> 规约）。</param>
    ///// <returns>返回 <see cref="IDispatcherAccessors"/>。</returns>
    //IDispatcherAccessors GetReadAccessors(ISpecification<IAccessor>? specification = null);

    ///// <summary>
    ///// 获取指定规约的写入调度器存取器集合。
    ///// </summary>
    ///// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>（可选；默认使用 <see cref="WriteAccessAccessorSpecification"/> 规约）。</param>
    ///// <returns>返回 <see cref="IDispatcherAccessors"/>。</returns>
    //IDispatcherAccessors GetWriteAccessors(ISpecification<IAccessor>? specification = null);

    /// <summary>
    /// 获取指定规约的调度器存取器集合。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpecification{IAccessor}"/>。</param>
    /// <returns>返回 <see cref="IDispatcherAccessors"/>。</returns>
    IDispatcherAccessors GetAccessors(ISpecification<IAccessor> specification);
}

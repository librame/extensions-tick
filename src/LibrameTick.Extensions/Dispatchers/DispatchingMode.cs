#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义调度模式。
/// </summary>
public enum DispatchingMode
{
    /// <summary>
    /// 默认模式（单调度器场景）。此模式下，默认单个执行元素。
    /// </summary>
    Default = 0,

    /// <summary>
    /// 镜像模式（单调度器场景）。此模式下，集合内为冗余元素，仅需单个执行元素，异常时会自行切换下个元素执行，可用于高安全性低并发场景（类似于 RAID1）。
    /// </summary>
    Mirroring = 1,

    /// <summary>
    /// 分割模式（单调度器场景）。此模式下，集合内为独立元素，需遍历执行聚合为完整一组元素（通常配合分布式事务），可用于低安全性高并发场景（类似于 RAID0）。
    /// </summary>
    Striping = 2,

    /// <summary>
    /// 复合模式（多调度器场景）。此模式下，集合内为独立调度器，需遍历执行聚合为完整一组调度器，不同调度器内的所有元素根据各自的不同调度模式进行遍历或单个执行。
    /// </summary>
    Compositing = 4
}

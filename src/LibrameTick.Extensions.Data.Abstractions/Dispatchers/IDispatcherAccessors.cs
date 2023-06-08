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

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义一个实现 <see cref="IAccessor"/> 的调度器存取器集合接口。
/// </summary>
public interface IDispatcherAccessors : IAccessor, IEnumerable<IDispatcher<IAccessor>>, IEquatable<IDispatcherAccessors>
{
    /// <summary>
    /// 读存取器调度器。
    /// </summary>
    IDispatcher<IAccessor> ReadingDispatcher { get; }

    /// <summary>
    /// 写存取器调度器。
    /// </summary>
    IDispatcher<IAccessor> WritingDispatcher { get; }

    /// <summary>
    /// 调度模式。
    /// </summary>
    DispatchingMode Mode { get; }

    /// <summary>
    /// 是否读写分离。
    /// </summary>
    bool IsWritingSeparation { get; }
}
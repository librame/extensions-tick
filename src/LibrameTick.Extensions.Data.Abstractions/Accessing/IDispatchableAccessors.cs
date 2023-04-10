#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dispatchers;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义一个实现 <see cref="IAccessor"/> 的可调度存取器集合接口。
/// </summary>
public interface IDispatchableAccessors : IAccessor
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
    /// 是否读写分离。
    /// </summary>
    bool IsWritingSeparation { get; }
}
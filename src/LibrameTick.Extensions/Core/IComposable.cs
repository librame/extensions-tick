#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个可复合接口。
/// </summary>
public interface IComposable<T> : IEnumerable<T>
{
    /// <summary>
    /// 复合数量。
    /// </summary>
    int Count { get; }
}

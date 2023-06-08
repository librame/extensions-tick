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
/// 定义一个实现 <see cref="IEnumerable{T}"/> 的可复合接口。
/// </summary>
/// <typeparam name="T">指定的复合类型。</typeparam>
public interface IComposable<T> : IEnumerable<T>
{
    /// <summary>
    /// 复合数量。
    /// </summary>
    int Count { get; }
}

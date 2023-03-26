﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Specifications;

/// <summary>
/// 定义抽象实现 <see cref="ISpec{T}"/> 规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public abstract class AbstractSpec<T> : ISpec<T>
{
    /// <summary>
    /// 是否满足规约。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public abstract bool IsSatisfiedBy(T instance);

}

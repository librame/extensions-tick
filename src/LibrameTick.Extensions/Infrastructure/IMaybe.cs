#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义模仿函数式编程的公共接口。
/// </summary>
/// <remarks>
/// 参考：<see href="https://github.com/vkhorikov/CSharpFunctionalExtensions"/>
/// </remarks>
/// <typeparam name="T">指定的类型。</typeparam>
public interface IMaybe<T>
{
    /// <summary>
    /// 值。
    /// </summary>
    T Value { get; }

    /// <summary>
    /// 包含值。
    /// </summary>
    bool HasValue { get; }

    /// <summary>
    /// 不包含值。
    /// </summary>
    bool HasNoValue { get; }
}

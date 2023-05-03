#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义一个强类型标识符。
/// </summary>
/// <typeparam name="TValue">指定的值类型。</typeparam>
public readonly struct StronglyTypedIdentifier<TValue>
{
    /// <summary>
    /// 构造一个 <see cref="StronglyTypedIdentifier{TValue}"/>。
    /// </summary>
    /// <param name="value">给定的 <typeparamref name="TValue"/>。</param>
    public StronglyTypedIdentifier(TValue value)
    {
        Value = value;
    }


    /// <summary>
    /// 获取 <typeparamref name="TValue"/> 标识。
    /// </summary>
    public TValue Value { get; }
}

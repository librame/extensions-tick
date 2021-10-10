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
/// 定义一个实现 <see cref="IDecoratable{TSource, TImplementation}"/> 可装饰接口。
/// </summary>
/// <typeparam name="TSource">指定的源类型。</typeparam>
/// <typeparam name="TImplementation">指定的实现类型。</typeparam>
public interface IDecoratable<out TSource, out TImplementation> : IDecoratable<TSource>
    where TImplementation : TSource
{
    /// <summary>
    /// 实现实例。
    /// </summary>
    /// <value>返回 <typeparamref name="TImplementation"/>。</value>
    new TImplementation Source { get; }
}


/// <summary>
/// 定义一个可装饰接口。
/// </summary>
/// <typeparam name="TSource">指定的源类型。</typeparam>
public interface IDecoratable<out TSource>
{
    /// <summary>
    /// 源实例。
    /// </summary>
    /// <value>返回 <typeparamref name="TSource"/>。</value>
    TSource Source { get; }
}

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
/// 定义一个可装饰接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public interface IDecoratable<out TSource>
{
    /// <summary>
    /// 来源实例。
    /// </summary>
    /// <value>返回 <typeparamref name="TSource"/>。</value>
    TSource Source { get; }
}


/// <summary>
/// 定义实现 <see cref="IDecoratable{TSource}"/> 的可装饰类。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class Decoratable<TSource> : IDecoratable<TSource>
{
    /// <summary>
    /// 构造一个 <see cref="Decoratable{TSource}"/>。
    /// </summary>
    /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
    public Decoratable(TSource source)
    {
        Source = source;
    }


    /// <summary>
    /// 来源实例。
    /// </summary>
    /// <value>返回 <typeparamref name="TSource"/>。</value>
    public TSource Source { get; }
}

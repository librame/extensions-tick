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
/// 定义抽象实现 <see cref="IDecoratable{TSource, TImplementation}"/> 的可装饰类。
/// </summary>
/// <typeparam name="TSource">指定的源类型。</typeparam>
/// <typeparam name="TImplementation">指定的实现类型。</typeparam>
public abstract class AbstractDecoratable<TSource, TImplementation>
    : AbstractDecoratable<TSource>, IDecoratable<TSource, TImplementation>
    where TImplementation : TSource
{
    /// <summary>
    /// 构造一个 <see cref="AbstractDecoratable{TSource, TImplementation}"/>。
    /// </summary>
    /// <param name="implementation">给定的 <typeparamref name="TImplementation"/>。</param>
    protected AbstractDecoratable(TImplementation implementation)
        : base(implementation)
    {
        Source = implementation;
    }


    /// <summary>
    /// 实现实例。
    /// </summary>
    /// <value>返回 <typeparamref name="TImplementation"/>。</value>
    public new TImplementation Source { get; }
}


/// <summary>
/// 定义抽象实现 <see cref="IDecoratable{TSource}"/> 的可装饰类。
/// </summary>
/// <typeparam name="TSource">指定的源类型。</typeparam>
public abstract class AbstractDecoratable<TSource> : IDecoratable<TSource>
{
    /// <summary>
    /// 构造一个 <see cref="AbstractDecoratable{TSource}"/>。
    /// </summary>
    /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
    protected AbstractDecoratable(TSource source)
    {
        Source = source.NotNull(nameof(source));
    }


    /// <summary>
    /// 源实例。
    /// </summary>
    /// <value>返回 <typeparamref name="TSource"/>。</value>
    public TSource Source { get; }
}

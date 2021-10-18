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
/// 定义实现 <see cref="IDecoratable{TSource}"/> 的可装饰基类。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public class BaseDecoratable<TSource> : IDecoratable<TSource>
{
    /// <summary>
    /// 构造一个 <see cref="BaseDecoratable{TSource}"/>。
    /// </summary>
    /// <param name="source">给定的 <typeparamref name="TSource"/>。</param>
    public BaseDecoratable(TSource source)
    {
        Source = source;
    }


    /// <summary>
    /// 来源实例。
    /// </summary>
    /// <value>返回 <typeparamref name="TSource"/>。</value>
    public TSource Source { get; }
}

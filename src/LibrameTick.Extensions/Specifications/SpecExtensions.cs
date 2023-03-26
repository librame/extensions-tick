#region License

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
/// <see cref="ISpec{T}"/> 静态扩展。
/// </summary>
public static class SpecExtensions
{

    /// <summary>
    /// 与复合规约。
    /// </summary>
    /// <param name="left">给定的 <see cref="ISpec{T}"/> 左实例。</param>
    /// <param name="right">给定的 <see cref="ISpec{T}"/> 右实例。</param>
    /// <returns>返回 <see cref="ISpec{T}"/>。</returns>
    public static ISpec<T> And<T>(this ISpec<T> left, ISpec<T> right)
        => new AndSpec<T>(left, right);

    /// <summary>
    /// 或复合规约。
    /// </summary>
    /// <param name="left">给定的 <see cref="ISpec{T}"/> 左实例。</param>
    /// <param name="right">给定的 <see cref="ISpec{T}"/> 右实例。</param>
    /// <returns>返回 <see cref="ISpec{T}"/>。</returns>
    public static ISpec<T> Or<T>(this ISpec<T> left, ISpec<T> right)
        => new OrSpec<T>(left, right);

}

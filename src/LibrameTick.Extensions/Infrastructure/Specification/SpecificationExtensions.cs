#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Specification;

/// <summary>
/// <see cref="ISpecification{T}"/> 静态扩展。
/// </summary>
public static class SpecificationExtensions
{

    /// <summary>
    /// 与复合规约。
    /// </summary>
    /// <param name="left">给定的 <see cref="ISpecification{T}"/> 左实例。</param>
    /// <param name="right">给定的 <see cref="ISpecification{T}"/> 右实例。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    public static ISpecification<T> And<T>(this ISpecification<T> left, ISpecification<T> right)
        => new AndSpecification<T>(left, right);

    /// <summary>
    /// 或复合规约。
    /// </summary>
    /// <param name="left">给定的 <see cref="ISpecification{T}"/> 左实例。</param>
    /// <param name="right">给定的 <see cref="ISpecification{T}"/> 右实例。</param>
    /// <returns>返回 <see cref="ISpecification{T}"/>。</returns>
    public static ISpecification<T> Or<T>(this ISpecification<T> left, ISpecification<T> right)
        => new OrSpecification<T>(left, right);

}

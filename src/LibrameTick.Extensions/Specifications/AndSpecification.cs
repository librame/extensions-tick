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
/// 定义实现 <see cref="AbstractSpecification{T}"/> 的与复合规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class AndSpecification<T> : AbstractSpecification<T>
{
    private ISpecification<T> _left;
    private ISpecification<T> _right;


    /// <summary>
    /// 构造一个 <see cref="AndSpecification{T}"/> 实例。
    /// </summary>
    /// <param name="left">给定的 <see cref="ISpecification{T}"/> 左实例。</param>
    /// <param name="right">给定的 <see cref="ISpecification{T}"/> 右实例。</param>
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
    {
        _left = left;
        _right = right;
    }


    /// <summary>
    /// 是否满足与复合规约。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public override bool IsSatisfiedBy(T instance)
        => _left.IsSatisfiedBy(instance) && _right.IsSatisfiedBy(instance);

}

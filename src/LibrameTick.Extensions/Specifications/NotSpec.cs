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
/// 定义实现 <see cref="AbstractSpec{T}"/> 的非规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class NotSpec<T> : AbstractSpec<T>
{
    private ISpec<T> _specification;


    /// <summary>
    /// 构造一个 <see cref="NotSpec{T}"/> 实例。
    /// </summary>
    /// <param name="specification">给定的 <see cref="ISpec{T}"/> 实例。</param>
    public NotSpec(ISpec<T> specification)
    {
        _specification = specification;
    }


    /// <summary>
    /// 是否满足非规约。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public override bool IsSatisfiedBy(T instance)
        => !_specification.IsSatisfiedBy(instance);

}

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
/// 定义实现 <see cref="AbstractSpecification{T}"/> 的表达式规约。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public class ExpressionSpecification<T> : AbstractSpecification<T>
{
    private Func<T, bool> _expression;


    /// <summary>
    /// 构造一个 <see cref="NotSpecification{T}"/> 实例。
    /// </summary>
    /// <param name="expression">给定的 <see cref="Func{T, Boolean}"/> 实例。</param>
    public ExpressionSpecification(Func<T, bool> expression)
    {
        _expression = expression;
    }


    /// <summary>
    /// 是否满足规约。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public override bool IsSatisfiedBy(T instance)
        => _expression(instance);

}

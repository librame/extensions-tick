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
/// 定义一个规约接口。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// 是否满足规约。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    bool IsSatisfiedBy(T instance);
}

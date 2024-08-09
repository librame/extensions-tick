#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Data.Accessing;

namespace Librame.Extensions.Specification;

/// <summary>
/// 定义一个实现 <see cref="AbstractSpecification{IAccessor}"/> 的命名数据访问存取器规约。
/// </summary>
public class NamedAccessorSpecification : AbstractSpecification<IAccessor>
{
    /// <summary>
    /// 构造一个 <see cref="NamedAccessorSpecification"/>。
    /// </summary>
    /// <param name="name">给定的名称。</param>
    public NamedAccessorSpecification(string name)
    {
        Name = name;
    }


    /// <summary>
    /// 名称。
    /// </summary>
    public string Name { get; init; }


    /// <summary>
    /// 是否满足规约。
    /// </summary>
    /// <param name="instance">给定的实例。</param>
    /// <returns>返回布尔值。</returns>
    public override bool IsSatisfiedBy(IAccessor instance)
        => Name == instance.AccessorDescriptor?.Name;

}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependency;

/// <summary>
/// 定义继承 <see cref="IDependencyInitializer"/> 的依赖初始化器泛型接口。
/// </summary>
/// <typeparam name="TDependency">指定的依赖类型。</typeparam>
public interface IDependencyInitializer<TDependency> : IDependencyInitializer
    where TDependency : IDependency
{
    /// <summary>
    /// 初始化依赖。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDependencyContext"/>。</param>
    /// <param name="characteristic">给定的 <see cref="DependencyCharacteristic"/>。</param>
    /// <returns>返回包含 <typeparamref name="TDependency"/> 的延迟方法。</returns>
    TDependency Initialize(IDependencyContext context, DependencyCharacteristic characteristic);
}


/// <summary>
/// 定义依赖的初始化器标记接口。
/// </summary>
public interface IDependencyInitializer
{
}

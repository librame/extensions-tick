#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependencies;

/// <summary>
/// 定义静态扩展依赖的初始化器接口。
/// </summary>
/// <typeparam name="TDependency">指定的依赖类型。</typeparam>
public interface IExtensionsDependencyInitializer<TDependency>
{
    /// <summary>
    /// 初始化静态扩展依赖。
    /// </summary>
    /// <param name="dependency">给定的 <see cref="IExtensionsDependency"/>。</param>
    /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
    TDependency Initialize(IExtensionsDependency dependency);
}

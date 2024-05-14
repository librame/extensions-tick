#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Dependency;

/// <summary>
/// 定义实现 <see cref="IDependencyInitializer{TDependency}"/> 的默认依赖初始化器。
/// </summary>
/// <typeparam name="TDependency">指定的依赖类型。</typeparam>
/// <param name="initialFunc">给定依赖的初始方法。</param>
public class DefaultDependencyInitializer<TDependency>(Func<IDependencyContext, DependencyCharacteristic, TDependency> initialFunc)
    : IDependencyInitializer<TDependency>
    where TDependency : IDependency
{

    /// <summary>
    /// 初始化依赖。
    /// </summary>
    /// <param name="context">给定的 <see cref="IDependencyContext"/>。</param>
    /// <param name="characteristic">给定的 <see cref="DependencyCharacteristic"/>。</param>
    /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
    public TDependency Initialize(IDependencyContext context, DependencyCharacteristic characteristic)
    {
        var dependency = initialFunc(context, characteristic);
        return dependency;
    }

}

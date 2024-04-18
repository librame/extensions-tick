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
/// 定义静态扩展依赖初始化器。
/// </summary>
/// <typeparam name="TInitializer">指定实现 <see cref="IExtensionsDependencyInitializer{TDependency}"/> 的静态扩展依赖初始化器类型。</typeparam>
/// <typeparam name="TDependency">指定的静态扩展依赖类型。</typeparam>
public static class ExtensionsDependencyInitializer<TInitializer, TDependency>
    where TInitializer : IExtensionsDependencyInitializer<TDependency>, new()
{

    /// <summary>
    /// 初始化静态扩展依赖。
    /// </summary>
    /// <remarks>
    /// 默认启用静态扩展依赖。如果需要默认禁用，可在调用 <see cref="ExtensionsDependencyInstantiator.Instance"/> 前，
    /// 使用 <see cref="ExtensionsDependencyRegistration.DisableCharacteristic(Type)"/> 提前配置禁用此静态扩展依赖特征。
    /// </remarks>
    /// <param name="dependency">给定的 <see cref="IExtensionsDependency"/>。</param>
    /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
    /// <exception cref="InvalidOperationException">
    /// The extensions dependency characteristic is configured to be isEnabled = false,
    /// and can be used only after it is configured to be isEnabled = true.
    /// </exception>
    public static TDependency Initialize(IExtensionsDependency dependency)
    {
        var characteristic = ExtensionsDependencyRegistration.GetOrAddCharacteristic(new(typeof(TDependency), isEnabled: true));
        if (!characteristic.IsEnabled)
        {
            throw new InvalidOperationException($"The extensions dependency '{characteristic.DependencyType}' characteristic is configured to be isEnabled = false, and can be used only after it is configured to be isEnabled = true.");
        }

        var initializer = new TInitializer();

        return initializer.Initialize(dependency);
    }

}

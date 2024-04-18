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
/// 定义静态扩展依赖注册。
/// </summary>
public static class ExtensionsDependencyRegistration
{
    private static readonly ConcurrentDictionary<Type, ExtensionsDependencyCharacteristic> _characteristics = new();


    /// <summary>
    /// 获取当前依赖特征集合。
    /// </summary>
    public static ICollection<ExtensionsDependencyCharacteristic> Characteristics => _characteristics.Values;


    /// <summary>
    /// 快速禁用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <returns>返回 <see cref="ExtensionsDependencyCharacteristic"/>。</returns>
    public static ExtensionsDependencyCharacteristic DisableCharacteristic<TDependency>()
        => DisableCharacteristic(typeof(TDependency));

    /// <summary>
    /// 快速禁用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <param name="dependencyType">给定的依赖类型。</param>
    /// <returns>返回 <see cref="ExtensionsDependencyCharacteristic"/>。</returns>
    public static ExtensionsDependencyCharacteristic DisableCharacteristic(Type dependencyType)
    {
        if (!_characteristics.TryGetValue(dependencyType, out var result))
        {
            result = new(dependencyType, isEnabled: false);

            _characteristics[dependencyType] = result;
        }
        else
        {
            if (result.IsEnabled)
            {
                result.IsEnabled = false;
            }
        }

        return result;
    }


    /// <summary>
    /// 快速启用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
    /// <returns>返回 <see cref="ExtensionsDependencyCharacteristic"/>。</returns>
    public static ExtensionsDependencyCharacteristic EnableCharacteristic<TDependency>()
        => EnableCharacteristic(typeof(TDependency));

    /// <summary>
    /// 快速启用依赖特征。如果特征不存在，则添加，反之则更新特征。
    /// </summary>
    /// <param name="dependencyType">给定的依赖类型。</param>
    /// <returns>返回 <see cref="ExtensionsDependencyCharacteristic"/>。</returns>
    public static ExtensionsDependencyCharacteristic EnableCharacteristic(Type dependencyType)
    {
        if (!_characteristics.TryGetValue(dependencyType, out var result))
        {
            result = new(dependencyType, isEnabled: true);

            _characteristics[dependencyType] = result;
        }
        else
        {
            if (!result.IsEnabled)
            {
                result.IsEnabled = true;
            }
        }

        return result;
    }


    /// <summary>
    /// 获取或添加依赖特征。
    /// </summary>
    /// <param name="characteristic">给定的 <see cref="ExtensionsDependencyCharacteristic"/>。</param>
    /// <returns>返回 <see cref="ExtensionsDependencyCharacteristic"/>。</returns>
    public static ExtensionsDependencyCharacteristic GetOrAddCharacteristic(ExtensionsDependencyCharacteristic characteristic)
        => _characteristics.GetOrAdd(characteristic.DependencyType, key => characteristic);

    /// <summary>
    /// 添加或更新依赖特征。
    /// </summary>
    /// <param name="characteristic">给定的 <see cref="ExtensionsDependencyCharacteristic"/>。</param>
    /// <returns>返回 <see cref="ExtensionsDependencyCharacteristic"/>。</returns>
    public static ExtensionsDependencyCharacteristic AddOrUpdateCharacteristic(ExtensionsDependencyCharacteristic characteristic)
        => _characteristics.AddOrUpdate(characteristic.DependencyType, key => characteristic, (key, oldValue) => characteristic);

    /// <summary>
    /// 尝试获取依赖特征。
    /// </summary>
    /// <param name="dependencyType">给定的依赖类型。</param>
    /// <param name="characteristic">输出成功获取的 <see cref="ExtensionsDependencyCharacteristic"/>。</param>
    /// <returns>返回是否成功获取的布尔值。</returns>
    public static bool TryGetCharacteristic(Type dependencyType, [MaybeNullWhen(false)] out ExtensionsDependencyCharacteristic characteristic)
        => _characteristics.TryGetValue(dependencyType, out characteristic);

}

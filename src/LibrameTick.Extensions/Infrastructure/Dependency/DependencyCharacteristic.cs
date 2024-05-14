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
/// 定义继承 <see cref="IEquatable{DependencyCharacteristic}"/> 的依赖特征。
/// </summary>
/// <param name="dependencyType">给定的依赖类型。</param>
/// <param name="isEnabled">给定是否启用依赖。</param>
public class DependencyCharacteristic(Type dependencyType, bool isEnabled) : IEquatable<DependencyCharacteristic>
{
    /// <summary>
    /// 获取依赖类型。
    /// </summary>
    public Type DependencyType { get; init; } = dependencyType;

    /// <summary>
    /// 获取或设置是否启用依赖。
    /// </summary>
    public bool IsEnabled { get; set; } = isEnabled;


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as DependencyCharacteristic);

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="DependencyCharacteristic"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(DependencyCharacteristic? other)
        => other?.DependencyType == DependencyType;

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    public override int GetHashCode()
        => DependencyType.GetHashCode();

}

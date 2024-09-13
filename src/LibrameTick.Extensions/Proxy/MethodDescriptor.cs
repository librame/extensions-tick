#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Proxy;

/// <summary>
/// 定义一个方法描述符。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="MethodDescriptor"/>。
/// </remarks>
/// <param name="target">给定的目标 <see cref="MethodInfo"/>。</param>
/// <param name="source">给定的来源 <see cref="MethodInfo"/>。</param>
/// <param name="args">给定的实参数组。</param>
public sealed class MethodDescriptor(MethodInfo target, MethodInfo source, object?[]? args)
    : IEquatable<MethodDescriptor>
{
    /// <summary>
    /// 拦截的目标方法信息，通常是接口方法信息。
    /// </summary>
    public MethodInfo Target { get; init; } = target;

    /// <summary>
    /// 拦截的来源方法信息，通常是接口实现的方法信息。
    /// </summary>
    public MethodInfo Source { get; init; } = source;

    /// <summary>
    /// 实参数组。
    /// </summary>
    public object?[]? Args { get; init; } = args;

    /// <summary>
    /// 方法参数类型数组。
    /// </summary>
    public Type[] ParamTypes { get; init; } = target.GetParameters().Select(s => s.ParameterType).ToArray();


    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="other">给定的 <see cref="MethodDescriptor"/>。</param>
    /// <returns>返回布尔值。</returns>
    public bool Equals(MethodDescriptor? other)
        => other is not null && Target.Equals(other.Target);

    /// <summary>
    /// 比较相等。
    /// </summary>
    /// <param name="obj">给定的对象。</param>
    /// <returns>返回布尔值。</returns>
    public override bool Equals(object? obj)
        => Equals(obj as MethodDescriptor);

    /// <summary>
    /// 获取哈希码。
    /// </summary>
    /// <returns>返回整数。</returns>
    public override int GetHashCode()
        => Target.GetHashCode();

}

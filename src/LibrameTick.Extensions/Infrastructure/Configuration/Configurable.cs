#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Configuration;

/// <summary>
/// 定义实现 <see cref="IConfigurable{T}"/> 的可配置。
/// </summary>
/// <param name="initialValue">给定的初始值。</param>
public class Configurable<T>(T initialValue) : IConfigurable<T>
{
    /// <summary>
    /// 初始值。
    /// </summary>
    public T InitialValue { get; init; } = initialValue;

    /// <summary>
    /// 当前值。
    /// </summary>
    public T CurrentValue { get; private set; } = initialValue;


    /// <summary>
    /// 配置当前值。
    /// </summary>
    /// <param name="configureFunc">给定当前值的配置方法。</param>
    /// <returns>返回当前 <see cref="IConfigurable{T}"/>。</returns>
    public IConfigurable<T> ConfigureValue(Func<IConfigurable<T>, T> configureFunc)
    {
        CurrentValue = configureFunc(this);
        return this;
    }

}

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
/// 定义指定类型的可配置接口。
/// </summary>
/// <typeparam name="T">指定的类型。</typeparam>
public interface IConfigurable<T>
{
    /// <summary>
    /// 初始值。
    /// </summary>
    T InitialValue { get; }

    /// <summary>
    /// 当前值。
    /// </summary>
    T CurrentValue { get; }


    /// <summary>
    /// 配置当前值。
    /// </summary>
    /// <param name="configureFunc">给定当前值的配置方法。</param>
    /// <returns>返回当前 <see cref="IConfigurable{T}"/>。</returns>
    IConfigurable<T> ConfigureValue(Func<IConfigurable<T>, T> configureFunc);
}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// 定义一个集成各选项值的选项值集合接口。
/// </summary>
/// <typeparam name="TOptions">指定的选项类型。</typeparam>
public interface IOptionsValues<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] out TOptions>
{
    /// <summary>
    /// 获取单例值，即 <see cref="IOptions{TOptions}"/>。
    /// </summary>
    /// <returns>返回 <typeparamref name="TOptions"/>。</returns>
    TOptions GetSingletonValue();

    /// <summary>
    /// 获取域例值，即 <see cref="IOptionsSnapshot{TOptions}"/>。
    /// </summary>
    /// <returns>返回 <typeparamref name="TOptions"/>。</returns>
    TOptions GetScopeValue();

    /// <summary>
    /// 获取瞬例值，即 <see cref="IOptionsMonitor{TOptions}"/>。
    /// </summary>
    /// <returns>返回 <typeparamref name="TOptions"/>。</returns>
    TOptions GetTransientValue();
}

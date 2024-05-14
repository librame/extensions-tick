#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义表示是否通过验证的泛型接口。
/// </summary>
/// <typeparam name="T">指定的验证类型。</typeparam>
public interface IValidation<T> : IValidation
{
    /// <summary>
    /// 验证有效性。
    /// </summary>
    /// <returns>返回 <typeparamref name="T"/>。</returns>
    T Validate();
}


/// <summary>
/// 定义表示是否通过验证的接口。
/// </summary>
public interface IValidation
{
    /// <summary>
    /// 验证是否有效。
    /// </summary>
    /// <returns>返回布尔值。</returns>
    bool IsValidated();
}

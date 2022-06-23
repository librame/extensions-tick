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
/// 定义一个用于创建实例的可实例化接口。
/// </summary>
/// <typeparam name="TInstance">指定的实例类型。</typeparam>
public interface IInstantiable<TInstance>
    where TInstance : class
{
    /// <summary>
    /// 创建实例。
    /// </summary>
    /// <returns>返回 <typeparamref name="TInstance"/>。</returns>
    TInstance Create();
}

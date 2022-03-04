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
/// 定义一个可创建实例对象的实例化器接口。
/// </summary>
/// <typeparam name="TInstance">指定的实例类型。</typeparam>
public interface IInstantiator<TInstance>
    where TInstance : class
{
    /// <summary>
    /// 创建实例。
    /// </summary>
    /// <returns>返回 <typeparamref name="TInstance"/>。</returns>
    TInstance Create();
}

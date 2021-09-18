#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义 <see cref="IAccessor"/> 种子机。
/// </summary>
public interface IAccessorSeeder
{
    /// <summary>
    /// 时钟。
    /// </summary>
    IClock Clock { get; }

    /// <summary>
    /// 标识生成器工厂。
    /// </summary>
    IIdentificationGeneratorFactory IdGeneratorFactory { get; }


    /// <summary>
    /// 获取初始用户标识。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <returns>返回 <typeparamref name="TId"/>。</returns>
    TId GetInitialUserId<TId>();

    /// <summary>
    /// 获取初始用户标识。
    /// </summary>
    /// <param name="idType">给定的标识类型。</param>
    /// <returns>返回标识对象。</returns>
    object GetInitialUserId(Type idType);
}

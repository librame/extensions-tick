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

namespace Librame.Extensions.Data;

/// <summary>
/// 定义标识生成器工厂接口。
/// </summary>
public interface IIdentificationGeneratorFactory
{
    /// <summary>
    /// 获取标识生成器。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回 <see cref="IIdentificationGenerator{TId}"/>。</returns>
    IIdentificationGenerator<TId> GetIdGenerator<TId>(TypeNamedKey key)
        where TId : IEquatable<TId>;

    /// <summary>
    /// 获取对象标识生成器。
    /// </summary>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回 <see cref="IObjectIdentificationGenerator"/>。</returns>
    IObjectIdentificationGenerator GetIdGenerator(TypeNamedKey key);
}

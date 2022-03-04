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
using Librame.Extensions.IdGenerators;

namespace Librame.Extensions.Data;

/// <summary>
/// 定义标识生成器工厂接口。
/// </summary>
public interface IIdGeneratorFactory
{
    /// <summary>
    /// 获取标识生成器。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回 <see cref="IIdGenerator{TId}"/>。</returns>
    IIdGenerator<TId> GetIdGenerator<TId>(TypeNamedKey key)
        where TId : IEquatable<TId>;

    /// <summary>
    /// 获取对象标识生成器。
    /// </summary>
    /// <param name="key">给定的 <see cref="TypeNamedKey"/>。</param>
    /// <returns>返回 <see cref="IObjectIdGenerator"/>。</returns>
    IObjectIdGenerator GetIdGenerator(TypeNamedKey key);
}

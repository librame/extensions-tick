#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.IdGenerators;

/// <summary>
/// 定义对象标识生成器接口。
/// </summary>
public interface IObjectIdGenerator
{
    /// <summary>
    /// 标识类型。
    /// </summary>
    Type IdType { get; }


    /// <summary>
    /// 生成对象标识。
    /// </summary>
    /// <returns>返回标识对象。</returns>
    object GenerateObjectId();

    /// <summary>
    /// 异步生成对象标识。
    /// </summary>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个包含标识对象的异步操作。</returns>
    ValueTask<object> GenerateObjectIdAsync(CancellationToken cancellationToken = default);
}

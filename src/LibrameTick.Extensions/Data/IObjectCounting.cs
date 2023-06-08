#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data;

/// <summary>
/// 定义对象计数接口。
/// </summary>
public interface IObjectCounting
{
    /// <summary>
    /// 计数类型。
    /// </summary>
    Type CountType { get; }


    /// <summary>
    /// 递减对象计数。
    /// </summary>
    /// <param name="count">给定要递减的计数对象。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <returns>返回对象。</returns>
    object DecrementalObjectCount(object count, object? decrement = null);

    /// <summary>
    /// 异步递减对象计数。
    /// </summary>
    /// <param name="count">给定要递减的计数对象。</param>
    /// <param name="decrement">给定的减量（可选；如果为空则表示自减）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
    ValueTask<object> DecrementalObjectCountAsync(object count, object? decrement = null,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// 递加对象计数。
    /// </summary>
    /// <param name="count">给定要递加的对象计数对象。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <returns>返回对象。</returns>
    object IncrementalObjectCount(object count, object? increment = null);

    /// <summary>
    /// 异步递加对象计数。
    /// </summary>
    /// <param name="count">给定要递加的对象计数对象。</param>
    /// <param name="increment">给定的增量（可选；如果为空则表示自增）。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
    ValueTask<object> IncrementalObjectCountAsync(object count, object? increment = null,
        CancellationToken cancellationToken = default);
}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 定义泛型计数接口。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public interface ICount<TValue> : IObjectCount
        where TValue : struct
    {
        /// <summary>
        /// 转换为计数值。
        /// </summary>
        /// <param name="count">给定的计数对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ToCountValue(object count, string? paramName);

        /// <summary>
        /// 转换为减量值。
        /// </summary>
        /// <param name="decrement">给定的减量对象。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ToDecrementValue(object? decrement = null);

        /// <summary>
        /// 转换为增量值。
        /// </summary>
        /// <param name="increment">给定的增量对象。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ToIncrementValue(object? increment = null);


        /// <summary>
        /// 累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数值。</param>
        /// <param name="decrement">给定的减量（可选；默认减1）。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue DegressiveCount(TValue count, TValue decrement = default);

        /// <summary>
        /// 异步累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数值。</param>
        /// <param name="decrement">给定的减量（可选；默认减1）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> DegressiveCountAsync(TValue count, TValue decrement = default,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加计数。
        /// </summary>
        /// <param name="count">给定要累减的计数值。</param>
        /// <param name="increment">给定的增量（可选；默认加1）。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        TValue ProgressiveCount(TValue count, TValue increment = default);

        /// <summary>
        /// 异步累加计数。
        /// </summary>
        /// <param name="count">给定要累减的计数值。</param>
        /// <param name="increment">给定的增量（可选；默认加1）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        ValueTask<TValue> ProgressiveCountAsync(TValue count, TValue increment = default,
            CancellationToken cancellationToken = default);
    }
}

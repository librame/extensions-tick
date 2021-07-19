#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    /// <summary>
    /// 抽象计数。
    /// </summary>
    /// <typeparam name="TValue">指定的数值类型。</typeparam>
    public abstract class AbstractCount<TValue> : ICount<TValue>
        where TValue : struct
    {
        /// <summary>
        /// 值类型。
        /// </summary>
        [NotMapped]
        public virtual Type ValueType
            => typeof(TValue);


        /// <summary>
        /// 转换为计数值。
        /// </summary>
        /// <param name="count">给定的计数对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ToCountValue(object count, string? paramName)
            => count.AsNotNull<TValue>(paramName);

        /// <summary>
        /// 转换为减量值。
        /// </summary>
        /// <param name="decrement">给定的减量对象（如果对象为空，则返回默认值）。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ToDecrementValue(object? decrement = null)
            => decrement == null ? default : (TValue)decrement;

        /// <summary>
        /// 转换为增量值。
        /// </summary>
        /// <param name="increment">给定的增量对象（如果对象为空，则返回默认值）。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public virtual TValue ToIncrementValue(object? increment = null)
            => increment == null ? default : (TValue)increment;


        /// <summary>
        /// 累减对象计数。
        /// </summary>
        /// <param name="count">给定要累减的计数对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="decrement">给定的减量（可选；默认减1）。</param>
        /// <returns>返回对象。</returns>
        public virtual object? DegressiveObjectCount(object count, string? paramName, object? decrement = null)
        {
            DegressiveCount(ToCountValue(count, paramName), ToDecrementValue(decrement));

            return count;
        }

        /// <summary>
        /// 异步累减对象计数。
        /// </summary>
        /// <param name="count">给定要累减的计数对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="decrement">给定的减量（可选；默认减1）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
        public virtual async ValueTask<object> DegressiveObjectCountAsync(object count, string? paramName,
            object? decrement = null, CancellationToken cancellationToken = default)
        {
            await DegressiveCountAsync(ToCountValue(count, paramName), ToDecrementValue(decrement), cancellationToken)
                .ConfigureAwaitWithoutContext();

            return count;
        }


        /// <summary>
        /// 累加对象计数。
        /// </summary>
        /// <param name="count">给定要累加的计数对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="increment">给定的增量（可选；默认加1）。</param>
        /// <returns>返回对象。</returns>
        public virtual object ProgressiveObjectCount(object count, string? paramName, object? increment = null)
        {
            ProgressiveCount(ToCountValue(count, paramName), ToIncrementValue(increment));

            return count;
        }

        /// <summary>
        /// 异步累加对象计数。
        /// </summary>
        /// <param name="count">给定要累加的计数对象。</param>
        /// <param name="paramName">给定的参数名称。</param>
        /// <param name="increment">给定的增量（可选；默认加1）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{Object}"/>。</returns>
        public virtual async ValueTask<object> ProgressiveObjectCountAsync(object count, string? paramName,
            object? increment = null, CancellationToken cancellationToken = default)
        {
            await ProgressiveCountAsync(ToCountValue(count, paramName), ToIncrementValue(increment), cancellationToken)
                .ConfigureAwaitWithoutContext();

            return count;
        }


        /// <summary>
        /// 累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数值。</param>
        /// <param name="decrement">给定的减量（可选）。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public abstract TValue DegressiveCount(TValue count, TValue decrement = default);

        /// <summary>
        /// 异步累减计数。
        /// </summary>
        /// <param name="count">给定要累减的计数值。</param>
        /// <param name="decrement">给定的减量（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public abstract ValueTask<TValue> DegressiveCountAsync(TValue count,
            TValue decrement = default, CancellationToken cancellationToken = default);


        /// <summary>
        /// 累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数值。</param>
        /// <param name="increment">给定的增量（可选）。</param>
        /// <returns>返回 <typeparamref name="TValue"/>。</returns>
        public abstract TValue ProgressiveCount(TValue count, TValue increment = default);

        /// <summary>
        /// 异步累加计数。
        /// </summary>
        /// <param name="count">给定要累加的计数值。</param>
        /// <param name="increment">给定的增量（可选）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回 <see cref="ValueTask{TValue}"/>。</returns>
        public abstract ValueTask<TValue> ProgressiveCountAsync(TValue count,
            TValue increment = default, CancellationToken cancellationToken = default);
    }
}

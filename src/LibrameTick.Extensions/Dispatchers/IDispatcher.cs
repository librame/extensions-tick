#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dispatchers;

/// <summary>
/// 定义一个用于处理集合的调度器接口。
/// </summary>
/// <typeparam name="TSource">指定的来源类型。</typeparam>
public interface IDispatcher<TSource>
{
    /// <summary>
    /// 调用指定的方法。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    void Invoke(Action<TSource> action);

    /// <summary>
    /// 调用指定的方法并返回结果集合。
    /// </summary>
    /// <typeparam name="TResult">指定的返回结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <see cref="IEnumerable{TResult}"/>。</returns>
    IEnumerable<TResult> Invoke<TResult>(Func<TSource, TResult> func);
}

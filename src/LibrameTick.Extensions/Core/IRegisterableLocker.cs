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
/// 定义一个实现 <see cref="IRegisterable"/> 的可注册锁定器接口。
/// </summary>
public interface IRegisterableLocker : IRegisterable, IDisposable
{
    /// <summary>
    /// 使用 SpinLock 锁定动作。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    void SpinLock(Action action);

    /// <summary>
    /// 使用 SpinLock 锁定方法。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult SpinLock<TResult>(Func<TResult> func);


    /// <summary>
    /// 使用 Monitor 锁定动作。
    /// </summary>
    /// <param name="action">给定的动作（传入参数为锁定器索引）。</param>
    void Lock(Action<int> action);

    /// <summary>
    /// 使用 Monitor 锁定方法。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的方法（传入参数为锁定器索引）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult Lock<TResult>(Func<int, TResult> func);
}

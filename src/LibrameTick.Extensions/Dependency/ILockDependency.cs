#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Dependency;

/// <summary>
/// 定义继承 <see cref="IDependency"/> 的锁依赖接口。
/// </summary>
public interface ILockDependency : IDisposable, IDependency
{

    #region Monitor

    /// <summary>
    /// 获取最大锁数。
    /// </summary>
    /// <remarks>
    /// 默认为处理器线程数。
    /// </remarks>
    int MaxLockersCount { get; }

    /// <summary>
    /// 获取断定为死锁的持续时长。
    /// </summary>
    /// <remarks>
    /// 默认为 3 秒，表示持续 3 秒即为死锁。
    /// </remarks>
    TimeSpan DeadlockDuration { get; }


    /// <summary>
    /// 使用 Monitor 锁定动作。
    /// </summary>
    /// <param name="action">给定的动作。</param>
    void Lock(Action action);

    /// <summary>
    /// 使用 Monitor 锁定动作。
    /// </summary>
    /// <param name="action">给定的动作（传入参数为锁索引）。</param>
    void Lock(Action<int> action);

    /// <summary>
    /// 使用 Monitor 锁定方法。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult Lock<TResult>(Func<TResult> func);

    /// <summary>
    /// 使用 Monitor 锁定方法。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的方法（传入参数为锁索引）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult Lock<TResult>(Func<int, TResult> func);

    #endregion


    #region Mutex

    /// <summary>
    /// 使用互斥锁。用于多线程中防止两条线程同时对一个公共资源进行读写的机制。
    /// </summary>
    /// <remarks>
    /// 注：Mutex 跟 lock 相似，但是 Mutex 支持多个进程。Mutex 大约比 lock 慢 20 倍。
    /// </remarks>
    /// <param name="action">给定的动作。</param>
    /// <param name="initiallyOwned">指示调用线程是否应具有互斥体的初始所有权。</param>
    /// <param name="name">给定的字符串是否为互斥体的名称。</param>
    /// <param name="createdNew">当线程返回时可指示调用线程是否已赋予互斥体的初始所有权。</param>
    void Mutex(Action action, bool initiallyOwned, string? name, out bool createdNew);

    /// <summary>
    /// 使用互斥锁。用于多线程中防止两条线程同时对一个公共资源进行读写的机制。
    /// </summary>
    /// <remarks>
    /// 注：Mutex 跟 lock 相似，但是 Mutex 支持多个进程。Mutex 大约比 lock 慢 20 倍。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <param name="initiallyOwned">指示调用线程是否应具有互斥体的初始所有权。</param>
    /// <param name="name">给定的字符串是否为互斥体的名称。</param>
    /// <param name="createdNew">当线程返回时可指示调用线程是否已赋予互斥体的初始所有权。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult Mutex<TResult>(Func<TResult> func, bool initiallyOwned, string? name, out bool createdNew);

    #endregion


    #region SpinLock

    /// <summary>
    /// 使用自旋锁。
    /// </summary>
    /// <remarks>
    /// 注：自旋锁特点为互斥、自旋、非重入、只能用于极短暂的运算，进程内使用。
    /// </remarks>
    /// <param name="action">给定的动作。</param>
    void SpinLock(Action action);

    /// <summary>
    /// 使用自旋锁。
    /// </summary>
    /// <remarks>
    /// 注：自旋锁特点为互斥 、自旋、非重入、只能用于极短暂的运算，进程内使用。
    /// </remarks>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的方法。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult SpinLock<TResult>(Func<TResult> func);

    #endregion

}

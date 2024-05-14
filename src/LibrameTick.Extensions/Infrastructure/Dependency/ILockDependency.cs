#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Dependency;

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
    /// <param name="action">给定的动作（传入参数为锁索引）。</param>
    void Lock(Action<int> action);

    /// <summary>
    /// 使用 Monitor 锁定方法。
    /// </summary>
    /// <typeparam name="TResult">指定的结果类型。</typeparam>
    /// <param name="func">给定的方法（传入参数为锁索引）。</param>
    /// <returns>返回 <typeparamref name="TResult"/>。</returns>
    TResult Lock<TResult>(Func<int, TResult> func);

    #endregion


    #region SpinLock

    /// <summary>
    /// 使用自旋锁。
    /// </summary>
    /// <remarks>
    /// 注：自旋锁特点为互斥 、自旋、非重入、只能用于极短暂的运算，进程内使用。
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

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义抽象实现 <see cref="IDisposable"/> 的抽象可处置资源类。
/// </summary>
public abstract class AbstractDisposable : IDisposable
{
    private bool _disposed = false;


    /// <summary>
    /// 析构当前 <see cref="AbstractDisposable"/> 对象实例。
    /// </summary>
    ~AbstractDisposable()
    {
        Dispose(disposing: false);
    }


    /// <summary>
    /// 是否已处置资源。
    /// </summary>
    public bool IsDisposed => _disposed;


    /// <summary>
    /// 未释放资源。
    /// </summary>
    /// <param name="action">给定未释放资源时，要执行的动作。</param>
    public void NotDisposed(Action action)
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);

        action();
    }

    /// <summary>
    /// 未释放资源。
    /// </summary>
    /// <typeparam name="TValue">指定的值类型。</typeparam>
    /// <param name="func">给定未释放资源时，要执行的函数。</param>
    /// <returns>返回值。</returns>
    public TValue NotDisposed<TValue>(Func<TValue> func)
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().Name);

        return func();
    }


    /// <summary>
    /// 处置资源。
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 处置资源。
    /// </summary>
    /// <param name="disposing">立即处置资源。</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            // 防止被多次调用。
            return;
        }

        if (disposing)
        {
            // 释放已托管资源。
            ReleaseManaged();
        }

        // 释放原生（非托管）资源。
        ReleaseNative();

        _disposed = true;
    }


    /// <summary>
    /// 释放已托管资源。
    /// </summary>
    /// <returns>返回是否成功释放的布尔值。</returns>
    protected abstract bool ReleaseManaged();

    /// <summary>
    /// 释放原生（非托管）资源。
    /// </summary>
    /// <returns>返回是否成功释放的布尔值。</returns>
    protected virtual bool ReleaseNative()
        => true;

}

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
/// 定义抽象实现 <see cref="IDisposable"/> 的可释放资源类。
/// </summary>
public abstract class Disposable : IDisposable
{
    private bool _disposed = false;


    /// <summary>
    /// 析构当前 <see cref="Disposable"/> 对象实例。
    /// </summary>
    ~Disposable()
    {
        Dispose(disposing: false);
    }


    /// <summary>
    /// 是否已释放资源。
    /// </summary>
    public bool IsDisposed => _disposed;


    /// <summary>
    /// 如果已释放资源则抛出 <see cref="ObjectDisposedException"/> 异常。
    /// </summary>
    public void ThrowIfDisposed()
        => ObjectDisposedException.ThrowIf(_disposed, this);


    /// <summary>
    /// 释放资源。
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放资源。
    /// </summary>
    /// <param name="disposing">立即释放资源。</param>
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
    /// <remarks>
    /// 通常是指包装操作系统资源的对象，例如文件句柄、内存块、GDI与COM对象、线程与进程、网络套接字、数据库连接、文件映射、Windows API等。
    /// </remarks>
    /// <returns>返回是否成功释放的布尔值。</returns>
    protected virtual bool ReleaseNative()
        => true;

}

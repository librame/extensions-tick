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
/// 定义抽象实现 <see cref="IAutoloader"/>。
/// </summary>
public abstract class AbstractAutoloader : IAutoloader
{
    /// <summary>
    /// 是否已自启动实例。
    /// </summary>
    public bool IsAutoloaded { get; private set; }


    /// <summary>
    /// 自启动实例。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    public virtual void Autoload(IServiceProvider services)
    {
        if (!IsAutoloaded)
        {
            OnAutoload(services);

            IsAutoloaded = true;
        }
    }

    /// <summary>
    /// 异步自启动实例。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    public virtual async Task AutoloadAsync(IServiceProvider services,
        CancellationToken cancellationToken = default)
    {
        if (!IsAutoloaded)
        {
            await OnAutoloadAsync(services, cancellationToken);

            IsAutoloaded = true;
        }
    }


    /// <summary>
    /// 在自启动实例。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    protected abstract void OnAutoload(IServiceProvider services);

    /// <summary>
    /// 异步在自启动实例。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    protected abstract Task OnAutoloadAsync(IServiceProvider services,
        CancellationToken cancellationToken = default);
}

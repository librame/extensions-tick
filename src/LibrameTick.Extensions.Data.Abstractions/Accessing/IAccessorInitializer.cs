#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Accessing;

/// <summary>
/// 定义 <see cref="IAccessor"/> 初始化器接口。
/// </summary>
public interface IAccessorInitializer
{
    /// <summary>
    /// 是否已填充。
    /// </summary>
    bool IsPopulated { get; }


    /// <summary>
    /// 初始化存取器。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    void Initialize(IAccessor accessor, IServiceProvider services);

    /// <summary>
    /// 异步初始化存取器。
    /// </summary>
    /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
    /// <param name="services">给定的 <see cref="IServiceProvider"/>。</param>
    /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
    /// <returns>返回一个异步操作。</returns>
    Task InitializeAsync(IAccessor accessor, IServiceProvider services,
        CancellationToken cancellationToken = default);
}

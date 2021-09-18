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
/// 定义抽象实现 <see cref="IExtensionBuilder{TOptions}"/> 并自动注册当前扩展构建器实例。
/// </summary>
/// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
/// <typeparam name="TBuilder">指定的扩展构建器类型。</typeparam>
public abstract class AbstractExtensionBuilder<TOptions, TBuilder> : AbstractExtensionBuilder<TOptions>
    where TOptions : IExtensionOptions
    where TBuilder : IExtensionBuilder<TOptions>
{
    /// <summary>
    /// 构造一个父级 <see cref="AbstractExtensionBuilder{TOptions, TBuilder}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> 或 <paramref name="options"/> 为空。
    /// </exception>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
    protected AbstractExtensionBuilder(IServiceCollection services, TOptions options)
        : base(services, options)
    {
        Services.AddSingleton(typeof(TBuilder), this);
    }

    /// <summary>
    /// 构造一个子级 <see cref="AbstractExtensionBuilder{TOptions, TBuilder}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 或 <paramref name="options"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
    protected AbstractExtensionBuilder(IExtensionBuilder parentBuilder, TOptions options)
        : base(parentBuilder, options)
    {
        Services.AddSingleton(typeof(TBuilder), this);
    }

}


/// <summary>
/// 定义抽象实现 <see cref="IExtensionBuilder{TOptions}"/>。
/// </summary>
/// <typeparam name="TOptions">指定的扩展选项类型。</typeparam>
public abstract class AbstractExtensionBuilder<TOptions> : AbstractExtensionBuilder, IExtensionBuilder<TOptions>
    where TOptions : IExtensionOptions
{
    /// <summary>
    /// 构造一个父级 <see cref="AbstractExtensionBuilder{TOptions}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="services"/> 或 <paramref name="options"/> 为空。
    /// </exception>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
    protected AbstractExtensionBuilder(IServiceCollection services, TOptions options)
        : base(services, options)
    {
        Options = options;

        Services.AddSingleton(typeof(TOptions), options);
    }

    /// <summary>
    /// 构造一个子级 <see cref="AbstractExtensionBuilder{TOptions}"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 或 <paramref name="options"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="options">给定的 <typeparamref name="TOptions"/>。</param>
    protected AbstractExtensionBuilder(IExtensionBuilder parentBuilder, TOptions options)
        : base(parentBuilder, options)
    {
        Options = options;

        Services.AddSingleton(typeof(TOptions), options);
    }


    /// <summary>
    /// 扩展选项。
    /// </summary>
    public new TOptions Options { get; init; }
}

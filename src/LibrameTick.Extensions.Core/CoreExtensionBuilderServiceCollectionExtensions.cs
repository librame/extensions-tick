#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <see cref="CoreExtensionBuilder"/> 与 <see cref="ServiceCollection"/> 静态扩展。
/// </summary>
public static class CoreExtensionBuilderServiceCollectionExtensions
{

    /// <summary>
    /// 添加 Librame 核心扩展构建器。
    /// </summary>
    /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
    /// <param name="setupAction">给定的配置选项动作（可选）。</param>
    /// <returns>返回 <see cref="CoreExtensionBuilder"/>。</returns>
    public static CoreExtensionBuilder AddLibrame(this IServiceCollection services,
        Action<CoreExtensionOptions>? setupAction = null)
    {
        var options = new CoreExtensionOptions();
        options.TryLoadOptionsFromJson();

        setupAction?.Invoke(options);

        return new CoreExtensionBuilder(services, options);
    }

    /// <summary>
    /// 添加 Librame 扩展构建器。
    /// </summary>
    /// <typeparam name="TBuilder">指定实现 <see cref="IExtensionBuilder"/> 的扩展构建器类型。</typeparam>
    /// <typeparam name="TOptions">指定实现 <see cref="IExtensionOptions"/> 的扩展选项类型。</typeparam>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="setupAction">给定的配置选项动作（可选）。</param>
    /// <returns>返回 <typeparamref name="TBuilder"/>。</returns>
    public static TBuilder AddLibrameExtension<TBuilder, TOptions>(this IExtensionBuilder parentBuilder,
        Action<TOptions>? setupAction = null)
        where TBuilder : class, IExtensionBuilder
        where TOptions : class, IExtensionOptions
    {
        var options = ExpressionExtensions.New<TOptions>(parentBuilder.Options, typeof(IExtensionOptions));
        options.TryLoadOptionsFromJson();

        setupAction?.Invoke(options);

        return ExpressionExtensions.New<TBuilder>(new object[] { parentBuilder, options },
            new Type[] { typeof(IExtensionBuilder), typeof(TOptions) });
    }

}

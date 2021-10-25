#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;
using Librame.Extensions.Drawing;
using Librame.Extensions.Drawing.Drawers;
using Librame.Extensions.Drawing.Verification;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <see cref="DrawingExtensionBuilder"/> 静态扩展。
/// </summary>
public static class DrawingExtensionBuilderExtensions
{

    /// <summary>
    /// 注册 Librame 图画扩展构建器。
    /// </summary>
    /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="setupOptions">给定用于设置选项的动作（可选；为空则不设置）。</param>
    /// <param name="configOptions">给定使用 <see cref="IConfiguration"/> 的选项配置（可选；为空则不配置）。</param>
    /// <param name="setupBuilder">给定用于设置构建器的动作（可选；为空则不设置）。</param>
    /// <returns>返回 <see cref="DrawingExtensionOptions"/>。</returns>
    public static DrawingExtensionBuilder AddDrawing(this IExtensionBuilder parentBuilder,
        Action<DrawingExtensionOptions>? setupOptions = null, IConfiguration? configOptions = null,
        Action<DrawingExtensionBuilder>? setupBuilder = null)
    {
        if (configOptions is null)
            configOptions = typeof(DrawingExtensionOptions).GetConfigOptionsFromJson();

        var builder = new DrawingExtensionBuilder(parentBuilder, setupOptions, configOptions);
        setupBuilder?.Invoke(builder);

        builder.AddDrawers().AddVerification();

        return builder;
    }


    private static DrawingExtensionBuilder AddDrawers(this DrawingExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<ISavingDrawer, InternalSavingDrawer>();
        builder.TryAddOrReplaceService<IScalingDrawer, InternalScaleDrawer>();
        builder.TryAddOrReplaceService<IWatermarkDrawer, InternalWatermarkDrawer>();

        return builder;
    }

    private static DrawingExtensionBuilder AddVerification(this DrawingExtensionBuilder builder)
    {
        builder.TryAddOrReplaceService<ICaptchaGenerator, InternalCaptchaGenerator>();

        return builder;
    }

}

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

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// <see cref="DrawingExtensionBuilder"/> 静态扩展。
/// </summary>
public static class DrawingExtensionBuilderExtensions
{

    /// <summary>
    /// 添加 <see cref="DrawingExtensionBuilder"/>。
    /// </summary>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="setupAction">给定的配置选项动作（可选）。</param>
    /// <returns>返回 <see cref="DrawingExtensionBuilder"/>。</returns>
    public static DrawingExtensionBuilder AddDrawing(this IExtensionBuilder parentBuilder,
        Action<DrawingExtensionOptions>? setupAction = null)
        => parentBuilder.AddLibrameExtension<DrawingExtensionBuilder, DrawingExtensionOptions>(setupAction);

}

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
using Librame.Extensions.Drawing.Drawers;
using Librame.Extensions.Drawing.Verification;

namespace Librame.Extensions.Drawing;

/// <summary>
/// 定义实现 <see cref="IExtensionBuilder"/> 的图画扩展构建器。
/// </summary>
public class DrawingExtensionBuilder : BaseExtensionBuilder<DrawingExtensionBuilder, DrawingExtensionOptions>
{
    /// <summary>
    /// 构造一个 <see cref="DrawingExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    /// <param name="setupOptions">给定用于设置选项的动作（可选；为空则不设置）。</param>
    /// <param name="configOptions">给定使用 <see cref="IConfiguration"/> 的选项配置（可选；为空则不配置）。</param>
    public DrawingExtensionBuilder(IExtensionBuilder parentBuilder,
        Action<DrawingExtensionOptions>? setupOptions = null, IConfiguration? configOptions = null)
        : base(parentBuilder, setupOptions, configOptions)
    {
        // Drawers
        ServiceCharacteristics.AddSingleton<ISavingDrawer>();
        ServiceCharacteristics.AddSingleton<IScalingDrawer>();
        ServiceCharacteristics.AddSingleton<IWatermarkDrawer>();

        // Verification
        ServiceCharacteristics.AddScope<ICaptchaGenerator>();
    }

}

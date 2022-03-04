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
public class DrawingExtensionBuilder : AbstractExtensionBuilder<DrawingExtensionBuilder>
{
    /// <summary>
    /// 构造一个 <see cref="DrawingExtensionBuilder"/>。
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="parentBuilder"/> 为空。
    /// </exception>
    /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
    public DrawingExtensionBuilder(IExtensionBuilder parentBuilder)
        : base(parentBuilder)
    {
        // Drawers
        ServiceCharacteristics.AddSingleton<ISavingDrawer>();
        ServiceCharacteristics.AddSingleton<IScalingDrawer>();
        ServiceCharacteristics.AddSingleton<IWatermarkDrawer>();

        // Verification
        ServiceCharacteristics.AddScope<ICaptchaGenerator>();
    }

}

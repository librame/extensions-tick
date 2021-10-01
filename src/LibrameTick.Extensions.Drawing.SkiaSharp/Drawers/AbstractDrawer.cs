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

namespace Librame.Extensions.Drawing.Drawers;

/// <summary>
/// 定义抽象实现 <see cref="IDrawer"/> 的绘制器。
/// </summary>
public abstract class AbstractDrawer : AbstractDisposable, IDrawer
{
    /// <summary>
    /// 构造一个 <see cref="InternalWatermarkDrawer"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DrawingExtensionOptions"/>。</param>
    protected AbstractDrawer(DrawingExtensionOptions options)
    {
        Options = options;
    }


    /// <summary>
    /// 图画扩展选项。
    /// </summary>
    public DrawingExtensionOptions Options { get; }


    /// <summary>
    /// 绘制位图列表。
    /// </summary>
    /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
    /// <returns>返回 <see cref="IBitmapList"/>。</returns>
    public virtual IBitmapList Draw(IBitmapList bitmaps)
    {
        if (bitmaps.Count > 0)
            return DrawCore(bitmaps);

        return bitmaps;
    }

    /// <summary>
    /// 绘制位图列表核心。
    /// </summary>
    /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
    /// <returns>返回 <see cref="IBitmapList"/>。</returns>
    protected abstract IBitmapList DrawCore(IBitmapList bitmaps);

}

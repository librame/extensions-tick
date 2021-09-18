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

namespace Librame.Extensions.Drawing.Processing;

/// <summary>
/// 定义实现 <see cref="IProcessor"/> 的水印图画处理器。
/// </summary>
public abstract class AbstractDrawableProcessor : AbstractDisposable, IDrawableProcessor
{
    /// <summary>
    /// 构造一个 <see cref="InternalWatermarkDrawableProcessor"/>。
    /// </summary>
    /// <param name="options">给定的 <see cref="DrawingExtensionOptions"/>。</param>
    protected AbstractDrawableProcessor(DrawingExtensionOptions options)
    {
        Options = options;
    }


    /// <summary>
    /// 图画扩展选项。
    /// </summary>
    public DrawingExtensionOptions Options { get; }


    /// <summary>
    /// 处理核心位图列表。
    /// </summary>
    /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
    /// <returns>返回 <see cref="IBitmapList"/>。</returns>
    protected abstract IBitmapList ProcessCore(IBitmapList bitmaps);


    /// <summary>
    /// 处理位图列表。
    /// </summary>
    /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
    /// <returns>返回 <see cref="IBitmapList"/>。</returns>
    public virtual IBitmapList Process(IBitmapList bitmaps)
    {
        if (bitmaps.Count > 0)
            return ProcessCore(bitmaps);

        return bitmaps;
    }

}

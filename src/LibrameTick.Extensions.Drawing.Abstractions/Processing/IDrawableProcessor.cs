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

namespace Librame.Extensions.Drawing.Processing
{
    /// <summary>
    /// 定义实现 <see cref="IProcessor"/> 的可绘制处理器接口。
    /// </summary>
    public interface IDrawableProcessor : IProcessor
    {
        /// <summary>
        /// 处理位图列表。
        /// </summary>
        /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
        /// <returns>返回 <see cref="IBitmapList"/>。</returns>
        IBitmapList Process(IBitmapList bitmaps);
    }
}

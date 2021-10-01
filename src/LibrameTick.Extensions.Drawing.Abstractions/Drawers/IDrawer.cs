#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Drawers;

/// <summary>
/// 定义实现 <see cref="IDisposable"/> 的绘制器接口。
/// </summary>
public interface IDrawer : IDisposable
{
    /// <summary>
    /// 绘制位图列表。
    /// </summary>
    /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
    /// <returns>返回 <see cref="IBitmapList"/>。</returns>
    IBitmapList Draw(IBitmapList bitmaps);
}

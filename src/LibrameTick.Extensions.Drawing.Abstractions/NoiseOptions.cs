#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Infrastructure;

namespace Librame.Extensions.Drawing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的噪点选项。
/// </summary>
public class NoiseOptions : IOptions
{
    /// <summary>
    /// 噪点宽度。
    /// </summary>
    public int Width { get; set; } = 2;

    /// <summary>
    /// 噪点间距。
    /// </summary>
    public Point Space { get; set; } = new(x: 5, y: 5);
}

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

namespace Librame.Extensions.Drawing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的字体选项。
/// </summary>
public class FontOptions : IOptions
{
    /// <summary>
    /// 字体文件（默认为“font.ttf”）。
    /// </summary>
    public string File { get; set; } = "font.ttf";

    /// <summary>
    /// 字体大小（默认为“16F”）。
    /// </summary>
    public float Size { get; set; } = 16F;

    /// <summary>
    /// 字体参数集合。
    /// </summary>
    public string? Parameters { get; set; }
}

#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的水印选项。
/// </summary>
public class WatermarkOptions : IOptions
{
    /// <summary>
    /// 字体选项。
    /// </summary>
    public FontOptions Font { get; set; } = new() { Size = 32F };


    /// <summary>
    /// 文件名称后缀（默认为“-wm”）。
    /// </summary>
    public string FileNameSuffix { get; set; } = "-wm";

    /// <summary>
    /// 水印模式（默认为 <see cref="WatermarkMode.Image"/>，请确保 <see cref="ImageFile"/> 设置的水印图片文件已存在）。
    /// </summary>
    public WatermarkMode Mode { get; set; } = WatermarkMode.Image;

    /// <summary>
    /// 初始坐标（支持负值表示反向，默认为“x:30, y:50”）。
    /// </summary>
    public Point InitialCoord { get; set; } = new(30, 50);

    /// <summary>
    /// 为多张图片添加水印时，是否使用随机坐标（默认不随机）。
    /// </summary>
    public bool IsRandomCoord { get; set; }

    /// <summary>
    /// 文本内容（默认为“Librame”）。
    /// </summary>
    public string Text { get; set; } = nameof(Librame);

    /// <summary>
    /// 图片文件（默认水印图片文件名为“watermark.png”）。
    /// </summary>
    public string ImageFile { get; set; } = "watermark.png";
}

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
using Librame.Extensions.Drawing.Verification;

namespace Librame.Extensions.Drawing;

/// <summary>
/// 图画扩展选项。
/// </summary>
public class DrawingExtensionOptions : AbstractExtensionOptions<DrawingExtensionOptions>
{
    /// <summary>
    /// 构造一个 <see cref="DrawingExtensionOptions"/>。
    /// </summary>
    public DrawingExtensionOptions()
    {
        ImageDirectory = Directories.ResourceDirectory.CombineDirectory("images");
    }


    /// <summary>
    /// 色彩选项。
    /// </summary>
    public ColorOptions Colors { get; set; } = new();

    /// <summary>
    /// 验证码选项。
    /// </summary>
    public CaptchaOptions Captcha { get; set; } = new();

    /// <summary>
    /// 缩放选项。
    /// </summary>
    public ScaleOptions Scale { get; set; } = new();

    /// <summary>
    /// 水印选项。
    /// </summary>
    public WatermarkOptions Watermark { get; set; } = new();


    /// <summary>
    /// 图像目录（主要用于存储图像）。
    /// </summary>
    public string ImageDirectory { get; set; }

    /// <summary>
    /// 图像格式（默认为“jpeg”）。
    /// </summary>
    public string ImageFormat { get; set; } = "jpeg";

    /// <summary>
    /// 编码品质（取值范围：1-100；默认为 80）。
    /// </summary>
    public int EncodeQuality { get; set; } = 80;

    /// <summary>
    /// 调整尺寸品质。
    /// </summary>
    public SKFilterQuality ResizeQuality { get; set; } = SKFilterQuality.Medium;

    /// <summary>
    /// 支持的图像扩展名列表。
    /// </summary>
    public List<string> SupportImageExtensions { get; set; } = new()
    {
        ".bmp",
        ".jpg",
        ".jpeg",
        ".png"
    };

}

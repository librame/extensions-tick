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
/// 定义实现 <see cref="IOptions"/> 的色彩选项。
/// </summary>
public class ColorOptions : IOptions
{
    /// <summary>
    /// 前景色（默认为“#0066cc”）。
    /// </summary>
    public string Fore { get; set; } = "#0066cc";

    /// <summary>
    /// 背景色（默认为“#ccffff”）。
    /// </summary>
    public string Background { get; set; } = "#ccffff";

    /// <summary>
    /// 交替色（默认为“#993366”）。
    /// </summary>
    public string Alternate { get; set; } = "#993366";

    /// <summary>
    /// 干扰色（默认为“#99ccff”）。
    /// </summary>
    public string Interference { get; set; } = "#99ccff";

    /// <summary>
    /// 阴影色（默认为“#ccffff”）。
    /// </summary>
    public string Shadow { get; set; } = "#ccffff";

    /// <summary>
    /// 水印色（默认为“#ffffff”）。
    /// </summary>
    public string Watermark { get; set; } = "#ffffff";
}

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
/// 定义实现 <see cref="IOptions"/> 的缩放选项。
/// </summary>
public class ScaleOptions : IOptions
{
    /// <summary>
    /// 添加缩放尺寸后缀（默认添加）。
    /// </summary>
    public bool AddScaleSizeSuffix { get; set; } = true;

    /// <summary>
    /// 缩放描述符列表（默认支持小-无水印、中-有水印、大-无水印等三种缩放尺寸）。
    /// </summary>
    public List<ScaleDescriptor> Descriptors { get; set; } = new()
    {
        new("-small", new Size(100, 60), AddWatermark: false),
        new("-medium", new Size(1000, 600), AddWatermark: true),
        new("-large", new Size(2000, 1200), AddWatermark: false)
    };
}

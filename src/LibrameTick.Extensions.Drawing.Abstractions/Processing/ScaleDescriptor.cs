#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Processing;

/// <summary>
/// 定义表示图像缩放的描述符。
/// </summary>
/// <param name="FileNameSuffix">给定的文件命名后缀。</param>
/// <param name="MaxSize">给定的最大尺寸。</param>
/// <param name="AddWatermark">是否添加水印。</param>
public record ScaleDescriptor(string FileNameSuffix, Size MaxSize, bool AddWatermark);

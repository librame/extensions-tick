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
/// SkiaSharp 静态扩展。
/// </summary>
public static class SkiaSharpExtensions
{

    /// <summary>
    /// 将颜色字符串转换为颜色实例。
    /// </summary>
    /// <param name="color">给定的颜色字符串。</param>
    /// <returns>返回 <see cref="SKColor"/>。</returns>
    public static SKColor AsColor(this string color)
        => SKColor.Parse(color);

    /// <summary>
    /// 将图像格式字符串转换为已编码图像格式枚举。
    /// </summary>
    /// <param name="imageFormat">给定的图像格式。</param>
    /// <returns>返回 <see cref="SKEncodedImageFormat"/>。</returns>
    public static SKEncodedImageFormat AsEncodedImageFormat(this string imageFormat)
        => Enum.Parse<SKEncodedImageFormat>(imageFormat, ignoreCase: true);


    /// <summary>
    /// 计算 <see cref="SKBitmap"/> 的键。
    /// </summary>
    /// <param name="bitmap">给定的 <see cref="SKBitmap"/>。</param>
    /// <returns>返回 SHA256 哈希字符串。</returns>
    public static string ComputeKey(this SKBitmap bitmap)
        => bitmap.Bytes.ComputeBitmapKey();

    /// <summary>
    /// 计算位图字节数组的键。
    /// </summary>
    /// <param name="bitmapBuffer">给定的位图字节数组。</param>
    /// <returns>返回 SHA256 哈希字符串。</returns>
    public static string ComputeBitmapKey(this byte[] bitmapBuffer)
        => bitmapBuffer.AsSha256().AsBase64String();


    /// <summary>
    /// 创建字体绘画工具。
    /// </summary>
    /// <param name="font">给定的 <see cref="FontOptions"/>。</param>
    /// <param name="color">给定的字体颜色。</param>
    /// <returns>返回 <see cref="SKPaint"/>。</returns>
    public static SKPaint CreatePaint(this FontOptions font, string color)
    {
        var paint = new SKPaint();

        paint.IsAntialias = true;
        paint.Color = color.AsColor();
        // paint.StrokeCap = SKStrokeCap.Round;
        paint.Typeface = SKTypeface.FromFile(font.File);
        paint.TextSize = font.Size;

        return paint;
    }

    /// <summary>
    /// 创建噪点绘画工具。
    /// </summary>
    /// <param name="noise">给定的 <see cref="NoiseOptions"/>。</param>
    /// <param name="color">给定的字体颜色。</param>
    /// <returns>返回 <see cref="SKPaint"/>。</returns>
    public static SKPaint CreatePaint(this NoiseOptions noise, string color)
    {
        var paint = new SKPaint();

        paint.IsAntialias = true;
        paint.Color = color.AsColor();
        paint.StrokeCap = SKStrokeCap.Square;
        paint.StrokeWidth = noise.Width;

        return paint;
    }


    /// <summary>
    /// 创建噪点数组。
    /// </summary>
    /// <param name="noise">给定的 <see cref="NoiseOptions"/>。</param>
    /// <param name="imageSize">给定的图像 <see cref="Size"/>。</param>
    /// <returns>返回 <see cref="SKPoint"/> 数组。</returns>
    public static SKPoint[] CreatePoints(this NoiseOptions noise, Size imageSize)
    {
        var points = new List<SKPoint>();

        var offset = noise.Width;
        var xCount = imageSize.Width / noise.Space.X + offset;
        var yCount = imageSize.Height / noise.Space.Y + offset;

        for (var i = 0; i < xCount; i++)
        {
            for (var j = 0; j < yCount; j++)
            {
                var point = new SKPoint(i * noise.Space.X, j * noise.Space.Y);
                points.Add(point);
            }
        }

        return points.ToArray();
    }

}

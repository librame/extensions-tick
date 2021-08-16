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
using SkiaSharp;
using System.Drawing;

namespace Librame.Extensions.Drawing.Processing
{
    /// <summary>
    /// 定义实现 <see cref="IProcessor"/> 的水印图画处理器。
    /// </summary>
    public class WatermarkDrawingProcessor : AbstractDisposable, IDrawingProcessor
    {
        private readonly DrawingExtensionOptions _options;
        private SKBitmap? _watermarkBitmap;


        /// <summary>
        /// 构造一个 <see cref="WatermarkDrawingProcessor"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="DrawingExtensionOptions"/>。</param>
        public WatermarkDrawingProcessor(DrawingExtensionOptions options)
        {
            _options = options;

            // 限定水印图像文件的存储在模块的资源目录中
            var watermarkFile = _options.Watermark.ImageFile
                .SetBasePath(_options.Directories.ResourceDirectory);

            if (watermarkFile.FileExists())
                _watermarkBitmap = SKBitmap.Decode(watermarkFile);
        }


        /// <summary>
        /// 释放已托管资源。
        /// </summary>
        /// <returns>返回是否成功释放的布尔值。</returns>
        protected override bool ReleaseManaged()
        {
            if (_watermarkBitmap != null)
                _watermarkBitmap.Dispose();

            return true;
        }


        /// <summary>
        /// 处理图画。
        /// </summary>
        /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
        /// <returns>返回 <see cref="IDrawingProcessor"/>。</returns>
        public IDrawingProcessor Process(IBitmapList bitmaps)
        {
            if (bitmaps.Count > 0)
            {
                if (_options.Watermark.Mode == WatermarkMode.Image
                    && _watermarkBitmap != null)
                {
                    DrawImageWatermark(bitmaps);
                }
                else
                {
                    DrawTextWatermark(bitmaps);
                }
            }
            
            return this;
        }

        private void DrawImageWatermark(IBitmapList bitmaps)
        {
            foreach (SKBitmap bitmap in bitmaps)
            {
                using (var canvas = new SKCanvas(bitmap))
                {
                    var imageSize = new Size(bitmap.Width, bitmap.Height);
                    var coord = ImageHelper.CalculateCoord(imageSize,
                        _options.Watermark.InitialCoord, _options.Watermark.IsRandomCoord);

                    // 绘制图像水印
                    canvas.DrawBitmap(_watermarkBitmap, coord.X, coord.Y);
                }
            }
        }

        private void DrawTextWatermark(IBitmapList bitmaps)
        {
            if (string.IsNullOrEmpty(_options.Watermark.Text))
                throw new ArgumentException("The text watermark cannot be empty.");

            using (var forePaint = CreatePaint(_options.Colors.Fore))
            using (var alternPaint = CreatePaint(_options.Colors.Alternate))
            {
                foreach (SKBitmap bitmap in bitmaps)
                {
                    using (var canvas = new SKCanvas(bitmap))
                    {
                        var imageSize = new Size(bitmap.Width, bitmap.Height);
                        var coord = ImageHelper.CalculateCoord(imageSize,
                            _options.Watermark.InitialCoord, _options.Watermark.IsRandomCoord);

                        // 绘制文本水印
                        for (var i = 0; i < _options.Watermark.Text.Length; i++)
                        {
                            // 当前字符
                            var character = _options.Watermark.Text.Substring(i, 1);

                            // 测算水印文本内容矩形尺寸
                            var rect = new SKRect();
                            forePaint.MeasureText(character, ref rect);

                            // 绘制文本水印
                            canvas.DrawText(character, coord.X + (int)rect.Width, coord.Y,
                                i % 2 > 0 ? forePaint : alternPaint);

                            // 递增字符宽度
                            coord.X += (int)rect.Width;
                        }
                    }
                }
            }
        }

        private SKPaint CreatePaint(string color)
        {
            var paint = new SKPaint();

            paint.IsAntialias = true;
            paint.Color = SKColor.Parse(color);
            // paint.StrokeCap = SKStrokeCap.Round;
            paint.Typeface = SKTypeface.FromFile(_options.Watermark.Font.File);
            paint.TextSize = _options.Watermark.Font.Size;

            return paint;
        }

    }
}

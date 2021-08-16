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
    /// 定义实现 <see cref="IProcessor"/> 的缩放图画处理器。
    /// </summary>
    public class ScaleDrawingProcessor : AbstractDisposable, IDrawingProcessor
    {
        private readonly IClock _clock;
        private readonly DrawingExtensionOptions _options;
        private readonly SKEncodedImageFormat _imageFormat;


        /// <summary>
        /// 构造一个 <see cref="ScaleDrawingProcessor"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="DrawingExtensionOptions"/>。</param>
        public ScaleDrawingProcessor(DrawingExtensionOptions options)
        {
            _options = options;
            _clock = options.CoreOptions.Clock;
            _imageFormat = Enum.Parse<SKEncodedImageFormat>(options.ImageFormat, ignoreCase: true);
        }


        /// <summary>
        /// 释放已托管资源。
        /// </summary>
        /// <returns>返回是否成功释放的布尔值。</returns>
        protected override bool ReleaseManaged()
            => true;


        /// <summary>
        /// 处理图画。
        /// </summary>
        /// <param name="bitmaps">给定的 <see cref="IBitmapList"/>。</param>
        /// <returns>返回 <see cref="IDrawingProcessor"/>。</returns>
        public IDrawingProcessor Process(IBitmapList bitmaps)
        {
            if (bitmaps.Count > 0 && _options.Scales.Count > 0)
            {
                var scaleBitmaps = new List<(Size Size, ScaleDescriptor Descr, SKBitmap Bitmap)>();

                foreach (SKBitmap bitmap in bitmaps)
                {
                    // 遍历缩放信息
                    foreach (var scale in _options.Scales)
                    {
                        // 如果源图尺寸小于缩放尺寸，则跳过当前缩放信息
                        if (bitmap.Width <= scale.MaxSize.Width
                            && bitmap.Height <= scale.MaxSize.Height)
                        {
                            continue;
                        }

                        // 计算等比例缩放尺寸
                        var imageSize = new Size(bitmap.Width, bitmap.Height);
                        var scaleSize = ImageHelper.ScaleSize(imageSize, scale.MaxSize);
                        var scaleInfo = new SKImageInfo(scaleSize.Width, scaleSize.Height,
                            bitmap.Info.ColorType, bitmap.Info.AlphaType);

                        scaleBitmaps.Add((scaleSize, scale, bitmap.Resize(scaleInfo, _options.ResizeQuality)));
                    }
                }

                // 处理水印
                var watermarkBitmaps = scaleBitmaps
                    .Where(tuple => tuple.Descr.AddWatermark)
                    .Select(tuple => tuple.Bitmap)
                    .ToList();

                DrawWatermark(watermarkBitmaps);

                // 保存图像
                var now = _clock.GetNow();
                var baseDirectory = _options.ImageDirectory.SetBasePath();
                baseDirectory = _options.ImageSubdirectoryFunc.Invoke(now).SetBasePath(baseDirectory);

                foreach (var bitmap in scaleBitmaps)
                {
                    SaveImage(bitmap.Bitmap, bitmap.Descr, baseDirectory);
                }
            }

            return this;
        }

        private void DrawWatermark(List<SKBitmap> bitmaps)
        {
            var watermarkProcessor = new WatermarkDrawingProcessor(_options);
            watermarkProcessor.Process(new BitmapList(bitmaps));
        }

        private void SaveImage(SKBitmap bitmap, ScaleDescriptor descriptor, string baseDirectory)
        {
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(_imageFormat, _options.EncodeQuality))
            {
                // 使用时间周期+文件后缀名+文件格式名，如：6123xxxx_small.jpeg
                var fileName = _clock.GetNow().Ticks + descriptor.FileNameSuffix +
                    _options.ImageFormat.Leading('.');

                fileName = fileName.SetBasePath(baseDirectory);

                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    data.SaveTo(fs);
                }
            }
        }

    }
}

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
using Librame.Extensions.Drawing.Processing;
using SkiaSharp;

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 图画扩展选项。
    /// </summary>
    public class DrawingExtensionOptions : AbstractExtensionOptions<DrawingExtensionOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="DrawingExtensionOptions"/>。
        /// </summary>
        /// <param name="parentOptions">给定的父级 <see cref="IExtensionOptions"/>。</param>
        public DrawingExtensionOptions(IExtensionOptions parentOptions)
            : base(parentOptions, parentOptions.Directories)
        {
            CoreOptions = parentOptions.GetRequiredOptions<CoreExtensionOptions>();
            Colors = ColorOptions.CreateLightOptions(Notifier);
            Captcha = new CaptchaOptions(Notifier);
            Scaling = new ScalingOptions(Notifier);
            Watermark = new WatermarkOptions(Notifier);

            ImageDirectory = Directories.BaseDirectory.CombinePath("images");

            // Processing
            ServiceCharacteristics.AddSingleton<ISavingDrawableProcessor>();
            ServiceCharacteristics.AddSingleton<IScalingDrawableProcessor>();
            ServiceCharacteristics.AddSingleton<IWatermarkDrawableProcessor>();
            ServiceCharacteristics.AddSingleton<IProcessorManager>();

            // Verification
            ServiceCharacteristics.AddScope<ICaptchaGenerator>();
        }


        /// <summary>
        /// 核心扩展选项。
        /// </summary>
        public CoreExtensionOptions CoreOptions { get; init; }

        /// <summary>
        /// 色彩选项。
        /// </summary>
        public ColorOptions Colors { get; init; }

        /// <summary>
        /// 验证码选项。
        /// </summary>
        public CaptchaOptions Captcha { get; init; }

        /// <summary>
        /// 缩放选项。
        /// </summary>
        public ScalingOptions Scaling { get; init; }

        /// <summary>
        /// 水印选项。
        /// </summary>
        public WatermarkOptions Watermark { get; init; }


        /// <summary>
        /// 图像目录（主要用于存储图像）。
        /// </summary>
        public string ImageDirectory
        {
            get => Notifier.GetOrAddDirectory(nameof(ImageDirectory), string.Empty);
            set => Notifier.AddOrUpdate(nameof(ImageDirectory), value);
        }

        /// <summary>
        /// 图像格式（默认为“jpeg”）。
        /// </summary>
        public string ImageFormat
        {
            get => Notifier.GetOrAdd(nameof(ImageFormat), "jpeg");
            set => Notifier.AddOrUpdate(nameof(ImageFormat), value);
        }

        /// <summary>
        /// 编码品质（取值范围：1-100；默认为 80）。
        /// </summary>
        public int EncodeQuality
        {
            get => Notifier.GetOrAdd(nameof(EncodeQuality), 80);
            set => Notifier.AddOrUpdate(nameof(EncodeQuality), value);
        }

        /// <summary>
        /// 调整尺寸品质。
        /// </summary>
        public SKFilterQuality ResizeQuality
        {
            get => Notifier.GetOrAdd(nameof(ResizeQuality), SKFilterQuality.Medium);
            set => Notifier.AddOrUpdate(nameof(ResizeQuality), value);
        }

        /// <summary>
        /// 支持的图像扩展名列表。
        /// </summary>
        public List<string> SupportImageExtensions { get; init; }
            = new List<string>
            {
                ".bmp",
                ".jpg",
                ".jpeg",
                ".png"
            };

    }
}

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

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 定义实现 <see cref="IOptions"/> 的色彩选项。
    /// </summary>
    public class ColorOptions : AbstractOptions
    {
        /// <summary>
        /// 构造一个 <see cref="ColorOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        public ColorOptions(IPropertyNotifier parentNotifier)
            : base(parentNotifier)
        {
        }


        /// <summary>
        /// 前景色。
        /// </summary>
        public string Fore
        {
            get => Notifier.GetOrAdd(nameof(Fore), string.Empty);
            set => Notifier.AddOrUpdate(nameof(Fore), value);
        }

        /// <summary>
        /// 背景色。
        /// </summary>
        public string Background
        {
            get => Notifier.GetOrAdd(nameof(Background), string.Empty);
            set => Notifier.AddOrUpdate(nameof(Background), value);
        }

        /// <summary>
        /// 交替色。
        /// </summary>
        public string Alternate
        {
            get => Notifier.GetOrAdd(nameof(Alternate), string.Empty);
            set => Notifier.AddOrUpdate(nameof(Alternate), value);
        }

        /// <summary>
        /// 干扰色。
        /// </summary>
        public string Interference
        {
            get => Notifier.GetOrAdd(nameof(Interference), string.Empty);
            set => Notifier.AddOrUpdate(nameof(Interference), value);
        }

        /// <summary>
        /// 阴影色。
        /// </summary>
        public string Shadow
        {
            get => Notifier.GetOrAdd(nameof(Shadow), string.Empty);
            set => Notifier.AddOrUpdate(nameof(Shadow), value);
        }

        /// <summary>
        /// 水印色。
        /// </summary>
        public string Watermark
        {
            get => Notifier.GetOrAdd(nameof(Watermark), string.Empty);
            set => Notifier.AddOrUpdate(nameof(Watermark), value);
        }


        /// <summary>
        /// 创建明亮色彩选项。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        /// <returns>返回 <see cref="ColorOptions"/>。</returns>
        public static ColorOptions CreateLightOptions(IPropertyNotifier parentNotifier)
        {
            var options = new ColorOptions(parentNotifier);

            options.Fore = "#0066cc";
            options.Background = "#ccffff";
            options.Alternate = "#993366";
            options.Interference = "#99ccff";
            options.Shadow = "#ccffff";
            options.Watermark = "#ffffff";

            return options;
        }

    }
}

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
    /// 定义实现 <see cref="IOptions"/> 的字体选项。
    /// </summary>
    public class FontOptions : AbstractOptions
    {
        /// <summary>
        /// 构造一个 <see cref="FontOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        public FontOptions(IPropertyNotifier parentNotifier)
            : base(parentNotifier)
        {
        }


        /// <summary>
        /// 字体文件。
        /// </summary>
        public string File
        {
            get => Notifier.GetOrAdd(nameof(File), "font.ttf");
            set => Notifier.AddOrUpdate(nameof(File), value);
        }

        /// <summary>
        /// 字体大小。
        /// </summary>
        public float Size
        {
            get => Notifier.GetOrAdd(nameof(Size), 16);
            set => Notifier.AddOrUpdate(nameof(Size), value);
        }

        /// <summary>
        /// 字体参数集合。
        /// </summary>
        public string Parameters
        {
            get => Notifier.GetOrAdd(nameof(Parameters), string.Empty);
            set => Notifier.AddOrUpdate(nameof(Parameters), value);
        }

    }
}

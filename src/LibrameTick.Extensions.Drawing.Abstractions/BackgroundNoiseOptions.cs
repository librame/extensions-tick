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
using System.Drawing;

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 定义实现 <see cref="IOptions"/> 的背景噪点选项。
    /// </summary>
    public class BackgroundNoiseOptions : AbstractOptions
    {
        /// <summary>
        /// 构造一个 <see cref="BackgroundNoiseOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        public BackgroundNoiseOptions(IPropertyNotifier parentNotifier)
            : base(parentNotifier)
        {
        }


        /// <summary>
        /// 噪点宽度。
        /// </summary>
        public int Width
        {
            get => Notifier.GetOrAdd(nameof(Width), 2);
            set => Notifier.AddOrUpdate(nameof(Width), value);
        }

        /// <summary>
        /// 噪点间距。
        /// </summary>
        public PointF Space
        {
            get => Notifier.GetOrAdd(nameof(Space), new PointF(x: 5, y: 5));
            set => Notifier.AddOrUpdate(nameof(Space), value);
        }

    }
}

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

namespace Librame.Extensions.Drawing.Verification
{
    /// <summary>
    /// 定义实现 <see cref="IOptions"/> 的验证码选项。
    /// </summary>
    public class CaptchaOptions : AbstractOptions
    {
        /// <summary>
        /// 构造一个 <see cref="CaptchaOptions"/>。
        /// </summary>
        /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
        public CaptchaOptions(IPropertyNotifier parentNotifier)
            : base(parentNotifier)
        {
            Font = new FontOptions(Notifier);
            BackgroundNoise = new NoiseOptions(Notifier);
        }


        /// <summary>
        /// 字体选项。
        /// </summary>
        public FontOptions Font { get; init; }

        /// <summary>
        /// 背景噪点选项。
        /// </summary>
        public NoiseOptions BackgroundNoise { get; init; }
    }
}

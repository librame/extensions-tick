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

namespace Librame.Extensions.Drawing.Verification;

/// <summary>
/// 定义实现 <see cref="IOptions"/> 的验证码选项。
/// </summary>
public class CaptchaOptions : AbstractOptions
{
    /// <summary>
    /// 构造一个独立属性通知器的 <see cref="CaptchaOptions"/>（此构造函数适用于独立使用 <see cref="CaptchaOptions"/> 的情况）。
    /// </summary>
    /// <param name="sourceAliase">给定的源别名（独立属性通知器必须命名实例）。</param>
    public CaptchaOptions(string sourceAliase)
        : base(sourceAliase)
    {
        Font = new FontOptions(Notifier, nameof(Font));
        BackgroundNoise = new NoiseOptions(Notifier, nameof(BackgroundNoise));
    }

    /// <summary>
    /// 构造一个 <see cref="CaptchaOptions"/>。
    /// </summary>
    /// <param name="parentNotifier">给定的父级 <see cref="IPropertyNotifier"/>。</param>
    /// <param name="sourceAliase">给定的源别名（可选）。</param>
    public CaptchaOptions(IPropertyNotifier parentNotifier, string? sourceAliase = null)
        : base(parentNotifier, sourceAliase)
    {
        Font = new FontOptions(Notifier, nameof(Font));
        BackgroundNoise = new NoiseOptions(Notifier, nameof(BackgroundNoise));
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

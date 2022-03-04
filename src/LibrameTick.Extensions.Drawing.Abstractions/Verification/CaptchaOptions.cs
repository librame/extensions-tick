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
public class CaptchaOptions : IOptions
{
    /// <summary>
    /// 字体选项。
    /// </summary>
    public FontOptions Font { get; set; } = new();

    /// <summary>
    /// 背景噪点选项。
    /// </summary>
    public NoiseOptions BackgroundNoise { get; set; } = new();
}

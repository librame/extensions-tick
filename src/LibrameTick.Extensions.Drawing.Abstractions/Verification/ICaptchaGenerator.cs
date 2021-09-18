#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Verification;

/// <summary>
/// 定义验证码生成器接口。
/// </summary>
public interface ICaptchaGenerator : IDisposable
{
    /// <summary>
    /// 绘制验证码文件。
    /// </summary>
    /// <param name="captcha">给定的验证码。</param>
    /// <param name="savePath">给定的保存路径。</param>
    /// <returns>返回是否成功的布尔值。</returns>
    bool DrawFile(string captcha, string savePath);

    /// <summary>
    /// 绘制验证码流。
    /// </summary>
    /// <param name="captcha">给定的验证码。</param>
    /// <param name="target">给定的目标流。</param>
    /// <returns>返回是否成功的布尔值。</returns>
    bool DrawStream(string captcha, Stream target);

    /// <summary>
    /// 绘制验证码字节数组。
    /// </summary>
    /// <param name="captcha">给定的验证码。</param>
    /// <returns>返回图像字节数组。</returns>
    byte[] DrawBytes(string captcha);
}

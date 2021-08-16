#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Verification
{
    /// <summary>
    /// 定义验证码生成器接口。
    /// </summary>
    public interface ICaptchaGenerator
    {
        /// <summary>
        /// 绘制验证码文件。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="savePath">给定的保存路径。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawFileAsync(string captcha, string savePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// 绘制验证码流。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="target">给定的目标流。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含是否成功的异步操作。</returns>
        Task<bool> DrawStreamAsync(string captcha, Stream target, CancellationToken cancellationToken = default);

        /// <summary>
        /// 绘制验证码字节数组。
        /// </summary>
        /// <param name="captcha">给定的验证码。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含图像字节数组的异步操作。</returns>
        Task<byte[]> DrawBytesAsync(string captcha, CancellationToken cancellationToken = default);
    }
}

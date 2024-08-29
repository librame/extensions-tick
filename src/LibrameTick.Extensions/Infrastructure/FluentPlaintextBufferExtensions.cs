#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Dependency;

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义 <see cref="FluentPlaintextBuffer"/> 静态扩展。
/// </summary>
public static class FluentPlaintextBufferExtensions
{

    /// <summary>
    /// 将明文字符串编码为字节数组缓冲区。
    /// </summary>
    /// <param name="plaintext">给定的明文字符串。</param>
    /// <param name="encoding">给定的字符编码（可选；默认为 <see cref="DependencyRegistration.CurrentContext"/> 的字符编码）。</param>
    /// <returns>返回 <see cref="FluentPlaintextBuffer"/>。</returns>
    public static FluentPlaintextBuffer EncodePlaintextBuffer(this string plaintext, Encoding? encoding = null)
        => new(plaintext, encoding ?? DependencyRegistration.CurrentContext.Encoding);

}

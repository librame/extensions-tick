#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions;

/// <summary>
/// <see cref="Exception"/> 静态扩展。
/// </summary>
public static class ExceptionExtensions
{

    /// <summary>
    /// 获取内部异常消息。
    /// </summary>
    /// <param name="exception">给定的 <see cref="Exception"/>。</param>
    /// <returns>返回异常消息字符串。</returns>
    public static string GetInnerMessage(this Exception exception)
    {
        if (exception.InnerException == null)
            return exception.Message;

        return exception.InnerException.GetInnerMessage();
    }

}

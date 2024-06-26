﻿#region License

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
    /// 获取最内层的异常消息。
    /// </summary>
    /// <param name="exception">给定的 <see cref="Exception"/>。</param>
    /// <returns>返回异常消息字符串。</returns>
    public static string GetInnermostMessage(this Exception exception)
    {
        if (exception.InnerException == null)
            return exception.Message;

        return exception.InnerException.GetInnermostMessage();
    }


    /// <summary>
    /// 写入调试异常。
    /// </summary>
    /// <param name="exception">给定的 <see cref="Exception"/>。</param>
    public static void WriteDebug(this Exception exception)
    {
        Debug.WriteLine(exception.ToString());
        Debug.Assert(false, exception.GetInnermostMessage());
    }

    /// <summary>
    /// 写入调试异常并作为指定异常输出。
    /// </summary>
    /// <typeparam name="TOutputException">指定的输出异常类型。</typeparam>
    /// <param name="exception">给定的 <see cref="Exception"/>。</param>
    /// <param name="outputMessage">给定的输出消息（可空）。</param>
    /// <returns>返回 <typeparamref name="TOutputException"/>。</returns>
    public static TOutputException WriteDebugAs<TOutputException>(this Exception exception, string? outputMessage = null)
        where TOutputException : Exception
    {
        exception.WriteDebug();

        if (!string.IsNullOrWhiteSpace(outputMessage))
            return ExpressionExtensions.New<TOutputException>([outputMessage]);

        return ExpressionExtensions.New<TOutputException>();
    }


    /// <summary>
    /// 获取上次系统错误消息。
    /// </summary>
    /// <returns>返回错误消息字符串。</returns>
    public static string GetLastSystemErrorMessage()
    {
        var error = string.Empty;

#if NET7_0_OR_GREATER
        error = Marshal.GetLastPInvokeErrorMessage();
#elif NET6_0
        error = Marshal.GetLastPInvokeError().GetSystemErrorMessage();
#else
        error = Marshal.GetLastWin32Error().GetSystemErrorMessage();
#endif

        return error;
    }

}

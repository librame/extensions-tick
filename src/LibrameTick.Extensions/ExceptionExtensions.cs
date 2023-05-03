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


    /// <summary>
    /// 写入调试异常。
    /// </summary>
    /// <param name="exception">给定的 <see cref="Exception"/>。</param>
    public static void WriteDebug(this Exception exception)
    {
        Debug.WriteLine(exception.ToString());
        Debug.Assert(false, exception.GetInnerMessage());
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
            return ExpressionExtensions.New<TOutputException>(new object[] { outputMessage });

        return ExpressionExtensions.New<TOutputException>();
    }


    /// <summary>
    /// 获取上次系统错误消息。
    /// </summary>
    /// <returns>返回错误消息字符串。</returns>
    public static string GetLastSystemErrorMessage()
    {
        string error = string.Empty;

#if NET7_0_OR_GREATER
        error = Marshal.GetLastPInvokeErrorMessage();
#elif NET6_0
        error = Marshal.GetLastPInvokeError().GetSystemErrorMessage();
#else
        error = Marshal.GetLastWin32Error().GetSystemErrorMessage();
#endif

        return error;
    }

    /// <summary>
    /// 获取系统错误消息。
    /// </summary>
    /// <param name="errorCode">给定的错误代码。</param>
    /// <returns>返回错误消息字符串。</returns>
    public static string GetSystemErrorMessage(this int errorCode)
    {
        if (errorCode > 0)
        {
            var tempPtr = IntPtr.Zero;
            string msg = string.Empty;

            Core.DllInterop.FormatMessage(0x1300, ref tempPtr, errorCode, 0, ref msg, 255, ref tempPtr);

            return msg;
        }

        return errorCode.ToString();
    }

}

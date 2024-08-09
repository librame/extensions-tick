#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure;

/// <summary>
/// 定义动态链接库互操作静态类。
/// </summary>
public static partial class DllInterop
{
    /// <summary>
    /// NT 层内核。
    /// </summary>
    public const string Ntdll = "ntdll.dll";

    /// <summary>
    /// 32 位内核。
    /// </summary>
    public const string Kernel32 = "kernel32.dll";


    /// <summary>
    /// 获取系统错误消息。
    /// </summary>
    /// <param name="errorCode">给定的错误代码。</param>
    /// <returns>返回错误消息字符串。</returns>
    public static string GetSystemErrorMessage(int errorCode)
    {
        if (errorCode > 0)
        {
            var tempPtr = IntPtr.Zero;
            var msg = string.Empty;

            FormatMessage(0x1300, ref tempPtr, errorCode, 0, ref msg, 255, ref tempPtr);

            return msg;
        }

        return errorCode.ToString();
    }


    /// <summary>
    /// 格式化错误消息。
    /// </summary>
    /// <remarks>
    /// 参考：https://learn.microsoft.com/zh-cn/windows/win32/api/winbase/nf-winbase-formatmessage
    /// </remarks>
    /// <param name="flag">给定的格式设置选项标志。</param>
    /// <param name="source">给定的消息定义源。</param>
    /// <param name="msgId">给定的消息标识符。</param>
    /// <param name="langId">给定的语言标识符。</param>
    /// <param name="msg">给定的输出消息参数。</param>
    /// <param name="size">给定的输出内容长度。</param>
    /// <param name="args">给定格式化消息中的插入值。</param>
    /// <returns>返回获取到的字符数（失败返回0）。</returns>
    [LibraryImport(Kernel32, StringMarshalling = StringMarshalling.Utf16)]
    internal static partial int FormatMessage(int flag, ref IntPtr source, int msgId, int langId, ref string msg,
        int size, ref IntPtr args);

}

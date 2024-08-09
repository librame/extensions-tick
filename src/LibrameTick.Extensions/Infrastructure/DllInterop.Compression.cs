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
    [LibraryImport(Ntdll)]
    internal static partial uint RtlGetCompressionWorkSpaceSize(ushort dCompressionFormat, out uint dNeededBufferSize, out uint dUnknown);

    [LibraryImport(Ntdll)]
    internal static partial uint RtlCompressBuffer(ushort dCompressionFormat, byte[] dSourceBuffer, int dSourceBufferLength,
        byte[] dDestinationBuffer, int dDestinationBufferLength, uint dUnknown, out int dDestinationSize, IntPtr dWorkspaceBuffer);

    [LibraryImport(Ntdll)]
    internal static partial uint RtlDecompressBuffer(ushort dCompressionFormat, byte[] dDestinationBuffer, int dDestinationBufferLength,
        byte[] dSourceBuffer, int dSourceBufferLength, out uint dDestinationSize);

    [LibraryImport(Kernel32, SetLastError = true)]
    internal static partial IntPtr LocalAlloc(int uFlags, IntPtr sizetdwBytes);

    [LibraryImport(Kernel32, SetLastError = true)]
    internal static partial IntPtr LocalFree(IntPtr hMem);
}

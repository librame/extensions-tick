#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Infrastructure.Storage;

/// <summary>
/// 定义一个表示文件大小的描述符特性。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="FileSizeDescriptionAttribute"/>。
/// </remarks>
/// <param name="description">给定的描述（如：千字节）。</param>
/// <param name="name">给定的名称（如：KibiByte/KiloByte）。</param>
/// <param name="abbrName">给定的简称（如：KiB/KB）。</param>
/// <param name="system">给定的进制（如：二进制/十进制）。</param>
/// <param name="exponent">给定的进制大小指数（用于幂运算）。</param>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
public class FileSizeDescriptionAttribute(string description,
    string name, string abbrName, FileSizeSystem system, int exponent) : Attribute
{
    /// <summary>
    /// 描述（如：字节）。
    /// </summary>
    public string Description { get; init; } = description;

    /// <summary>
    /// 名称（如：KibiByte/KiloByte）。
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// 简称（如：KiB/KB）。
    /// </summary>
    public string AbbrName { get; init; } = abbrName;

    /// <summary>
    /// 进制（如：二进制/十进制）。
    /// </summary>
    public FileSizeSystem System { get; init; } = system;

    /// <summary>
    /// 进制底数大小（用于幂运算。如：10）。
    /// </summary>
    public long BaseNumber { get; init; } = (int)system;

    /// <summary>
    /// 进制指数大小（用于幂运算。如：28）。
    /// </summary>
    public int Exponent { get; init; } = exponent;
}

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
/// 定义文件大小描述符。
/// </summary>
/// <remarks>
/// 构造一个 <see cref="FileSizeDescriptor"/>。
/// </remarks>
/// <param name="description">给定的描述。</param>
/// <param name="name">给定的名称。</param>
/// <param name="abbrName">给定的简称。</param>
/// <param name="system">给定的进制。</param>
/// <param name="size">给定的进制大小。</param>
public class FileSizeDescriptor(string description, string name,
    string abbrName, FileSizeSystem system, BigInteger size)
{
    /// <summary>
    /// 描述。
    /// </summary>
    public string Description { get; init; } = description;

    /// <summary>
    /// 名称。
    /// </summary>
    public string Name { get; init; } = name;

    /// <summary>
    /// 简称。
    /// </summary>
    public string AbbrName { get; init; } = abbrName;

    /// <summary>
    /// 进制。
    /// </summary>
    public FileSizeSystem System { get; init; } = system;

    /// <summary>
    /// 文件大小。
    /// </summary>
    public BigInteger Size { get; init; } = size;


    /// <summary>
    /// 格式化为带单位的文件大小。
    /// </summary>
    /// <param name="fileSize">给定要格式化的文件大小。</param>
    /// <returns>返回字符串。</returns>
    public virtual string FormatSize(BigInteger fileSize)
    {
        if (fileSize <= Size)
            return ToString();

        var str = string.Format(CultureInfo.CurrentCulture,
            "{0:0,0.00} " + AbbrName,
            fileSize / Size);

        // 移除可能存在的前置0
        return str.TrimStart('0');
    }


    /// <summary>
    /// 转换为字符串。
    /// </summary>
    /// <returns>返回字符串。</returns>
    public override string ToString()
        => $"{Size} {AbbrName}";


    /// <summary>
    /// 从 <see cref="FileSizeDescriptionAttribute"/> 创建文件大小描述符。
    /// </summary>
    /// <param name="attribute">给定的 <see cref="FileSizeDescriptionAttribute"/>。</param>
    /// <returns>返回 <see cref="FileSizeDescriptor"/>。</returns>
    public static FileSizeDescriptor FromDescription(FileSizeDescriptionAttribute attribute)
    {
        var size = BigInteger.Pow(attribute.BaseNumber, attribute.Exponent);

        return new FileSizeDescriptor(attribute.Description,
            attribute.Name, attribute.AbbrName, attribute.System, size);
    }

}

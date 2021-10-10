#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core;

/// <summary>
/// <see cref="FileSizeDescriptionAttribute"/> 静态扩展。
/// </summary>
public static class FileSizeDescriptionExtensions
{
    /// <summary>
    /// 文件大小单位描述字典。
    /// </summary>
    public static readonly IReadOnlyDictionary<FileSizeUnit, IEnumerable<FileSizeDescriptor>> UnitDescriptors
        = InitialUnitDescriptors();

    private static readonly IEnumerable<FileSizeDescriptor> _reverseUnitDescriptors
        = UnitDescriptors.Reverse().SelectMany(s => s.Value).ToList();


    private static Dictionary<FileSizeUnit, IEnumerable<FileSizeDescriptor>> InitialUnitDescriptors()
    {
        return EnumExtensions.GetEnumItemsWithAttributes<FileSizeUnit, int,
            FileSizeDescriptionAttribute, FileSizeDescriptor>((value, attrib) => FileSizeDescriptor.FromDescription(attrib));
    }


    /// <summary>
    /// 将文件大小格式化为带自适应单位的字符串。
    /// </summary>
    /// <param name="fileSize">给定的文件大小。</param>
    /// <param name="system">给定的 <see cref="FileSizeSystem"/>。</param>
    /// <returns>返回格式化字符串。</returns>
    public static string FormatSizeStringWithAdaptionUnit(this long fileSize, FileSizeSystem system)
        => new BigInteger(fileSize).FormatSizeStringWithAdaptionUnit(system);

    /// <summary>
    /// 将文件大小格式化为带自适应单位的字符串。
    /// </summary>
    /// <param name="fileSize">给定的文件大小。</param>
    /// <param name="system">给定的 <see cref="FileSizeSystem"/>。</param>
    /// <returns>返回格式化字符串。</returns>
    public static string FormatSizeStringWithAdaptionUnit(this BigInteger fileSize, FileSizeSystem system)
    {
        foreach (var descr in _reverseUnitDescriptors.Where(p => p.System == system))
        {
            if (fileSize >= descr.Size)
                return descr.FormatSizeString(fileSize);
        }

        return fileSize.ToString();
    }


    /// <summary>
    /// 将文件大小格式化为带单位的字符串。
    /// </summary>
    /// <param name="fileSize">给定的文件大小。</param>
    /// <param name="system">给定的 <see cref="FileSizeSystem"/>。</param>
    /// <param name="unit">给定的 <see cref="FileSizeUnit"/>。</param>
    /// <returns>返回格式化字符串。</returns>
    public static string FormatSizeStringWithUnit(this long fileSize, FileSizeSystem system, FileSizeUnit unit)
        => new BigInteger(fileSize).FormatSizeStringWithUnit(system, unit);

    /// <summary>
    /// 将文件大小格式化为带单位的字符串。
    /// </summary>
    /// <param name="fileSize">给定的文件大小。</param>
    /// <param name="system">给定的 <see cref="FileSizeSystem"/>。</param>
    /// <param name="unit">给定的 <see cref="FileSizeUnit"/>。</param>
    /// <returns>返回格式化字符串。</returns>
    public static string FormatSizeStringWithUnit(this BigInteger fileSize, FileSizeSystem system, FileSizeUnit unit)
    {
        if (UnitDescriptors.TryGetValue(unit, out var descriptions))
        {
            var descr = descriptions.FirstOrDefault(s => s.System == system);
            if (descr is null)
                throw new ArgumentException($"Unsupported file size system '{system}'.");

            return descr.FormatSizeString(fileSize);
        }

        return fileSize.ToString();
    }

}

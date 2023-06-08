#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Core;

namespace Librame.Extensions.Storage;

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
        = UnitDescriptors.Reverse().SelectMany(static s => s.Value).ToList();


    private static Dictionary<FileSizeUnit, IEnumerable<FileSizeDescriptor>> InitialUnitDescriptors()
        => EnumExtensions.GetEnumItemsWithAttributes<FileSizeUnit,
            FileSizeDescriptionAttribute, FileSizeDescriptor>(static (descr, attrib) => FileSizeDescriptor.FromDescription(attrib));


    /// <summary>
    /// 将文件大小格式化为带单位的字符串（支持自适应单位大小）。
    /// </summary>
    /// <param name="fileSize">给定的文件大小。</param>
    /// <param name="system">给定的 <see cref="FileSizeSystem"/>。</param>
    /// <param name="unit">给定的 <see cref="FileSizeUnit"/>（可选；为空表示自适应单位大小，反之则使用指定单位）。</param>
    /// <returns>返回格式化字符串。</returns>
    public static string FormatSizeWithUnit(this long fileSize, FileSizeSystem system, FileSizeUnit? unit = null)
        => new BigInteger(fileSize).FormatSizeWithUnit(system, unit);

    /// <summary>
    /// 将文件大小格式化为带单位的字符串（支持自适应单位大小）。
    /// </summary>
    /// <param name="fileSize">给定的文件大小。</param>
    /// <param name="system">给定的 <see cref="FileSizeSystem"/>。</param>
    /// <param name="unit">给定的 <see cref="FileSizeUnit"/>（可选；为空表示自适应单位大小，反之则使用指定单位）。</param>
    /// <returns>返回格式化字符串。</returns>
    public static string FormatSizeWithUnit(this BigInteger fileSize, FileSizeSystem system, FileSizeUnit? unit = null)
    {
        if (unit is not null)
        {
            // 使用指定单位格式化
            if (UnitDescriptors.TryGetValue(unit.Value, out var descriptions))
            {
                var descr = descriptions.FirstOrDefault(s => s.System == system);
                if (descr is null)
                    throw new ArgumentException($"Unsupported file size system '{system}'.");

                return descr.FormatSize(fileSize);
            }

            return fileSize.ToString();
        }

        // 自适应大小单位格式化
        foreach (var descr in _reverseUnitDescriptors.Where(p => p.System == system))
        {
            if (fileSize >= descr.Size)
                return descr.FormatSize(fileSize);
        }

        return fileSize.ToString();
    }

}

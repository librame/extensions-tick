#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Numerics;
using System.Reflection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// <see cref="FileSizeDescriptionAttribute"/> 静态扩展。
    /// </summary>
    public static class FileSizeDescriptionExtensions
    {
        private static readonly Type _attributeType = typeof(FileSizeDescriptionAttribute);

        /// <summary>
        /// 文件大小单位描述字典。
        /// </summary>
        public static readonly IReadOnlyDictionary<string, List<FileSizeDescriptor>> UnitDescriptors
            = InitialUnitDescriptors();

        private static readonly IEnumerable<FileSizeDescriptor> _reverseUnitDescriptors
            = UnitDescriptors.Reverse().SelectMany(s => s.Value).ToList();


        private static Dictionary<string, List<FileSizeDescriptor>> InitialUnitDescriptors()
        {
            var units = new Dictionary<string, List<FileSizeDescriptor>>();

            var fields = typeof(FileSizeUnit).GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attributes = field.GetCustomAttributes(_attributeType, false)
                    .Select(obj => FileSizeDescriptor.FromDescription((FileSizeDescriptionAttribute)obj))
                    .ToList();

                units.Add(field.Name, attributes);
            }

            return units;
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
            var unitName = Enum.GetName(typeof(FileSizeUnit), unit);

            if (UnitDescriptors.TryGetValue(unitName!, out var descriptions))
            {
                var descr = descriptions.FirstOrDefault(s => s.System == system);
                if (descr == null)
                    throw new ArgumentException($"Unsupported file size system '{system}'.");

                return descr.FormatSizeString(fileSize);
            }

            return fileSize.ToString();
        }

    }
}

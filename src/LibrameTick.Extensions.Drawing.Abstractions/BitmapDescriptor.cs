#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 定义位图描述符。
    /// </summary>
    public record BitmapDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="BitmapDescriptor"/>。
        /// </summary>
        /// <param name="source">给定的位图源。</param>
        /// <param name="filePath">给定的位图文件路径（可选）。</param>
        /// <param name="saveAsSuffix">给定的位图文件另存为后缀（可选）。</param>
        public BitmapDescriptor(object source, string? filePath = null, string? saveAsSuffix = null)
        {
            Source = source;
            FilePath = filePath;
            SaveAsSuffix = saveAsSuffix;
        }


        /// <summary>
        /// 位图源。
        /// </summary>
        public object Source { get; init; }

        /// <summary>
        /// 位图文件路径。
        /// </summary>
        public string? FilePath { get; init; }

        /// <summary>
        /// 位图文件另存为后缀。
        /// </summary>
        public string? SaveAsSuffix { get; private set; }

        /// <summary>
        /// 位图文件另存为路径。
        /// </summary>
        public string? SaveAsPath { get; set; }

        /// <summary>
        /// 是否来自位图文件。
        /// </summary>
        public bool FromFile
            => !string.IsNullOrEmpty(FilePath);


        /// <summary>
        /// 附加位图文件另存为后缀（同时更新当前另存为后缀）。
        /// </summary>
        /// <param name="suffix">给定要附加的后缀。</param>
        /// <returns>返回附加后的字符串。</returns>
        public string AppendSaveAsSuffix(string suffix)
            => SaveAsSuffix += suffix;

        /// <summary>
        /// 尝试添加位图文件另存为后缀（如果当前另存为后缀为空或空字符串，则进行添加，反之则直接返回）。
        /// </summary>
        /// <param name="suffix">给定的后缀。</param>
        /// <returns>返回是否添加的布尔值。</returns>
        public bool TryAddSaveAsSuffix(string suffix)
        {
            if (string.IsNullOrEmpty(SaveAsSuffix))
            {
                SaveAsSuffix = suffix;
                return true;
            }

            return false;
        }

    }
}

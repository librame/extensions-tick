#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Drawing;

namespace Librame.Extensions.Drawing.Processing
{
    /// <summary>
    /// 定义表示图像缩放的描述符。
    /// </summary>
    public record ScaleDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="ScaleDescriptor"/>。
        /// </summary>
        /// <param name="fileNameSuffix">给定的文件命名后缀。</param>
        /// <param name="maxSize">给定的最大尺寸。</param>
        /// <param name="addWatermark">是否添加水印。</param>
        public ScaleDescriptor(string fileNameSuffix, Size maxSize,
            bool addWatermark)
        {
            FileNameSuffix = fileNameSuffix;
            MaxSize = maxSize;
            AddWatermark = addWatermark;
        }


        /// <summary>
        /// 文件命名后缀。
        /// </summary>
        public string FileNameSuffix { get; init; }

        /// <summary>
        /// 最大尺寸。
        /// </summary>
        public Size MaxSize { get; init; }

        /// <summary>
        /// 是否添加水印。
        /// </summary>
        public bool AddWatermark { get; init; }
    }
}

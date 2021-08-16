#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using SkiaSharp;

namespace Librame.Extensions.Drawing.Processing
{
    /// <summary>
    /// 定义实现 <see cref="ICollection{SKBitmap}"/> 的位图列表接口。
    /// </summary>
    public interface IBitmapList : ICollection<SKBitmap>, IDisposable
    {
        /// <summary>
        /// 添加位图。
        /// </summary>
        /// <param name="imageBuffer">给定的位图字节数组。</param>
        void Add(byte[] imageBuffer);

        /// <summary>
        /// 添加位图集合。
        /// </summary>
        /// <param name="imageBuffers">给定的位图字节数组集合。</param>
        void Add(IEnumerable<byte[]> imageBuffers);


        /// <summary>
        /// 添加位图。
        /// </summary>
        /// <param name="imagePath">给定的位图路径。</param>
        void Add(string imagePath);

        /// <summary>
        /// 添加位图集合。
        /// </summary>
        /// <param name="imagePaths">给定的位图路径集合。</param>
        void Add(IEnumerable<string> imagePaths);
    }
}

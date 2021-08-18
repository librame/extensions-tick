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
    /// <see cref="IBitmapList"/> 静态扩展。
    /// </summary>
    public static class BitmapListExtensions
    {

        /// <summary>
        /// 将图像路径转为位图列表。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        /// <returns>返回 <see cref="IBitmapList"/>。</returns>
        public static IBitmapList AsBitmaps(this string imagePath)
        {
            var bitmaps = new BitmapList();
            bitmaps.Add(imagePath);

            return bitmaps;
        }

        /// <summary>
        /// 将图像路径集合转为位图列表。
        /// </summary>
        /// <param name="imagePaths">给定的 <see cref="IEnumerable{String}"/>。</param>
        /// <returns>返回 <see cref="IBitmapList"/>。</returns>
        public static IBitmapList AsBitmaps(this IEnumerable<string> imagePaths)
        {
            var bitmaps = new BitmapList();
            bitmaps.Add(imagePaths);

            return bitmaps;
        }

    }
}

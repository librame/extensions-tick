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
using SkiaSharp;
using System.Collections;

namespace Librame.Extensions.Drawing.Processing
{
    /// <summary>
    /// 定义实现 <see cref="IBitmapList"/> 的位图列表。
    /// </summary>
    public class BitmapList : AbstractDisposable, IBitmapList
    {
        private readonly List<SKBitmap> _bitmaps;


        /// <summary>
        /// 构造一个 <see cref="BitmapList"/>。
        /// </summary>
        /// <param name="bitmaps">给定的 <see cref="List{SKBitmap}"/>（可选）。</param>
        public BitmapList(List<SKBitmap>? bitmaps = null)
        {
            _bitmaps = bitmaps ?? new List<SKBitmap>();
        }

        /// <summary>
        /// 构造一个 <see cref="BitmapList"/>。
        /// </summary>
        /// <param name="bitmaps">给定的 <see cref="IEnumerable{SKBitmap}"/>。</param>
        public BitmapList(IEnumerable<SKBitmap> bitmaps)
            : this(new List<SKBitmap>(bitmaps))
        {
        }


        /// <summary>
        /// 位图数。
        /// </summary>
        public int Count
            => _bitmaps.Count;

        /// <summary>
        /// 是否只读。
        /// </summary>
        public bool IsReadOnly
            => ((IList<SKBitmap>)_bitmaps).IsReadOnly;


        /// <summary>
        /// 添加位图。
        /// </summary>
        /// <param name="imageBuffer">给定的字节数组。</param>
        public void Add(byte[] imageBuffer)
            => _bitmaps.Add(SKBitmap.Decode(imageBuffer));

        /// <summary>
        /// 添加位图集合。
        /// </summary>
        /// <param name="imageBuffers">给定的字节数组集合。</param>
        public void Add(IEnumerable<byte[]> imageBuffers)
            => imageBuffers.ForEach(buffer => SKBitmap.Decode(buffer));

        /// <summary>
        /// 添加位图。
        /// </summary>
        /// <param name="imagePath">给定的图像路径。</param>
        public void Add(string imagePath)
            => _bitmaps.Add(SKBitmap.Decode(imagePath));

        /// <summary>
        /// 添加位图集合。
        /// </summary>
        /// <param name="imagePaths">给定的图像路径集合。</param>
        public void Add(IEnumerable<string> imagePaths)
            => imagePaths.ForEach(path => SKBitmap.Decode(path));

        /// <summary>
        /// 添加位图。
        /// </summary>
        /// <param name="item">给定的 <see cref="SKBitmap"/>。</param>
        public void Add(SKBitmap item)
            => _bitmaps.Add(item);


        /// <summary>
        /// 复制到位置数组。
        /// </summary>
        /// <param name="array">给定要复制到的 <see cref="SKBitmap"/> 数组。</param>
        /// <param name="arrayIndex">给定要复制的数组索引。</param>
        public void CopyTo(SKBitmap[] array, int arrayIndex)
            => _bitmaps.CopyTo(array, arrayIndex);


        /// <summary>
        /// 获取位图枚举器。
        /// </summary>
        /// <returns>返回 <see cref="IEnumerator{SKBitmap}"/>。</returns>
        public IEnumerator<SKBitmap> GetEnumerator()
            => _bitmaps.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();


        /// <summary>
        /// 不可用，始终抛出未实现异常。
        /// </summary>
        /// <param name="item">给定的 <see cref="SKBitmap"/>。</param>
        /// <returns>始终抛出未实现异常。</returns>
        public bool Contains(SKBitmap item)
            => throw new NotImplementedException();

        /// <summary>
        /// 不可用，始终抛出未实现异常。
        /// </summary>
        /// <param name="item">给定的 <see cref="SKBitmap"/>。</param>
        /// <returns>始终抛出未实现异常。</returns>
        public bool Remove(SKBitmap item)
            => throw new NotImplementedException();


        /// <summary>
        /// 清除当前位图集合。
        /// </summary>
        public void Clear()
        {
            if (_bitmaps.Count > 0)
            {
                _bitmaps.ForEach(b => b.Dispose());
                _bitmaps.Clear();
            }
        }


        /// <summary>
        /// 释放已托管资源。
        /// </summary>
        /// <returns>返回是否成功释放的布尔值。</returns>
        protected override bool ReleaseManaged()
        {
            if (_bitmaps.Count > 0)
                _bitmaps.ForEach(b => b.Dispose());

            return true;
        }

    }
}

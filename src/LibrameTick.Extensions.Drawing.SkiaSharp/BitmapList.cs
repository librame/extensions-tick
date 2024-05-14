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
using Librame.Extensions.Infrastructure;
using Librame.Extensions.Infrastructure.Core;

namespace Librame.Extensions.Drawing;

/// <summary>
/// 定义实现 <see cref="IBitmapList"/> 的位图列表。
/// </summary>
public class BitmapList : AbstractDisposable, IBitmapList
{
    private readonly List<BitmapDescriptor> _bitmaps;


    /// <summary>
    /// 构造一个 <see cref="BitmapList"/>。
    /// </summary>
    public BitmapList()
    {
        _bitmaps = [];
    }

    /// <summary>
    /// 使用 <see cref="SKBitmap"/> 集合构造一个 <see cref="BitmapList"/>。
    /// </summary>
    /// <param name="bitmaps">给定的 <see cref="IEnumerable{SKBitmap}"/>。</param>
    public BitmapList(IEnumerable<SKBitmap> bitmaps)
    {
        _bitmaps = bitmaps.Select(static s => new BitmapDescriptor(s)).ToList();
    }

    /// <summary>
    /// 使用 <see cref="BitmapDescriptor"/> 集合构造一个 <see cref="BitmapList"/>。
    /// </summary>
    /// <param name="bitmaps">给定的 <see cref="List{BitmapDescriptor}"/>。</param>
    public BitmapList(List<BitmapDescriptor> bitmaps)
    {
        _bitmaps = bitmaps;
    }


    /// <summary>
    /// 位图数。
    /// </summary>
    public int Count
        => _bitmaps.Count;


    /// <summary>
    /// 添加位图。
    /// </summary>
    /// <param name="imageBuffer">给定的字节数组。</param>
    public void Add(byte[] imageBuffer)
    {
        var descr = new BitmapDescriptor(SKBitmap.Decode(imageBuffer));
        _bitmaps.Add(descr);
    }

    /// <summary>
    /// 添加位图集合。
    /// </summary>
    /// <param name="imageBuffers">给定的字节数组集合。</param>
    public void Add(IEnumerable<byte[]> imageBuffers)
        => imageBuffers.ForEach(Add);

    /// <summary>
    /// 添加位图。
    /// </summary>
    /// <param name="imagePath">给定的图像路径。</param>
    public void Add(string imagePath)
    {
        var descr = new BitmapDescriptor(SKBitmap.Decode(imagePath), imagePath);
        _bitmaps.Add(descr);
    }

    /// <summary>
    /// 添加位图集合。
    /// </summary>
    /// <param name="imagePaths">给定的图像路径集合。</param>
    public void Add(IEnumerable<string> imagePaths)
        => imagePaths.ForEach(Add);

    /// <summary>
    /// 添加位图描述符。
    /// </summary>
    /// <param name="descriptor">给定的 <see cref="BitmapDescriptor"/>。</param>
    public void Add(BitmapDescriptor descriptor)
        => _bitmaps.Add(descriptor);

    /// <summary>
    /// 添加位图描述符集合。
    /// </summary>
    /// <param name="descriptors">给定的 <see cref="IEnumerable{BitmapDescriptor}"/>。</param>
    public void Add(IEnumerable<BitmapDescriptor> descriptors)
        => _bitmaps.AddRange(descriptors);


    /// <summary>
    /// 清除当前位图集合。
    /// </summary>
    public void Clear()
    {
        if (_bitmaps.Count > 0)
        {
            _bitmaps.ForEach(static b => ((SKBitmap)b.Source).Dispose());
            _bitmaps.Clear();
        }
    }


    /// <summary>
    /// 获取位图枚举器。
    /// </summary>
    /// <returns>返回位图对象枚举器。</returns>
    public IEnumerator<BitmapDescriptor> GetEnumerator()
        => _bitmaps.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();


    /// <summary>
    /// 释放已托管资源。
    /// </summary>
    /// <returns>返回是否成功释放的布尔值。</returns>
    protected override bool ReleaseManaged()
    {
        Clear();
        return true;
    }

}

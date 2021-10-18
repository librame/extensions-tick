#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Drawing.Drawers;

class InternalScaleDrawer : AbstractDrawer, IScalingDrawer
{
    private IWatermarkDrawer _watermarkDrawer;


    public InternalScaleDrawer(IOptionsMonitor<DrawingExtensionOptions> options,
        IWatermarkDrawer watermarkDrawer)
        : base(options.CurrentValue)
    {
        _watermarkDrawer = watermarkDrawer;
    }


    protected override bool ReleaseManaged()
    {
        _watermarkDrawer.Dispose();
        return true;
    }


    protected override IBitmapList DrawCore(IBitmapList bitmaps)
    {
        if (Options.Scale.Descriptors.Count < 1)
            return bitmaps;

        var scaleBitmaps = new BitmapList();
        var watermarkBitmaps = new BitmapList();

        foreach (var bitmap in bitmaps)
        {
            var realBitmap = (SKBitmap)bitmap.Source;
            // 遍历缩放信息
            foreach (var scaleDescriptor in Options.Scale.Descriptors)
            {
                // 如果源图尺寸小于缩放尺寸，则跳过当前缩放信息
                if (realBitmap.Width <= scaleDescriptor.MaxSize.Width
                    && realBitmap.Height <= scaleDescriptor.MaxSize.Height)
                {
                    continue;
                }

                // 计算等比例缩放尺寸
                var imageSize = new Size(realBitmap.Width, realBitmap.Height);
                var scaleSize = ImageHelper.ScaleSize(imageSize, scaleDescriptor.MaxSize);
                var scaleInfo = new SKImageInfo(scaleSize.Width, scaleSize.Height,
                    realBitmap.Info.ColorType, realBitmap.Info.AlphaType);

                // 是否附加缩放尺寸后缀
                var scaleNameFuffix = scaleDescriptor.FileNameSuffix;
                if (Options.Scale.AddScaleSizeSuffix)
                    scaleNameFuffix += $"-{scaleSize.Width}x{scaleSize.Height}";

                // 创建缩放位图描述符
                var scaleBitmap = realBitmap.Resize(scaleInfo, Options.ResizeQuality);
                var descriptor = new BitmapDescriptor(scaleBitmap, bitmap.FilePath,
                    scaleNameFuffix);

                scaleBitmaps.Add(descriptor);

                if (scaleDescriptor.AddWatermark)
                    watermarkBitmaps.Add(descriptor);
            }
        }

        // 处理缩放位图集合的水印
        if (watermarkBitmaps.Count > 0)
            _watermarkDrawer.Draw(watermarkBitmaps);

        return scaleBitmaps;
    }

}

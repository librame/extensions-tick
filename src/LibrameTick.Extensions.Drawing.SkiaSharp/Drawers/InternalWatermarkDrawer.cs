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

class InternalWatermarkDrawer : AbstractDrawer, IWatermarkDrawer
{
    private SKPaint _forePaint;
    private SKPaint _alternPaint;
    private SKBitmap? _watermarkBitmap;


    public InternalWatermarkDrawer(IOptionsMonitor<DrawingExtensionOptions> options)
        : base(options.CurrentValue)
    {
        _forePaint = Options.Watermark.Font.CreatePaint(Options.Colors.Fore);
        _alternPaint = Options.Watermark.Font.CreatePaint(Options.Colors.Alternate);

        // 限定水印图像文件的存储在模块的资源目录中
        var watermarkFile = Options.Watermark.ImageFile
            .SetBasePath(Options.Directories.ResourceDirectory);

        if (watermarkFile.FileExists())
            _watermarkBitmap = SKBitmap.Decode(watermarkFile);
    }


    protected override bool ReleaseManaged()
    {
        _forePaint.Dispose();
        _alternPaint.Dispose();

        if (_watermarkBitmap is not null)
            _watermarkBitmap.Dispose();

        return true;
    }


    protected override IBitmapList DrawCore(IBitmapList bitmaps)
    {
        if (Options.Watermark.Mode is WatermarkMode.Image && _watermarkBitmap is not null)
        {
            DrawImageWatermark(bitmaps);
        }
        else
        {
            DrawTextWatermark(bitmaps);
        }

        return bitmaps;
    }

    private void DrawImageWatermark(IBitmapList bitmaps)
    {
        foreach (var bitmap in bitmaps)
        {
            bitmap.AppendSaveAsSuffix(Options.Watermark.FileNameSuffix);

            var realBitmap = (SKBitmap)bitmap.Source;
            using (var canvas = new SKCanvas(realBitmap))
            {
                var imageSize = new Size(realBitmap.Width, realBitmap.Height);
                var coord = ImageHelper.CalculateCoord(imageSize,
                    Options.Watermark.InitialCoord, Options.Watermark.IsRandomCoord);

                // 绘制图像水印
                canvas.DrawBitmap(_watermarkBitmap, coord.X, coord.Y);
            }
        }
    }

    private void DrawTextWatermark(IBitmapList bitmaps)
    {
        if (string.IsNullOrEmpty(Options.Watermark.Text))
            throw new ArgumentException("The text watermark cannot be empty.");

        var count = 1;
        foreach (var bitmap in bitmaps)
        {
            bitmap.AppendSaveAsSuffix(Options.Watermark.FileNameSuffix);

            var realBitmap = (SKBitmap)bitmap.Source;
            using (var canvas = new SKCanvas(realBitmap))
            {
                var imageSize = new Size(realBitmap.Width, realBitmap.Height);
                var coord = ImageHelper.CalculateCoord(imageSize,
                    Options.Watermark.InitialCoord, Options.Watermark.IsRandomCoord);

                canvas.DrawText(Options.Watermark.Text, coord.X, coord.Y,
                    count.IsMultiples(2) ? _alternPaint : _forePaint);

                count++;
            }
        }
    }

}

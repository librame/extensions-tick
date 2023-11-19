#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions.Bootstraps;
using Librame.Extensions.Core;

namespace Librame.Extensions.Drawing.Drawers;

internal sealed class InternalSavingDrawer : AbstractInternalDrawer, ISavingDrawer
{
    private readonly IOptionsMonitor<CoreExtensionOptions> _coreOptionsMonitor;


    public InternalSavingDrawer(
        IOptionsMonitor<DrawingExtensionOptions> drawingOptionsMonitor,
        IOptionsMonitor<CoreExtensionOptions> coreOptionsMonitor)
        : base(drawingOptionsMonitor)
    {
        _coreOptionsMonitor = coreOptionsMonitor;
    }


    public IClockBootstrap Clock
        => _coreOptionsMonitor.CurrentValue.Clock;

    public SKEncodedImageFormat ImageFormat
        => Options.ImageFormat.AsEncodedImageFormat();


    public Func<IClockBootstrap, string> SaveSubpathFunc { get; set; }
        = clock =>
        {
            var now = clock.GetNow();
            var folderNames = new string[] { now.ToString("yyMM"), now.ToString("dd") };
            return folderNames.CombineRelativeSubpath();
        };

    public Func<IClockBootstrap, BitmapDescriptor, string> SaveFileBaseNameFunc { get; set; }
        = (clock, descr) =>
        {
            var fileBaseName = string.Empty;
            if (descr.FromFile)
                fileBaseName = Path.GetFileNameWithoutExtension(descr.FilePath);

            return string.IsNullOrEmpty(fileBaseName)
                ? clock.GetNow().Ticks.ToString()
                : fileBaseName;
        };


    protected override bool ReleaseManaged()
        => true;


    protected override IBitmapList DrawCore(IBitmapList bitmaps)
    {
        var baseDirectory = SaveSubpathFunc(Clock).SetBasePath(Options.ImageDirectory);
        baseDirectory.CreateDirectory();

        foreach (var bitmap in bitmaps)
        {
            var realBitmap = (SKBitmap)bitmap.Source;
            using (var image = SKImage.FromBitmap(realBitmap))
            using (var data = image.Encode(ImageFormat, Options.EncodeQuality))
            {
                // 文件基础名+另存为后缀+文件格式名，如：6123xxxx-suffix.jpeg
                var fileName = SaveFileBaseNameFunc(Clock, bitmap)
                    + bitmap.SaveAsSuffix + Options.ImageFormat.Leading('.');

                bitmap.SaveAsPath = baseDirectory.CombinePath(fileName);

                using (var fs = new FileStream(bitmap.SaveAsPath, FileMode.OpenOrCreate))
                {
                    data.SaveTo(fs);
                }
            }
        }

        return bitmaps;
    }

}

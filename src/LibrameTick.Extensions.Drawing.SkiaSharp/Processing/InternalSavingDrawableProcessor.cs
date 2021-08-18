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

namespace Librame.Extensions.Drawing.Processing
{
    class InternalSavingDrawableProcessor : AbstractDrawableProcessor, ISavingDrawableProcessor
    {
        private readonly IClock _clock;
        private readonly SKEncodedImageFormat _imageFormat;


        public InternalSavingDrawableProcessor(DrawingExtensionOptions options)
            : base(options)
        {
            _clock = options.CoreOptions.Clock;
            _imageFormat = options.ImageFormat.AsEncodedImageFormat();
        }


        public Func<IClock, string> SaveSubpathFunc { get; set; }
            = clock =>
            {
                var now = clock.GetNow();
                var folderNames = new string[] { now.ToString("yyMM"), now.ToString("dd") };
                return folderNames.CombineRelativeSubpath();
            };

        public Func<IClock, BitmapDescriptor, string> SaveFileBaseNameFunc { get; set; }
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


        protected override IBitmapList ProcessCore(IBitmapList bitmaps)
        {
            var baseDirectory = SaveSubpathFunc.Invoke(_clock).SetBasePath(Options.ImageDirectory);
            baseDirectory.CreateDirectory();

            foreach (var bitmap in bitmaps)
            {
                var realBitmap = (SKBitmap)bitmap.Source;
                using (var image = SKImage.FromBitmap(realBitmap))
                using (var data = image.Encode(_imageFormat, Options.EncodeQuality))
                {
                    // 文件基础名+另存为后缀+文件格式名，如：6123xxxx-suffix.jpeg
                    var fileName = SaveFileBaseNameFunc.Invoke(_clock, bitmap)
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
}

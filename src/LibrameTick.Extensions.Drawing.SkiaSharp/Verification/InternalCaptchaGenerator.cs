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

namespace Librame.Extensions.Drawing.Verification;

class InternalCaptchaGenerator : AbstractDisposable, ICaptchaGenerator
{
    private readonly DrawingExtensionOptions _options;
    private readonly SKEncodedImageFormat _imageFormat;
    private readonly SKColor _backgroundColor;

    private SKPaint _forePaint;
    private SKPaint _alternPaint;
    private SKPaint _noisePaint;


    public InternalCaptchaGenerator(IOptionsMonitor<DrawingExtensionOptions> options)
    {
        _options = options.CurrentValue;
        _imageFormat = _options.ImageFormat.AsEncodedImageFormat();
        _backgroundColor = _options.Colors.Background.AsColor();

        _forePaint = _options.Watermark.Font.CreatePaint(_options.Colors.Fore);
        _alternPaint = _options.Watermark.Font.CreatePaint(_options.Colors.Alternate);
        _noisePaint = _options.Captcha.BackgroundNoise.CreatePaint(_options.Colors.Interference);
    }


    protected override bool ReleaseManaged()
    {
        _forePaint.Dispose();
        _alternPaint.Dispose();
        _noisePaint.Dispose();

        return true;
    }


    public bool DrawFile(string captcha, string savePath)
    {
        DrawCore(captcha, data =>
        {
            using (var fs = new FileStream(savePath, FileMode.OpenOrCreate))
            {
                data.SaveTo(fs);
            }
        });

        return File.Exists(savePath);
    }

    public bool DrawStream(string captcha, Stream target)
    {
        DrawCore(captcha, data =>
        {
            data.SaveTo(target);
        });

        return true;
    }

    public byte[] DrawBytes(string captcha)
    {
        var buffer = Array.Empty<byte>();

        DrawCore(captcha, data =>
        {
            buffer = data.ToArray();
        });

        return buffer;
    }

    private void DrawCore(string captcha, Action<SKData> postAction)
    {
        var sizeAndPoints = ComputeSizeAndPoints(captcha);
        var imageSize = sizeAndPoints.Size;
        var imageInfo = new SKImageInfo(imageSize.Width, imageSize.Height,
            SKColorType.Bgra8888, SKAlphaType.Premul);

        using (var bmp = new SKBitmap(imageInfo))
        using (var canvas = new SKCanvas(bmp))
        {
            // Clear
            canvas.DrawColor(_backgroundColor);

            // 绘制噪点
            var points = _options.Captcha.BackgroundNoise.CreatePoints(imageSize);
            canvas.DrawPoints(SKPointMode.Points, points, _noisePaint);

            // 绘制验证码
            foreach (var p in sizeAndPoints.Points)
            {
                var i = p.Key;
                var character = p.Value.Key;
                var point = p.Value.Value;

                canvas.DrawText(character, point.X, point.Y,
                    i % 2 > 0 ? _alternPaint : _forePaint);
            }

            using (var img = SKImage.FromBitmap(bmp))
            using (var data = img.Encode(_imageFormat, _options.EncodeQuality))
            {
                postAction(data);
            }
        }
    }


    private (Size Size, IDictionary<int, KeyValuePair<string, SKPoint>> Points) ComputeSizeAndPoints(string captcha)
    {
        var points = new Dictionary<int, KeyValuePair<string, SKPoint>>();
        var size = new Size();

        var paddingHeight = (int)_forePaint.TextSize;
        var paddingWidth = paddingHeight / 2;

        var startX = paddingWidth;
        var startY = paddingHeight;

        for (int i = 0; i < captcha.Length; i++)
        {
            // 当前字符
            var character = captcha.Substring(i, 1);

            // 测算字符矩形
            var rect = new SKRect();
            _forePaint.MeasureText(character, ref rect);

            // 当前字符宽高
            var charWidth = (int)rect.Width;
            var charHeight = (int)rect.Height;

            var point = new SKPoint();

            // 随机变换其余字符坐标
            RandomExtensions.Run(r =>
            {
                point.X = r.Next(startX, charWidth + startX);
                point.Y = r.Next(startY, charHeight + startY);
            });

            // 附加为字符宽度加当前字符横坐标
            startX = (int)point.X + charWidth + paddingWidth;

            points.Add(i, new KeyValuePair<string, SKPoint>(character, point));
        }

        size.Width += startX + paddingWidth;
        size.Height += startY + paddingHeight;

        return (size, points);
    }

}

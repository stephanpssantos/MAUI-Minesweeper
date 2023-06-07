using Microsoft.Graphics.Canvas;
using Windows.Storage.Streams;
using IImage = Microsoft.Maui.Graphics.IImage;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.UI.Xaml.Controls;
using System.Reflection;

namespace MauiTest1.Helpers
{
    public class WinIImage : IImage
    {
        private readonly ICanvasResourceCreator _creator;
        private CanvasBitmap _bitmap;

        public WinIImage(ICanvasResourceCreator creator, CanvasBitmap bitmap)
        {
            _creator = creator;
            _bitmap = bitmap;
        }

        public CanvasBitmap PlatformImage => _bitmap;

        public void Dispose()
        {
            var bitmap = Interlocked.Exchange(ref _bitmap, null);
            bitmap?.Dispose();
        }

        public IImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            if (Width > maxWidthOrHeight || Height > maxWidthOrHeight)
            {
                using (var memoryStream = new InMemoryRandomAccessStream())
                {
                    Save(memoryStream.AsStreamForWrite());
                    memoryStream.Seek(0);

                    // ReSharper disable once AccessToDisposedClosure
                    var newBitmap = AsyncPump.Run(async () => await CanvasBitmap.LoadAsync(_creator, memoryStream, 96));
                    using (var memoryStream2 = new InMemoryRandomAccessStream())
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        AsyncPump.Run(async () => await newBitmap.SaveAsync(memoryStream2, CanvasBitmapFileFormat.Png));

                        memoryStream2.Seek(0);
                        var newImage = FromStream(memoryStream2.AsStreamForRead());
                        if (disposeOriginal)
                            _bitmap.Dispose();

                        return newImage;
                    }
                }
            }

            return this;
        }

        public IImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public IImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit,
            bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public float Width => (float)_bitmap.Size.Width;

        public float Height => (float)_bitmap.Size.Height;

        public void Save(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1)
        {
            switch (format)
            {
                case ImageFormat.Jpeg:
                    AsyncPump.Run(async () => await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Jpeg, quality));
                    break;
                default:
                    AsyncPump.Run(async () => await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Png));
                    break;
            }
        }

        public async Task SaveAsync(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1)
        {
            switch (format)
            {
                case ImageFormat.Jpeg:
                    await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Jpeg, quality);
                    break;
                default:
                    await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Png);
                    break;
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.DrawImage(this, dirtyRect.Left, dirtyRect.Top, Math.Abs(dirtyRect.Width), Math.Abs(dirtyRect.Height));
        }

        public IImage ToPlatformImage()
        {
            throw new NotImplementedException();
        }

        public IImage ToImage(int width, int height, float scale = 1f)
        {
            throw new NotImplementedException();
        }

        public static IImage FromStream(Stream stream, ImageFormat format = ImageFormat.Png)
        {
            throw new NotImplementedException();

            //var _canvas = Canvas as W2DCanvas;
            //var canvasCreator = _canvas.Session.Device;

            //Assembly assembly = GetType().GetTypeInfo().Assembly;
            //using (Stream stream = assembly.GetManifestResourceStream("MauiTest1.Resources.Images.question_emb.png"))
            //{
            //    var bitmap = await CanvasBitmap.LoadAsync(canvasCreator, stream.AsRandomAccessStream());
            //    questionMarkImage = new WinIImage(canvasCreator, bitmap);
            //}
        }
    }
}

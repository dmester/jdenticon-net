using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GdiPixelFormat = System.Drawing.Imaging.PixelFormat;
using GdiPoint = System.Drawing.Point;
using WpfSize = System.Windows.Size;
using WpfPoint = System.Windows.Point;

namespace Jdenticon.Tests.EndToEnd.Helpers
{
    public class DesktopTestBed : TestBed
    {
        public DesktopTestBed(Type testType, TestContext context) : base(testType, context)
        {
        }

        public void Capture(FrameworkElement element, Stream outputStream)
        {
            var size = new WpfSize(element.ActualWidth, element.ActualHeight);

            var target = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();
            using (var context = drawingVisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(element), null, new Rect(new WpfPoint(), size));
            }

            target.Render(drawingVisual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(target));

            encoder.Save(outputStream);
        }

        public void Capture(Control control, Stream outputStream)
        {
            var size = control.Size;

            using (var bitmap = new Bitmap(size.Width, size.Height))
            {
                control.DrawToBitmap(bitmap, new Rectangle(GdiPoint.Empty, size));
                bitmap.Save(outputStream, ImageFormat.Png);
            }

            /*
            var rcClient = new NativeMethods.RECT();
            NativeMethods.GetClientRect(hWnd, rcClient);

            var bmp = new Bitmap(rcClient.Right, rcClient.Bottom, GdiPixelFormat.Format32bppArgb);

            using (var g = Graphics.FromImage(bmp))
            {
                var hdc = g.GetHdc();
                NativeMethods.PrintWindow(hWnd, hdc, NativeMethods.PW_CLIENTONLY);
            }

            bmp.Save(outputStream, ImageFormat.Png);
            */
        }

        public void AssertEqualWindow(Window window, string fileName)
        {
            using (var stream = new MemoryStream())
            {
                Capture((FrameworkElement)window.Content, stream);
                stream.Position = 0;
                AssertEqual(stream, fileName);
            }
        }

        public void AssertEqualWindow(Control control, string fileName)
        {
            using (var stream = new MemoryStream())
            {
                Capture(control, stream);
                stream.Position = 0;
                AssertEqual(stream, fileName);
            }
        }

        public override void Dispose()
        {
        }
    }
}

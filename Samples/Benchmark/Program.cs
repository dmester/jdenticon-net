using Jdenticon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var icon = Identicon.FromValue("df");
            icon.Style.BackColor = Jdenticon.Rendering.Color.Transparent;
            const int size = 160;

            icon.SaveAsPng("M:\\test1509-own.png", size);

            using (var bmp = icon.ToBitmap(size))
            {
                bmp.Save("M:\\test1509-gdi.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            //return;

            Benchmarker.Run("Own", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.SaveAsPng(stream, size);
                }
            }, 200);
            Benchmarker.Run("Gdi", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.ToBitmap(size).Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
            }, 200);

            Benchmarker.Run("Own", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.SaveAsPng(stream, size);
                }
            }, 200);
            Benchmarker.Run("Gdi", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.ToBitmap(size).Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
            }, 200);
        }
    }
}

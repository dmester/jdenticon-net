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
            var icon = Identicon.FromValue("df", 160);
            icon.Style.BackColor = Jdenticon.Rendering.Color.Transparent;
            
            icon.SaveAsPng("M:\\test1509-own.png");

            using (var bmp = icon.ToBitmap())
            {
                bmp.Save("M:\\test1509-gdi.png", System.Drawing.Imaging.ImageFormat.Png);
            }

            //return;

            Benchmarker.Run("Own", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.SaveAsPng(stream);
                }
            }, 200);
            Benchmarker.Run("Gdi", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.ToBitmap().Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
            }, 200);

            Benchmarker.Run("Own", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.SaveAsPng(stream);
                }
            }, 200);
            Benchmarker.Run("Gdi", () =>
            {
                using (var stream = new MemoryStream())
                {
                    icon.ToBitmap().Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                }
            }, 200);
        }
    }
}

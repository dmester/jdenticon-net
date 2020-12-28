#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the 
// "Software"), to deal in the Software without restriction, including 
// without limitation the rights to use, copy, modify, merge, publish, 
// distribute, sublicense, and/or sell copies of the Software, and to 
// permit persons to whom the Software is furnished to do so, subject to 
// the following conditions:
// 
// The above copyright notice and this permission notice shall be 
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion

using Jdenticon.Gdi.Extensions;
using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

#if SUPPORT_ASYNC_AWAIT
using System.Threading.Tasks;
#endif

namespace Jdenticon
{
    /// <summary>
    /// Extends <see cref="Identicon"/> with GDI specific methods.
    /// </summary>
    public static class GdiExtensions
    {
        /// <summary>
        /// Draws an <see cref="Identicon"/> in a specified GDI drawing context.
        /// </summary>
        /// <param name="icon">The identicon to draw.</param>
        /// <param name="g">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon, including padding.</param>
        public static void Draw(this Identicon icon, Graphics g, Rendering.Rectangle rect)
        {
            var renderer = new GdiRenderer(g);
            icon.Draw(renderer, rect);
        }

        /// <summary>
        /// Draws an <see cref="Identicon"/> in a specified GDI drawing context.
        /// </summary>
        /// <param name="icon">The identicon to draw.</param>
        /// <param name="g">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon, including padding.</param>
        public static void Draw(this Identicon icon, Graphics g, System.Drawing.Rectangle rect)
        {
            icon.Draw(g, rect.ToJdenticon());
        }

        /// <summary>
        /// Draws an <see cref="Identicon"/> in a specified GDI drawing context at position (0, 0).
        /// </summary>
        /// <param name="icon">The identicon to draw.</param>
        /// <param name="g">Drawing context in which the icon will be rendered.</param>
        public static void Draw(this Identicon icon, Graphics g)
        {
            var renderer = new GdiRenderer(g);
            icon.Draw(renderer);
        }

        /// <summary>
        /// Renders an <see cref="Identicon"/> to a GDI <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="icon">The identicon to render.</param>
        /// <remarks>
        /// The caller should dispose the returned <see cref="Bitmap"/> once it does not
        /// need it anymore.
        /// </remarks>
        public static Bitmap ToBitmap(this Identicon icon)
        {
            var img = new Bitmap(icon.Size, icon.Size);
            try
            {
                using (var g = Graphics.FromImage(img))
                {
                    icon.Draw(g);
                }

                return img;
            }
            catch
            {
                img.Dispose();
                throw;
            }
        }

        private static byte[] ToMetafile(this Identicon icon)
        {
            using (var desktopGraphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hdc = desktopGraphics.GetHdc();
                try
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var img = new Metafile(memoryStream, hdc,
                            new System.Drawing.Rectangle(0, 0, icon.Size, icon.Size), MetafileFrameUnit.Pixel,
                            EmfType.EmfPlusDual))
                        {
                            using (var graphics = Graphics.FromImage(img))
                            {
                                icon.Draw(graphics);
                            }
                        }

                        return memoryStream.ToArray();
                    }
                }
                finally
                {
                    desktopGraphics.ReleaseHdc(hdc);
                }
            }
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> as an Enhanced Metafile (EMF).
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        public static Stream SaveAsEmf(this Identicon icon)
        {
            return new MemoryStream(icon.ToMetafile(), false);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> as an Enhanced Metafile (EMF).
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the EMF data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static void SaveAsEmf(this Identicon icon, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var emf = icon.ToMetafile();
            stream.Write(emf, 0, emf.Length);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> as an Enhanced Metafile (EMF).
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the EMF file to create. If the file already exists it will be overwritten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static void SaveAsEmf(this Identicon icon, string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var emf = icon.ToMetafile();
                stream.Write(emf, 0, emf.Length);
            }
        }

#if SUPPORT_ASYNC_AWAIT
        /// <summary>
        /// Saves an <see cref="Identicon"/> as an Enhanced Metafile (EMF) asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the EMF data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static Task SaveAsEmfAsync(this Identicon icon, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var emf = icon.ToMetafile();
            return stream.WriteAsync(emf, 0, emf.Length);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> as an Enhanced Metafile (EMF) asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the EMF file to create. If the file already exists it will be overwritten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static async Task SaveAsEmfAsync(this Identicon icon, string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var emf = icon.ToMetafile();
                await stream.WriteAsync(emf, 0, emf.Length);
            }
        }
#endif
    }
}

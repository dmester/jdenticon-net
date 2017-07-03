#region License
//
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2016
//
// This software is provided 'as-is', without any express or implied
// warranty.In no event will the authors be held liable for any damages
// arising from the use of this software.
// 
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it
// freely, subject to the following restrictions:
// 
// 1. The origin of this software must not be misrepresented; you must not
//    claim that you wrote the original software.If you use this software
//    in a product, an acknowledgment in the product documentation would be
//    appreciated but is not required.
// 
// 2. Altered source versions must be plainly marked as such, and must not be
//    misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
//
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

namespace Jdenticon
{
    /// <summary>
    /// Extends <see cref="Identicon"/> with GDI specific methods.
    /// </summary>
    public static class GdiExtensions
    {
        /// <summary>
        /// Draws this icon in the specified drawing context.
        /// </summary>
        /// <param name="g">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon. No padding will be applied to the rectangle.</param>
        public static void Draw(this Identicon icon, Graphics g, Rendering.Rectangle rect)
        {
            var renderer = new GdiRenderer(g);
            icon.Draw(renderer, rect);
        }

        /// <summary>
        /// Draws this icon in the specified drawing context.
        /// </summary>
        /// <param name="g">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon. No padding will be applied to the rectangle.</param>
        public static void Draw(this Identicon icon, Graphics g, System.Drawing.Rectangle rect)
        {
            icon.Draw(g, rect.ToJdenticon());
        }
        
        /// <summary>
        /// Creates a bitmap icon.
        /// </summary>
        /// <param name="size">The size of the generated bitmap in pixels.</param>
        public static Bitmap ToBitmap(this Identicon icon, int size)
        {
            if (size < 30) throw new ArgumentOutOfRangeException("size", size, "The size was too small. Only sizes greater than or equal to 30 pixels are supported.");

            var iconBounds = icon.GetIconBounds(size);
            var img = new Bitmap(size, size);
            try
            {
                using (var g = Graphics.FromImage(img))
                {
                    icon.Draw(g, iconBounds);
                }

                return img;
            }
            catch
            {
                img.Dispose();
                throw;
            }
        }

        private static void ToMetafile(Identicon icon, Stream stream, int size)
        {
            var iconBounds = icon.GetIconBounds(size);

            using (var desktopGraphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hdc = desktopGraphics.GetHdc();
                try
                {
                    using (var img = new Metafile(stream, hdc,
                        new System.Drawing.Rectangle(0, 0, size, size), MetafileFrameUnit.Pixel,
                        EmfType.EmfPlusDual))
                    {
                        using (var graphics = Graphics.FromImage(img))
                        {
                            icon.Draw(graphics, iconBounds);
                        }
                    }
                }
                finally
                {
                    desktopGraphics.ReleaseHdc(hdc);
                }
            }
        }

        /// <summary>
        /// Saves this icon as an Enhanced Metafile (.emf).
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> less than 1 pixel.</exception>
        public static Stream SaveAsEmf(this Identicon icon, int size)
        {
            var memoryStream = new MemoryStream();
            icon.SaveAsEmf(memoryStream, size);
            memoryStream.Position = 0;
            return memoryStream;
        }

        /// <summary>
        /// Saves this icon as an Enhanced Metafile (.emf).
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="stream">The stream to which the icon will be written.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> less than 1 pixel.</exception>
        public static void SaveAsEmf(this Identicon icon, Stream stream, int size)
        {
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), size, "The size should be 1 pixel or larger.");
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            ToMetafile(icon, stream, size);
        }

        /// <summary>
        /// Saves this icon as an Enhanced Metafile (.emf).
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="path">The path to the file to which the icon will be written.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> less than 1 pixel.</exception>
        public static void SaveAsEmf(this Identicon icon, string path, int size)
        {
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), size, "The size should be 1 pixel or larger.");
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                icon.SaveAsEmf(stream, size);
            }
        }
    }
}

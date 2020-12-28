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

using Jdenticon.IO;
using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

#if SUPPORT_ASYNC_AWAIT
using System.Threading.Tasks;
#endif

namespace Jdenticon
{
    /// <summary>
    /// Extends <see cref="Identicon"/> with SVG specific methods.
    /// </summary>
    public static class SvgExtensions
    {
        private static string GenerateSvg(this Identicon icon, bool fragment)
        {
            var renderer = new SvgRenderer(icon.Size, icon.Size);
            icon.Draw(renderer);
            return renderer.ToSvg(fragment);
        }

        private static byte[] GenerateBinarySvg(this Identicon icon, bool fragment)
        {
            var svg = icon.GenerateSvg(fragment);
            var binaryLength = Encoding.UTF8.GetByteCount(svg);
            var preamble = Encoding.UTF8.GetPreamble();

            var buffer = new byte[binaryLength + preamble.Length];
            for (var i = 0; i < preamble.Length; i++)
            {
                buffer[i] = preamble[i];
            }

            Encoding.UTF8.GetBytes(svg, 0, svg.Length, buffer, preamble.Length);
            return buffer;
        }

        /// <summary>
        /// Renders an <see cref="Identicon"/> as a Scalable Vector Graphics (SVG) data string.
        /// </summary>
        /// <param name="icon">The identicon to render as SVG.</param>
        public static string ToSvg(this Identicon icon)
        {
            return icon.ToSvg(false);
        }

        /// <summary>
        /// Renders an <see cref="Identicon"/> as a Scalable Vector Graphics (SVG) data string.
        /// </summary>
        /// <param name="icon">The identicon to render as SVG.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        public static string ToSvg(this Identicon icon, bool fragment)
        {
            return icon.GenerateSvg(fragment);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to which the SVG data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, TextWriter writer)
        {
            icon.SaveAsSvg(writer, false);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the SVG data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, Stream stream)
        {
            icon.SaveAsSvg(stream, false);
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the SVG file to create. If the file already exists it will be overwritten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, string path)
        {
            icon.SaveAsSvg(path, false);
        }
#endif

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to which the SVG data will be written.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, TextWriter writer, bool fragment)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            var svg = icon.GenerateSvg(fragment);
            writer.Write(svg);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the SVG data will be written.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, Stream stream, bool fragment)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var binarySvg = icon.GenerateBinarySvg(fragment);
            stream.Write(binarySvg, 0, binarySvg.Length);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        public static Stream SaveAsSvg(this Identicon icon, bool fragment)
        {
            var binarySvg = icon.GenerateBinarySvg(fragment);
            return new MemoryStream(binarySvg, false);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        public static Stream SaveAsSvg(this Identicon icon)
        {
            return SaveAsSvg(icon, false);
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the SVG file to create. If the file already exists it will be overwritten.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, string path, bool fragment)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                icon.SaveAsSvg(stream, fragment);
            }
        }
#endif

#if SUPPORT_ASYNC_AWAIT
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to which the SVG data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> was <c>null</c>.</exception>
        public static Task SaveAsSvgAsync(this Identicon icon, TextWriter writer)
        {
            return icon.SaveAsSvgAsync(writer, false);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the SVG data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static Task SaveAsSvgAsync(this Identicon icon, Stream stream)
        {
            return icon.SaveAsSvgAsync(stream, false);
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the SVG file to create. If the file already exists it will be overwritten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static Task SaveAsSvgAsync(this Identicon icon, string path)
        {
            return icon.SaveAsSvgAsync(path, false);
        }
#endif

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to which the SVG data will be written.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> was <c>null</c>.</exception>
        public static Task SaveAsSvgAsync(this Identicon icon, TextWriter writer, bool fragment)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            var svg = icon.GenerateSvg(fragment);
            return writer.WriteAsync(svg);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the SVG data will be written.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static Task SaveAsSvgAsync(this Identicon icon, Stream stream, bool fragment)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var binarySvg = icon.GenerateBinarySvg(fragment);
            return stream.WriteAsync(binarySvg, 0, binarySvg.Length);
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Scalable Vector Graphics (SVG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the SVG file to create. If the file already exists it will be overwritten.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static async Task SaveAsSvgAsync(this Identicon icon, string path, bool fragment)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                await icon.SaveAsSvgAsync(stream, fragment);
            }
        }
#endif
#endif
    }
}

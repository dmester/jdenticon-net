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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon
{
    /// <summary>
    /// Extends <see cref="Identicon"/> with SVG specific methods.
    /// </summary>
    public static class SvgExtensions
    {
        private static void GenerateSvg(this Identicon icon, TextWriter writer, bool fragment)
        {
            var iconBounds = icon.GetIconBounds();
            var renderer = new SvgRenderer(icon.Size, icon.Size);
            icon.Draw(renderer, iconBounds);
            renderer.Save(writer, fragment);
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
            using (var writer = new StringWriter())
            {
                icon.GenerateSvg(writer, fragment);
                return writer.ToString();
            }
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
            icon.GenerateSvg(writer, fragment);
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

            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                icon.SaveAsSvg(writer, fragment);
            }
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
            var memoryStream = new MemoryStream();
            icon.SaveAsSvg(memoryStream, fragment);
            memoryStream.Position = 0;
            return memoryStream;
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
            if (path == null) throw new ArgumentNullException("path");

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                icon.SaveAsSvg(stream, fragment);
            }
        }
#endif
    }
}

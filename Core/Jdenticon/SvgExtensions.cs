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
        /// Creates a string containing an SVG version of this icon.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        public static string ToSvg(this Identicon icon)
        {
            return icon.ToSvg(false);
        }

        /// <summary>
        /// Creates a string containing an SVG version of this icon.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
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
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to which the icon will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, TextWriter writer)
        {
            icon.SaveAsSvg(writer, false);
        }

        /// <summary>
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="stream">The stream to which the icon will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, Stream stream)
        {
            icon.SaveAsSvg(stream, false);
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="path">The path to the file to which the icon will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static void SaveAsSvg(this Identicon icon, string path)
        {
            icon.SaveAsSvg(path, false);
        }
#endif
        
        /// <summary>
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="writer">The <see cref="TextWriter"/> to which the icon will be written.</param>
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
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="stream">The stream to which the icon will be written.</param>
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
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
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
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        public static Stream SaveAsSvg(this Identicon icon)
        {
            return SaveAsSvg(icon, false);
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves this icon as an SVG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="path">The path to the file to which the icon will be written.</param>
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

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
    /// Extends <see cref="Identicon"/> with PNG specific methods.
    /// </summary>
    public static class PngExtensions
    {
        /// <summary>
        /// Saves this icon as a PNG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="stream">The stream to which the icon will be written.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> less than 1 pixel.</exception>
        public static void SaveAsPng(this Identicon icon, Stream stream, int size)
        {
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), size, "The size should be 1 pixel or larger.");
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var renderer = new PngRenderer(size, size);
            var iconBounds = icon.GetIconBounds(size);
            icon.Draw(renderer, iconBounds);
            renderer.SavePng(stream);
        }

        /// <summary>
        /// Saves this icon as a PNG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> less than 1 pixel.</exception>
        public static Stream SaveAsPng(this Identicon icon, int size)
        {
            var memoryStream = new MemoryStream();
            icon.SaveAsPng(memoryStream, size);
            memoryStream.Position = 0;
            return memoryStream;
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves this icon as a PNG file.
        /// </summary>
        /// <param name="icon">Icon instance.</param>
        /// <param name="path">The path to the file to which the icon will be written.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> less than 1 pixel.</exception>
        public static void SaveAsPng(this Identicon icon, string path, int size)
        {
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), size, "The size should be 1 pixel or larger.");
            if (path == null) throw new ArgumentNullException("path");
            
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                icon.SaveAsPng(stream, size);
            }
        }
#endif
    }
}

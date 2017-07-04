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
        /// Saves an <see cref="Identicon"/> icon as a Portable Network Graphics (PNG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the PNG data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static void SaveAsPng(this Identicon icon, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var renderer = new PngRenderer(icon.Size, icon.Size);
            var iconBounds = icon.GetIconBounds();
            icon.Draw(renderer, iconBounds);
            renderer.SavePng(stream);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Portable Network Graphics (PNG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        public static Stream SaveAsPng(this Identicon icon)
        {
            var memoryStream = new MemoryStream();
            icon.SaveAsPng(memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Portable Network Graphics (PNG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the PNG file to create. If the file already exists it will be overwritten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static void SaveAsPng(this Identicon icon, string path)
        {
            if (path == null) throw new ArgumentNullException("path");
            
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                icon.SaveAsPng(stream);
            }
        }
#endif
    }
}

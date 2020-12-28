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

#if SUPPORT_ASYNC_AWAIT
using System.Threading.Tasks;
#endif

namespace Jdenticon
{
    /// <summary>
    /// Extends <see cref="Identicon"/> with PNG specific methods.
    /// </summary>
    public static class PngExtensions
    {
        private static byte[] GeneratePng(this Identicon icon)
        {
            var renderer = new PngRenderer(icon.Size, icon.Size);
            icon.Draw(renderer);

            using (var memoryStream = new MemoryStream())
            {
                renderer.SavePng(memoryStream);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Portable Network Graphics (PNG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the PNG data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static void SaveAsPng(this Identicon icon, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var pngData = icon.GeneratePng();
            stream.Write(pngData, 0, pngData.Length);
        }

        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Portable Network Graphics (PNG) file.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        public static Stream SaveAsPng(this Identicon icon)
        {
            var pngData = icon.GeneratePng();
            return new MemoryStream(pngData, false);
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
            if (path == null) throw new ArgumentNullException(nameof(path));
            
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var pngData = icon.GeneratePng();
                stream.Write(pngData, 0, pngData.Length);
            }
        }
#endif

#if SUPPORT_ASYNC_AWAIT
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Portable Network Graphics (PNG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="stream">The stream to which the PNG data will be written.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> was <c>null</c>.</exception>
        public static Task SaveAsPngAsync(this Identicon icon, Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            var pngData = icon.GeneratePng();
            return stream.WriteAsync(pngData, 0, pngData.Length);
        }
        
#if HAVE_FILE_STREAM
        /// <summary>
        /// Saves an <see cref="Identicon"/> icon as a Portable Network Graphics (PNG) file asynchronously.
        /// </summary>
        /// <param name="icon">The identicon to save.</param>
        /// <param name="path">The path to the PNG file to create. If the file already exists it will be overwritten.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> was <c>null</c>.</exception>
        public static async Task SaveAsPngAsync(this Identicon icon, string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var pngData = icon.GeneratePng();
                await stream.WriteAsync(pngData, 0, pngData.Length);
            }
        }
#endif
#endif
    }
}

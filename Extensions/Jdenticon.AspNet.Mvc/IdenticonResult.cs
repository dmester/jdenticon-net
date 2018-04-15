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

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Jdenticon.AspNet.Mvc
{
    /// <summary>
    /// Defines a result that will render and return an identicon.
    /// </summary>
    public class IdenticonResult : ActionResult
    {
        private Identicon icon;
        private ExportImageFormat format;
        
        private IdenticonResult(Identicon icon, ExportImageFormat format = ExportImageFormat.Png)
        {
            this.icon = icon;
            this.format = format;
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 6 bytes.</param>
        /// <param name="size">The size of the icon in pixels.</param>
        /// <param name="format">The format of the generated icon.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
        /// <exception cref="ArgumentException"><paramref name="hash"/> was too short.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> was <c>null</c>.</exception>
        public static IdenticonResult FromHash(byte[] hash, int size, ExportImageFormat format = ExportImageFormat.Png)
        {
            return new IdenticonResult(Identicon.FromHash(hash, size), format);
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hex encoded hash that will be used as base for the icon. The hash string must contain at least 12 characters.</param>
        /// <param name="size">The size of the icon in pixels.</param>
        /// <param name="format">The format of the generated icon.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
        /// <exception cref="ArgumentException"><paramref name="hash"/> was too short.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> was <c>null</c>.</exception>
        public static IdenticonResult FromHash(string hash, int size, ExportImageFormat format = ExportImageFormat.Png)
        {
            return new IdenticonResult(Identicon.FromHash(hash, size), format);
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with a hash of the specified object.
        /// </summary>
        /// <param name="value">The string representation of this object will be hashed and used as base for this icon. Null values are supported and handled as empty strings.</param>
        /// <param name="size">The size of the icon in pixels.</param>
        /// <param name="format">The format of the generated icon.</param>
        /// <param name="hashAlgorithmName">The name of the hash algorithm to use for hashing.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
        public static IdenticonResult FromValue(object value, int size, ExportImageFormat format = ExportImageFormat.Png, string hashAlgorithmName = "SHA1")
        {
            return new IdenticonResult(Identicon.FromValue(value, size, hashAlgorithmName), format);
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with a hash of the specified object.
        /// </summary>
        /// <param name="icon">The <see cref="Identicon"/> to be rendered.</param>
        /// <param name="format">The format of the generated icon.</param>
        /// <exception cref="ArgumentNullException"><paramref name="icon"/> was <c>null</c>.</exception>
        public static IdenticonResult FromIcon(Identicon icon, ExportImageFormat format = ExportImageFormat.Png)
        {
            if (icon == null) throw new ArgumentNullException(nameof(icon));
            return new IdenticonResult(icon, format);
        }

        /// <summary>
        /// Renders the requested identicon and returns it to the client.
        /// </summary>
        /// <param name="context"><see cref="ControllerContext"/> with the current request and response.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            switch (format)
            {
                case ExportImageFormat.Png:
                    response.ContentType = "image/png";
                    icon.SaveAsPng(response.OutputStream);
                    break;
                case ExportImageFormat.Svg:
                    response.ContentType = "image/svg+xml";
                    icon.SaveAsSvg(response.OutputStream);
                    break;
                default:
                    throw new NotSupportedException($"The image format '{format}' is not supported by {nameof(IdenticonResult)}.");
            }
        }
    }
}

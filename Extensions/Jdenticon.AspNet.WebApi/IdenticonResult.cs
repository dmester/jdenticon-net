﻿#region License
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Jdenticon.AspNet.WebApi
{
    /// <summary>
    /// Defines a result that will render and return an identicon.
    /// </summary>
    public class IdenticonResult : IHttpActionResult
    {
        private Identicon icon;
        private ExportImageFormat format;

        private IdenticonResult(Identicon icon, ExportImageFormat format = ExportImageFormat.Png)
        {
            this.icon = icon;
            this.format = format;
        }

        /// <summary>
        /// Creates an <see cref="IdenticonResult"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 6 bytes.</param>
        /// <param name="size">The size of the icon in pixels.</param>
        /// <param name="format">The format of the generated icon.</param>
        public static IdenticonResult FromHash(byte[] hash, int size, ExportImageFormat format = ExportImageFormat.Png)
        {
            return new IdenticonResult(Identicon.FromHash(hash, size), format);
        }

        /// <summary>
        /// Creates an <see cref="IdenticonResult"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 6 bytes.</param>
        /// <param name="size">The size of the icon in pixels.</param>
        /// <param name="format">The format of the generated icon.</param>
        public static IdenticonResult FromHash(string hash, int size, ExportImageFormat format = ExportImageFormat.Png)
        {
            return new IdenticonResult(Identicon.FromHash(hash, size), format);
        }

        /// <summary>
        /// Creates an <see cref="IdenticonResult"/> instance with a hash of the specified object.
        /// </summary>
        /// <param name="value">The string representation of this object will be hashed and used as base for this icon.</param>
        /// <param name="size">The size of the icon in pixels.</param>
        /// <param name="format">The format of the generated icon.</param>
        /// <param name="hashAlgorithmName">The name of the hash algorithm to use for hashing.</param>
        public static IdenticonResult FromValue(object value, int size, ExportImageFormat format = ExportImageFormat.Png, string hashAlgorithmName = "SHA1")
        {
            return new IdenticonResult(Identicon.FromValue(value, size, hashAlgorithmName), format);
        }

        /// <summary>
        /// Creates an <see cref="IdenticonResult"/> instance with a hash of the specified object.
        /// </summary>
        /// <param name="icon">The <see cref="Identicon"/> to be rendered.</param>
        /// <param name="format">The format of the generated icon.</param>
        public static IdenticonResult FromIcon(Identicon icon, ExportImageFormat format = ExportImageFormat.Png)
        {
            return new IdenticonResult(icon, format);
        }

        /// <summary>
        /// Creates an <see cref="HttpResponseMessage"/> containing an identicon.
        /// </summary>
        /// <param name="cancellationToken">Not supported by Jdenticon.</param>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new MemoryStream();
            
            switch (format)
            {
                case ExportImageFormat.Png:
                    response.Content = new StreamContent(icon.SaveAsPng());
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                    break;
                case ExportImageFormat.Svg:
                    response.Content = new StreamContent(icon.SaveAsSvg());
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/svg+xml");
                    break;
                default:
                    throw new NotSupportedException($"The image format '{format}' is not supported by {nameof(IdenticonResult)}.");
            }
            
            return Task.FromResult(response);
        }
    }
}

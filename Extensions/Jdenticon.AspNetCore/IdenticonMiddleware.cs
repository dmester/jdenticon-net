#region License
// Jdenticon-net
// https://github.com/dmester/jdenticon-net
// Copyright © Daniel Mester Pirttijärvi 2018
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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.AspNetCore
{
    /// <summary>
    /// Middleware for serving identicons. Only requests having a path only consisting
    /// of a serialized <see cref="IdenticonRequest"/> are handled.
    /// </summary>
    internal class IdenticonMiddleware
    {
        private readonly RequestDelegate next;

        /// <summary>
        /// Gets or sets a value indicating whether an instance of <see cref="IdenticonMiddleware"/>
        /// has been created.
        /// </summary>
        public static bool IsConstructed { get; private set; }

        /// <summary>
        /// Creates a new instance of the class <see cref="IdenticonMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware to be called.</param>
        public IdenticonMiddleware(RequestDelegate next)
        {
            this.next = next;
            IsConstructed = true;
        }

        /// <summary>
        /// Checks if an <see cref="IdenticonMiddleware"/> has been created. If
        /// not, an exception will be thrown.
        /// </summary>
        public static void EnsureConstructed()
        {
            if (!IsConstructed)
            {
                throw new InvalidOperationException(
                    "To render identicons the Jdenticon middleware must be registered, " +
                    "typically by calling app.UseJdenticon() in Startup.Configure.");
            }
        }

        /// <summary>
        /// Processes a request and determines whether it can be handled as an identicon request.
        /// </summary>
        /// <param name="context">Current context.</param>
        public Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;
            if (path.HasValue)
            {
                var requestString = context.Request.Path.Value.Trim('/');

                if (IdenticonRequest.TryParse(requestString, out var request))
                {
                    return HandleRequestAsync(context, request);
                }
            }

            return next(context);
        }

        private async Task HandleRequestAsync(HttpContext context, IdenticonRequest request)
        {
            var icon = Identicon.FromHash(request.Hash, request.Size);
            icon.Style = request.Style;

            var headers = context.Response.GetTypedHeaders();
            Stream data;
            string contentType;

            switch (request.Format)
            {
                case ExportImageFormat.Svg:
                    data = icon.SaveAsSvg();
                    contentType = "image/svg+xml";
                    break;
                default:
                    data = icon.SaveAsPng();
                    contentType = "image/png";
                    break;
            }

            try
            {
                context.Response.ContentType = contentType;
                context.Response.ContentLength = data.Length;

                // The urls are permanent so it is ok to cache icons for a long time
                headers.CacheControl =
                    new CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromDays(30)
                    };

                await data.CopyToAsync(context.Response.Body);
            }
            finally
            {
                data?.Dispose();
            }
        }
    }
}

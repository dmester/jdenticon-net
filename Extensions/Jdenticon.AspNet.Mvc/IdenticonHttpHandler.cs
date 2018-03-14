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
using System.Web;

namespace Jdenticon.AspNet.Mvc
{
    /// <summary>
    /// <see cref="IHttpHandler"/> implementation used to serve icons.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <see cref="IdenticonRequest"/> is used to generate a parameter string that is passed as
    /// query string to the handler without any key, e.g. <c>Identicon.axd?1gMA84z~Li~s84z~Lg--</c>.
    /// </para>
    /// <para>
    /// Since a specific URL is always generating the same icon, the HTTP responses will be served with 
    /// a <c>Cache-Control</c> header that allows caching the icon for 1 year.
    /// </para>
    /// </remarks>
    public class IdenticonHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Creates a new instance of <see cref="IdenticonHttpHandler"/>.
        /// </summary>
        public IdenticonHttpHandler()
        {
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="IdenticonHttpHandler"/> instance.
        /// </summary>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Renders the requested identicon and returns it to the client.
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/> with the current request and response.</param>
        public void ProcessRequest(HttpContext context)
        {
            if (IdenticonRequest.TryParse(context.Request.Url.Query, out var request))
            {
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetMaxAge(TimeSpan.FromDays(365));

                var icon = Identicon.FromHash(request.Hash, request.Size);
                icon.Style = request.Style;
                
                switch (request.Format)
                {
                    case ExportImageFormat.Png:
                        context.Response.ContentType = "image/png";
                        icon.SaveAsPng(context.Response.OutputStream);
                        break;
                    case ExportImageFormat.Svg:
                        context.Response.ContentType = "image/svg+xml";
                        icon.SaveAsSvg(context.Response.OutputStream);
                        break;
                    default:
                        throw new NotSupportedException($"The image format '{request.Format}' is not supported by {nameof(IdenticonHttpHandler)}.");
                }
            }
            else
            {
                throw new HttpException(404, "Icon not found");
            }
        }
    }
}

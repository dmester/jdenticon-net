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

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Jdenticon.AspNet.Mvc
{
    /// <summary>
    /// <see cref="IHttpHandler"/> implementation used to serve icons.
    /// </summary>
    public class IdenticonHttpHandler : IHttpHandler
    {
        /// <summary>
        /// Creates a new instance of <see cref="IdenticonHttpHandler"/>.
        /// </summary>
        public IdenticonHttpHandler()
        {
        }

        /// <inheritdoc/>
        public bool IsReusable
        {
            get { return true; }
        }

        /// <inheritdoc/>
        public void ProcessRequest(HttpContext context)
        {
            if (IdenticonRequest.TryParse(context.Request.Url.Query, out var request))
            {
                context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetMaxAge(TimeSpan.FromDays(365));

                var icon = Identicon.FromHash(request.Hash);
                icon.Style = request.Style;
                
                switch (request.Format)
                {
                    case ExportImageFormat.Png:
                        context.Response.ContentType = "image/png";
                        icon.SaveAsPng(context.Response.OutputStream, request.Size);
                        break;
                    case ExportImageFormat.Svg:
                        context.Response.ContentType = "image/svg+xml";
                        icon.SaveAsSvg(context.Response.OutputStream, request.Size);
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

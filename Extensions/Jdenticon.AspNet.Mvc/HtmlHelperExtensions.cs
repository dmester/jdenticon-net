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
using System.Web.Mvc;

namespace Jdenticon.AspNet.Mvc
{
    /// <summary>
    /// <see cref="HtmlHelper"/> extension methods for Jdenticon.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Renders an identicon as an IMG tag.
        /// </summary>
        /// <param name="helper">The <see cref="HtmlHelper"/>.</param>
        /// <param name="value">The value that will be hashed and used as base for the icon.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="alt">The alt attribute of the rendered image.</param>
        /// <param name="style">The icon style.</param>
        /// <param name="format">The file format of the generated icon.</param>
        public static MvcHtmlString Identicon(this HtmlHelper helper, object value, int size, string alt = null, ExportImageFormat format = ExportImageFormat.Png, IdenticonStyle style = null)
        {
            var hash = HashGenerator.ComputeHash(value, "SHA1");
            return helper.Identicon(hash, size, alt, format, style);
        }

        /// <summary>
        /// Renders an identicon as an IMG tag.
        /// </summary>
        /// <param name="helper">The <see cref="HtmlHelper"/>.</param>
        /// <param name="icon">The icon that will be rendered.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="alt">The alt attribute of the rendered image.</param>
        /// <param name="format">The file format of the generated icon.</param>
        public static MvcHtmlString Identicon(this HtmlHelper helper, Identicon icon, int size, string alt = null, 
            ExportImageFormat format = ExportImageFormat.Png)
        {
            return helper.Identicon(icon.Hash, size, alt, format, icon.Style);
        }

        /// <summary>
        /// Renders an identicon as an IMG tag.
        /// </summary>
        /// <param name="helper">The <see cref="HtmlHelper"/>.</param>
        /// <param name="hash">The hash that will be used as base for the icon.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="alt">The alt attribute of the rendered image.</param>
        /// <param name="style">The icon style.</param>
        /// <param name="format">The file format of the generated icon.</param>
        public static MvcHtmlString Identicon(this HtmlHelper helper, byte[] hash, int size, string alt = null, 
            ExportImageFormat format = ExportImageFormat.Png, IdenticonStyle style = null)
        {
            var url = IdenticonUrl.Create(helper.ViewContext.HttpContext.Response, hash, size, format, style);

            var img = new TagBuilder("img");
            
            img.Attributes["src"] = url;
            img.Attributes["width"] = size.ToString();
            img.Attributes["height"] = size.ToString();
            img.Attributes["alt"] = alt ?? "";
            
            return new MvcHtmlString(img.ToString(TagRenderMode.SelfClosing));
        }
    }
}

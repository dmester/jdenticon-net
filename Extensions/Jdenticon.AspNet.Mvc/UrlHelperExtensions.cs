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
    /// <see cref="UrlHelper"/> extension methods for Jdenticon.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Generates an URL to an identicon.
        /// </summary>
        /// <param name="helper">The <see cref="UrlHelper"/>.</param>
        /// <param name="value">The value that will be hashed and used as base for the icon.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="style">The icon style.</param>
        /// <param name="format">The file format of the generated icon.</param>
        public static string Identicon(this UrlHelper helper, object value, int size, ExportImageFormat format = ExportImageFormat.Png, IdenticonStyle style = null)
        {
            var hash = HashGenerator.ComputeHash(value, "SHA1");
            return helper.Identicon(hash, size, format, style);
        }

        /// <summary>
        /// Generates an URL to an identicon.
        /// </summary>
        /// <param name="helper">The <see cref="UrlHelper"/>.</param>
        /// <param name="hash">The hash that will be used as base for the icon.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="style">The icon style.</param>
        /// <param name="format">The file format of the generated icon.</param>
        public static string Identicon(this UrlHelper helper, byte[] hash, int size, ExportImageFormat format = ExportImageFormat.Png, IdenticonStyle style = null)
        {
            return IdenticonUrl.Create(helper.RequestContext.HttpContext.Response, hash, size, format, style);
        }

        /// <summary>
        /// Generates an URL to an identicon.
        /// </summary>
        /// <param name="helper">The <see cref="UrlHelper"/>.</param>
        /// <param name="icon">The icon that will be rendered.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="format">The file format of the generated icon.</param>
        public static string Identicon(this UrlHelper helper, Identicon icon, int size, ExportImageFormat format = ExportImageFormat.Png)
        {
            if (icon == null) throw new ArgumentNullException(nameof(icon));
            return helper.Identicon(icon.Hash, size, format, icon.Style);
        }
    }
}

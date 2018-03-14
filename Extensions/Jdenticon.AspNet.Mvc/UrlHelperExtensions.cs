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

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
    /// <see cref="HtmlHelper"/> extension methods for Jdenticon.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <inheritdoc cref="Identicon(HtmlHelper, Jdenticon.Identicon, int, string, ExportImageFormat)" />
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
        /// <param name="size">The size of the generated icon in pixels. If no size is specified the size of <paramref name="icon"/> will be used.</param>
        /// <param name="alt">The alt attribute of the rendered image.</param>
        /// <param name="format">The file format of the generated icon.</param>
        /// <remarks>
        /// <para>
        /// This extension method can be used in your cshtml files to insert identicons as IMG tags.
        /// The IMG tag will include an url that will be handled by the <see cref="IdenticonHttpHandler"/>
        /// handler, which is automatically installed in your Web.config when installing the 
        /// <c>Jdenticon.AspNet.Mvc</c> NuGet package.
        /// </para>
        /// <para>
        /// If you don't need a whole IMG tag, but rather just an url, please have a look at 
        /// <see cref="UrlHelperExtensions"/>.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// This example shows how to use the <see cref="HtmlHelper"/> extension methods to render
        /// identicons as IMG tags.
        /// </para>
        /// <code language="html" title="HtmlHelper approach">
        /// @using Jdenticon.AspNet.Mvc;
        /// 
        /// &lt;!-- The following markup --&gt;
        /// 
        /// &lt;div class="user-info"&gt;
        ///     @Html.Identicon("JohnDoe64", 60, alt: "JohnDoe64 icon")
        ///     &lt;div class="user-info__name"&gt;JohnDoe64&lt;/div&gt;
        /// &lt;/div&gt;
        /// 
        /// &lt;!-- is rendered as --&gt;
        /// 
        /// &lt;div class="user-info"&gt;
        ///     &lt;img src="/identicon.axd?5AMA8Xyneag78XyneQ--" width="60" height="60" alt="JohnDoe64 icon" /&gt;
        ///     &lt;div class="user-info__name"&gt;JohnDoe64&lt;/div&gt;
        /// &lt;/div&gt;
        /// </code>
        /// </example>
        public static MvcHtmlString Identicon(this HtmlHelper helper, Identicon icon, int size = 0, string alt = null, 
            ExportImageFormat format = ExportImageFormat.Png)
        {
            if (size == 0)
            {
                size = icon.Size;
            }

            return helper.Identicon(icon.Hash, size, alt, format, icon.Style);
        }

        /// <inheritdoc cref="Identicon(HtmlHelper, Jdenticon.Identicon, int, string, ExportImageFormat)" />
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

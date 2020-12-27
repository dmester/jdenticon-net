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
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jdenticon.AspNetCore
{
    /// <summary>
    /// <see cref="IUrlHelper"/> extension methods for Jdenticon.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <inheritdoc cref="Identicon(IUrlHelper, Jdenticon.Identicon, int, ExportImageFormat)" />
        /// <summary>
        /// Generates an URL to an identicon.
        /// </summary>
        /// <param name="helper">The <see cref="IUrlHelper"/>.</param>
        /// <param name="value">The value that will be hashed and used as base for the icon.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="style">The icon style.</param>
        /// <param name="format">The file format of the generated icon.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="helper"/> was <c>null</c>.</exception>
        public static string Identicon(this IUrlHelper helper, object value, int size, ExportImageFormat format = ExportImageFormat.Png, IdenticonStyle style = null)
        {
            var hash = HashGenerator.ComputeHash(value, "SHA1");
            return helper.Identicon(hash, size, format, style);
        }

        /// <inheritdoc cref="Identicon(IUrlHelper, Jdenticon.Identicon, int, ExportImageFormat)" />
        /// <summary>
        /// Generates an URL to an identicon.
        /// </summary>
        /// <param name="helper">The <see cref="IUrlHelper"/>.</param>
        /// <param name="hash">The hash that will be used as base for the icon. Must contain at least 6 bytes.</param>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="style">The icon style.</param>
        /// <param name="format">The file format of the generated icon.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
        /// <exception cref="ArgumentException"><paramref name="hash"/> was too short.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="helper"/> or <paramref name="hash"/> was <c>null</c>.</exception>
        public static string Identicon(this IUrlHelper helper, byte[] hash, int size, ExportImageFormat format = ExportImageFormat.Png, IdenticonStyle style = null)
        {
            if (helper == null) throw new ArgumentNullException(nameof(helper));
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if (hash.Length < 6) throw new ArgumentException("The specified hash is too short. At least 6 bytes are required.", nameof(hash));
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), size, "The size should be 1 pixel or larger");
            
            return IdenticonUrl.Create(helper.ActionContext.HttpContext, hash, size, format, style);
        }

        /// <summary>
        /// Generates an URL to an identicon.
        /// </summary>
        /// <param name="helper">The <see cref="IUrlHelper"/>.</param>
        /// <param name="icon">The icon that will be rendered.</param>
        /// <param name="size">The size of the generated icon in pixels. If no size is specified the size of <paramref name="icon"/> will be used.</param>
        /// <param name="format">The file format of the generated icon.</param>
        /// <remarks>
        /// <para>
        /// This extension method can be used in views to generate urls to identicons, 
        /// which are handled by the Jdenticon middleware. 
        /// Insert a call to <see cref="IdenticonBuilderExtensions.UseJdenticon(IApplicationBuilder)"/> right above 
        /// <see cref="StaticFileExtensions.UseStaticFiles(IApplicationBuilder)"/> to enable the Jdenticon middleware.
        /// </para>
        /// <code language="cs" title="Startup.cs">
        /// public class Startup
        /// {
        ///     /* ... */
        ///     
        ///     public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        ///     {
        ///         /* ... */
        ///         app.UseJdenticon();   /*highlight*/
        ///         app.UseStaticFiles();
        ///         app.UseMvc();
        ///         /* ... */
        ///     }
        /// }
        /// </code>
        /// </remarks>
        /// <example>
        /// <para>
        /// This example shows how to use the <see cref="IUrlHelper"/> extension methods to generate
        /// identicon urls.
        /// </para>
        /// <code language="html" title="HtmlHelper approach">
        /// @using Jdenticon.AspNetCore;
        /// 
        /// &lt;!-- The following markup --&gt;
        /// 
        /// &lt;div class="user-info"&gt;
        ///     &lt;img src="@Url.Identicon("JohnDoe64", 60)" width="60" height="60" alt="JohnDoe64 icon" &gt;
        ///     &lt;div class="user-info__name"&gt;JohnDoe64&lt;/div&gt;
        /// &lt;/div&gt;
        /// 
        /// &lt;!-- is rendered as --&gt;
        /// 
        /// &lt;div class="user-info"&gt;
        ///     &lt;img src="/identicons/5AMA8Xyneag78XyneQ--" width="60" height="60" alt="JohnDoe64 icon" /&gt;
        ///     &lt;div class="user-info__name"&gt;JohnDoe64&lt;/div&gt;
        /// &lt;/div&gt;
        /// </code>
        /// </example>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> was less than 1.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="helper"/> or <paramref name="icon"/> was <c>null</c>.</exception>
        public static string Identicon(this IUrlHelper helper, Identicon icon, int size = 0, ExportImageFormat format = ExportImageFormat.Png)
        {
            if (icon == null) throw new ArgumentNullException(nameof(icon));
            if (size == 0) size = icon.Size;

            return helper.Identicon(icon.Hash, size, format, icon.Style);
        }
    }
}

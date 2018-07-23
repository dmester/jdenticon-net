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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Jdenticon.AspNetCore
{
    /// <summary>
    /// Classes for using Jdenticon from ASP.NET Core. This namespace contains 
    /// classes for usage in Razor views, and an <see cref="Microsoft.AspNetCore.Mvc.IActionResult"/>
    /// for returning identicons from Mvc controllers.
    /// </summary>
    /// <example>
    /// <para>
    /// To use Jdenticon in a ASP.NET Core based application, 
    /// install the <c>Jdenticon.AspNetCore</c> NuGet package.
    /// </para>
    /// <code language="bat" title="NuGet Package Manager">
    /// PM&gt; Install-Package Jdenticon.AspNetCore
    /// </code>
    /// <para>
    /// After installing the NuGet package, enable Jdenticon in your applications
    /// by calling <see cref="IdenticonBuilderExtensions.UseJdenticon"/>
    /// in <c>Configure(IApplicationBuilder)</c> in your <c>Startup</c> class.
    /// Put it right above 
    /// <see cref="Microsoft.AspNetCore.Builder.StaticFileExtensions.UseStaticFiles"/>.
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
    /// <para>
    /// Make <see cref="IdenticonTagHelper"/> and the extensions for 
    /// <see cref="IHtmlHelper"/> and <see cref="IUrlHelper"/> available
    /// to your views by adding the following code to <c>_ViewImports.cshtml</c>.
    /// </para>
    /// <code language="html" title="_ViewImports.cshtml">
    /// @using MyWebApplication
    /// @namespace MyWebApplication.Pages
    /// @addTagHelper*, Microsoft.AspNetCore.Mvc.TagHelpers
    /// @using Jdenticon.AspNetCore             /*highlight*/
    /// @addTagHelper "*, Jdenticon.AspNetCore" /*highlight*/
    /// </code>
    /// 
    /// 
    /// <h4>Using tag helper</h4>
    /// <para>
    /// The <see cref="IdenticonTagHelper.Value">identicon-value</see> or 
    /// <see cref="IdenticonTagHelper.Hash">identicon-hash</see> attributes can be used 
    /// to render identicons in Razor templates. The width and the
    /// height of the img tag is used to determine the dimensions of the generated icon. 
    /// <see cref="IdenticonTagHelper.Format">identicon-format</see>
    /// is optional and defaults to PNG.
    /// </para>
    /// <code language="html" title="Using tag helper in a view">
    /// @{
    ///     var userId = "TestIcon";
    /// }
    /// &lt;div&gt;
    ///     An icon: &lt;img identicon-value="userId" identicon-format="Png" alt="Identicon" width="60" height="60" /&gt;
    /// &lt;/div&gt;
    /// 
    /// &lt;!-- is rendered as --&gt;
    /// &lt;div&gt;
    ///     An icon: &lt;img src="/jdenticon/5AMA8Xyneag78XyneQ--" alt="Identicon" width="60" height="60" /&gt;
    /// &lt;/div&gt;
    /// </code>
    /// 
    /// 
    /// 
    /// <h4>Using @Html.Identicon</h4>
    /// <para>
    /// Jdenticon can be used in multiple ways in ASP.NET MVC. The first example shows how 
    /// to use the <see cref="HtmlHelperExtensions">@Html extension methods</see> to render an IMG element
    /// on the page.
    /// </para>
    /// <code language="html" title="Using @Html.Identicon in a view">
    /// &lt;div&gt;
    ///     An icon: @Html.Identicon("TestIcon", 60, alt: "Identicon")
    /// &lt;/div&gt;
    /// 
    /// &lt;!-- is rendered as --&gt;
    /// &lt;div&gt;
    ///     An icon: &lt;img src="/identicons/5AMA8Xyneag78XyneQ--" width="60" height="60" alt="Identicon" /&gt;
    /// &lt;/div&gt;
    /// </code>
    /// 
    /// 
    /// 
    /// <h4>Using @Url.Identicon</h4>
    /// <para>
    /// The following example shows how to use the <see cref="UrlHelperExtensions">@Url extension methods</see> for
    /// generating identicon urls.
    /// </para>
    /// <code language="html" title="Using @Url.Identicon in a view">
    /// &lt;div&gt;
    ///     An icon: &lt;img src="@Url.Identicon("TestIcon", 60)" alt="Identicon" width="60" height="60" /&gt;
    /// &lt;/div&gt;
    /// 
    /// &lt;!-- is rendered as --&gt;
    /// &lt;div&gt;
    ///     An icon: &lt;img src="/identicons/5AMA8Xyneag78XyneQ--" alt="Identicon" width="60" height="60" /&gt;
    /// &lt;/div&gt;
    /// </code>
    /// 
    /// 
    /// 
    /// <h4>Returning an identicon from a MVC controller</h4>
    /// <para>
    /// It is also possible to return an identicon as a result from an ASP.NET MVC controller. Note
    /// that the actual icon data is returned by <see cref="IdenticonResult"/>. The other 
    /// approaches only writes urls to the page output, which are later handled by the 
    /// Jdenticon middleware.
    /// </para>
    /// <code language="cs" title="Identicon as action result">
    /// public class IconController : Controller
    /// {
    ///     public ActionResult Icon(string value, int size)
    ///     {
    ///         return IdenticonResult.FromValue(value, size);
    ///     }
    /// }
    /// </code>
    /// 
    /// 
    /// 
    /// </example>
    [CompilerGenerated]
    internal static class NamespaceDoc
    {
    }
}

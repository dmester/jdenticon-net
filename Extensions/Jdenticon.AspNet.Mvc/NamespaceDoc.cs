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
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Mvc;

namespace Jdenticon.AspNet.Mvc
{
    /// <summary>
    /// Classes for using Jdenticon from ASP.NET MVC.
    /// </summary>
    /// <example>
    /// <para>
    /// To use Jdenticon from ASP.NET MVC, install the <c>Jdenticon.AspNet.Mvc</c> NuGet package.
    /// </para>
    /// <code language="bat" title="NuGet Package Manager">
    /// PM&gt; Install-Package Jdenticon.AspNet.Mvc
    /// </code>
    /// <para>
    /// Jdenticon can be used in multiple ways in ASP.NET MVC. The first example shows how 
    /// to use the <see cref="HtmlHelper"/> extension methods to render an IMG element
    /// on the page.
    /// </para>
    /// <code language="html" title="HtmlHelper approach">
    /// @Html.Identicon("TestIcon", 60, alt: "Identicon")
    /// 
    /// is rendered as:
    /// &lt;img src="/identicon.axd?5AMA8Xyneag78XyneQ--" width="60" height="60" alt="Identicon" /&gt;
    /// </code>
    /// 
    /// <para>
    /// The following example shows how to use the <see cref="UrlHelper"/> extension methods for
    /// generating identicon urls.
    /// </para>
    /// <code language="html" title="UrlHelper approach">
    /// &lt;img src="@Url.Identicon("TestIcon", 60)" alt="Identicon" width="60" height="60" &gt;
    /// 
    /// is rendered as:
    /// &lt;img src="/identicon.axd?5AMA8Xyneag78XyneQ--" alt="Identicon" width="60" height="60" &gt;
    /// </code>
    /// <para>
    /// It is also possible to return an identicon as a result from a ASP.NET MVC controller. Note
    /// that the actual icon data is returned by the <see cref="IdenticonResult"/>. The other two
    /// approaches utilize urls handled by the <see cref="IdenticonHttpHandler"/>.
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
    /// </example>
    [CompilerGenerated]
    internal static class NamespaceDoc
    {
    }
}

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

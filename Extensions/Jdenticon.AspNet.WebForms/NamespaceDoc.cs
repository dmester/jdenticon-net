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
using System.Web;
using System.Web.UI.WebControls;

namespace Jdenticon.AspNet.WebForms
{
    /// <summary>
    /// Classes for using Jdenticon from ASP.NET WebForms. This namespace
    /// contains a web control to be used in .aspx pages and an <see cref="IHttpHandler"/>
    /// that serves icons.
    /// </summary>
    /// <example>
    /// <para>
    /// The <c>Jdenticon.AspNet.WebForms</c> assembly contains the <see cref="Icon"/> web control that 
    /// can be used on web pages. Install the assembly by using NuGet Package Manager.
    /// </para>
    /// <code language="bat" title="NuGet Package Manager">
    /// PM&gt; Install-Package Jdenticon.AspNet.WebForms
    /// </code>
    /// <para>
    /// This example shows a web page where an <see cref="Icon"/> 
    /// is data bound in a <see cref="Repeater"/>.
    /// </para>
    /// <code language="html" title="ASPX file">
    /// &lt;asp:Repeater ID="repeater" runat="server"&gt;
    ///     &lt;ItemTemplate&gt;
    ///         Value: &lt;asp:Literal runat="server" Text='&lt;%# Container.DataItem %&gt;' /&gt;: &lt;br/&gt;
    ///         &lt;jdenticon:Icon runat="server" Value='&lt;%# Container.DataItem %&gt;' Size="60" /&gt; &lt;br/&gt;&lt;br/&gt;
    ///     &lt;/ItemTemplate&gt;
    /// &lt;/asp:Repeater&gt;
    /// </code>
    /// <code language="cs" title="Code-behind file">
    /// protected void Page_Load(object sender, EventArgs e)
    /// {
    ///     repeater.DataSource = new object[]
    ///     {
    ///         123,
    ///         124,
    ///         "Item3",
    ///         "Item4"
    ///     };
    ///     repeater.DataBind();
    /// }
    /// </code>
    /// </example>
    [CompilerGenerated]
    internal static class NamespaceDoc
    {
    }
}

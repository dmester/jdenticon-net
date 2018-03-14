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

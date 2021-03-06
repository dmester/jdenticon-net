﻿#region License
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
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jdenticon.AspNet.WebForms
{
    /// <summary>
    /// Renders and displays an identicon on a web page.
    /// </summary>
    /// <example>
    /// In this example a static identicon is displayed on the page.
    /// <code language="html">
    /// &lt;jdenticon:Icon runat="server" StringValue="Example" Size="60" /&gt;
    /// </code>
    /// </example>
    /// <example>
    /// This example shows how to bind to the <see cref="Value"/> property in a 
    /// <see cref="Repeater"/>.
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
    public class Icon : Image
    {
        private byte[] compressedHash;
        private byte[] uncompressedHash;
        private object value;
        private int size;
        
        /// <summary>
        /// Gets or sets the format in which the icon will be generated. If no format is
        /// specified a PNG file is generated.
        /// </summary>
        public ExportImageFormat Format
        {
            get => (ExportImageFormat?)(ViewState["Format"] as int?) ?? ExportImageFormat.Png;
            set => ViewState["Format"] = (int)value;
        }
        
        /// <summary>
        /// Gets or sets the size of the generated icon in pixels. If no size is specified 
        /// a 64 pixel icon is generated.
        /// </summary>
        public virtual int Size
        {
            get
            {
                if (size == 0)
                {
                    size = ViewState["Size"] as int? ?? 64;
                }
                return size;
            }
            set
            {
                var size = value < 1 ? 1 : value;
                ViewState["Size"] = size;
            }
        }

        /// <inheritdoc />
        public override Unit Width
        {
            get => base.Width.IsEmpty ? new Unit(Size, UnitType.Pixel) : base.Width;
            set => base.Width = value; 
        }

        /// <inheritdoc />
        public override Unit Height
        {
            get => base.Height.IsEmpty ? new Unit(Size, UnitType.Pixel) : base.Height;
            set => base.Height = value;
        }

        /// <inheritdoc />
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string ImageUrl
        {
            get
            {
                var parameters = new IdenticonRequest
                {
                    Hash = HashCore ?? EmptyStringHash,
                    Size = Size,
                    Format = Format
                };

                var backColor = BackColor;
                if (!backColor.IsEmpty)
                {
                    parameters.Style.BackColor = Rendering.Color.FromArgb(
                        backColor.A, backColor.R, backColor.G, backColor.B);
                }

                return Page.Response.ApplyAppPathModifier("~/identicon.axd?" + parameters);
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or sets the value whose hash will be used as base for the icon. 
        /// </summary>
        /// <example>
        /// In this example a static identicon is displayed on the page.
        /// <code language="html">
        /// &lt;jdenticon:Icon runat="server" StringValue="Example" Size="60" /&gt;
        /// </code>
        /// </example>
        public string StringValue
        {
            get => Value as string;
            set => Value = value;
        }

        /// <summary>
        /// Gets or sets the value whose hash will be used as base for the icon. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// It is not possible to assign this property a literal on the control markup. However
        /// it is possible to bind to it via the control markup, or use it from the code-behind 
        /// file. It is possible to assign a string literal in the control markup by using the
        /// <see cref="StringValue"/> property.
        /// </para>
        /// </remarks>
        /// <example>
        /// This example shows how to bind to the <see cref="Value"/> property in a 
        /// <see cref="Repeater"/>.
        /// <code language="html" title="ASPX file">
        /// &lt;asp:Repeater ID="repeater" runat="server"&gt;
        ///     &lt;ItemTemplate&gt;
        ///         &lt;jdenticon:Icon runat="server" Value='&lt;%# Container.DataItem %&gt;' Size="60" /&gt;
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
        public virtual object Value
        {
            get { return value; }
            set
            {
                Hash = HashGenerator.ComputeHash(value, "SHA1");
                this.value = value;
            }
        }

        /// <summary>
        /// Gets or sets the hash that the icon will be based on.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The hash is stored in a compacted form in view state and is thus not 
        /// available on a postback. The compacted hash is enough to render the icon
        /// which will render also on postbacks.
        /// </para>
        /// </remarks>
        public virtual byte[] Hash
        {
            // Don't load the hash from view state as it might have been compressed. 
            // Returning another hash than what was set will probably confuse the user.
            get { return uncompressedHash; }
            set { HashCore = value; uncompressedHash = value; }
        }

        /// <summary>
        /// Gets or sets the compacted hash.
        /// </summary>
        protected virtual byte[] HashCore
        {
            get
            {
                if (compressedHash != null)
                {
                    var raw = ViewState["Hash"] as string;
                    if (raw != null)
                    {
                        compressedHash = Convert.FromBase64String(raw);
                    }
                }
                return compressedHash;
            }
            set
            {
                if (value == null)
                {
                    ViewState["Hash"] = null;
                    compressedHash = null;
                }
                else
                {
                    compressedHash = Identicon.FromHash(value, 100).Hash;
                    ViewState["Hash"] = Convert.ToBase64String(compressedHash);
                }
            }
        }

        /// <summary>
        /// This is the SHA1 hash of an empty string.
        /// </summary>
        private static byte[] EmptyStringHash
        {
            get => new byte[] 
            {
                0xda, 0x39, 0xa3, 0xee, 0x5e, 0x6b, 0x4b, 0x0d, 0x32, 0x55,
                0xbf, 0xef, 0x95, 0x60, 0x18, 0x90, 0xaf, 0xd8, 0x07, 0x09
            };
        }
    }
}

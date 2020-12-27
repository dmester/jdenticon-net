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

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Jdenticon.AspNetCore
{
    /// <summary>
    /// Tag helper used to display an identicon in &lt;img&gt; elements.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following attributes are available for this tag helper.
    /// </para>
    /// <list type="table">
    ///     <title>Tag helper attributes</title>
    ///     <listheader>
    ///         <term>Attribute name</term>
    ///         <term>Description</term>
    ///     </listheader>
    ///     <item>
    ///         <term><see cref="Value">identicon-value</see></term>
    ///         <term>Value that will be hashed using SHA1 and then used as base for this icon.</term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Hash">identicon-hash</see></term>
    ///         <term>Hash that will be used as base for this icon.</term>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Format">identicon-format</see></term>
    ///         <term>Format of the generated icon. This attribute is optional and defaults to PNG.</term>
    ///     </item>
    ///     <item>
    ///         <term>width</term>
    ///         <term>The width of the generated icon.</term>
    ///     </item>
    ///     <item>
    ///         <term>height</term>
    ///         <term>The height of the generated icon.</term>
    ///     </item>
    /// </list>
    /// <para>
    /// The size of the icon is determined from the specified width and height of the
    /// <c>&lt;img&gt;</c> tag.
    /// </para>
    /// <note type="note">
    /// Only square icons are supported. Specifying different width and height will end up in a distorted icon.
    /// </note>
    /// </remarks>
    /// <example>
    /// <para>
    /// In the following example <see cref="Value">identicon-value</see> is used
    /// to render an identicon for a user id.
    /// </para>
    /// <code language="html" title="Using identicon-value">
    /// @{
    ///     var userId = "TestIcon";
    /// }
    /// &lt;img identicon-value="userId" width="100" height="100" /&lt;
    /// </code>
    /// </example>
    [HtmlTargetElement("img", Attributes = IdenticonValueAttributeName)]
    [HtmlTargetElement("img", Attributes = IdenticonHashAttributeName)]
    public class IdenticonTagHelper : TagHelper
    {
        private byte[] hash;

        private const string IdenticonValueAttributeName = "identicon-value";
        private const string IdenticonHashAttributeName = "identicon-hash";
        private const string IdenticonFormatAttributeName = "identicon-format";

        /// <summary>
        /// Gets or sets the value that will be hashed using SHA1 and then used as base for this icon.
        /// </summary>
        /// <remarks>
        /// <para>
        /// <c>null</c> values for this property is allowed and handled as empty strings.
        /// </para>
        /// <para>
        /// If both <see cref="Hash">identicon-hash</see> and <see cref="Value">identicon-value</see>
        /// is specified, <see cref="Hash">identicon-hash</see> has precedence.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// In the following example <see cref="Value">identicon-value</see> is used
        /// to render an identicon for a user id.
        /// </para>
        /// <code language="html" title="Using identicon-value">
        /// @{
        ///     var userId = "TestIcon";
        /// }
        /// &lt;img identicon-value="userId" width="100" height="100" /&lt;
        /// </code>
        /// </example>
        [HtmlAttributeName(IdenticonValueAttributeName)]
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets a hash that will be used as base for this icon.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The hash must contain at least 6 bytes.
        /// </para>
        /// <para>
        /// If both <see cref="Hash">identicon-hash</see> and <see cref="Value">identicon-value</see>
        /// is specified, <see cref="Hash">identicon-hash</see> has precedence.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException"><paramref name="value"/> does not contain 6 bytes.</exception>
        /// <example>
        /// <para>
        /// In the following example <see cref="Hash">identicon-hash</see> is used
        /// to render an identicon for a user id.
        /// </para>
        /// <code language="html" title="Using identicon-hash">
        /// @{
        ///     var userIdHash = new byte[] { 242, 94, 85, 42, 87, 222, 222, 190, 155, 231, 226, 21, 195, 40, 18, 137 };
        /// }
        /// &lt;img identicon-hash="userIdHash" width="100" height="100" /&lt;
        /// </code>
        /// </example>
        [HtmlAttributeName(IdenticonHashAttributeName)]
        public byte[] Hash
        {
            get => hash;
            set => hash =
                value == null || value.Length >= 6 ? value :
                throw new ArgumentException($"The value of identicon-hash must contain at least 6 bytes.", nameof(value));
        }

        /// <summary>
        /// Gets or sets the format of the generated icon. This attribute is optional and defaults to PNG.
        /// </summary>
        /// <example>
        /// <para>
        /// In the following example <see cref="Format">identicon-format</see> is used
        /// to render SVG icons instead of PNG icons.
        /// </para>
        /// <code language="html" title="Using identicon-format">
        /// @{
        ///     var userId = "TestIcon";
        /// }
        /// &lt;img identicon-value="userId" identicon-format="Svg" width="100" height="100" /&lt;
        /// </code>
        /// </example>
        [HtmlAttributeName(IdenticonFormatAttributeName)]
        public ExportImageFormat Format { get; set; } = ExportImageFormat.Png;

        /// <exclude/>
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <exclude/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.RemoveAll(IdenticonValueAttributeName);
            output.Attributes.RemoveAll(IdenticonHashAttributeName);
            output.Attributes.RemoveAll(IdenticonFormatAttributeName);

            var sizeAttributes = new[] { "width", "height" };
            var size = sizeAttributes
                .Select(name =>
                {
                    var attribute = output.Attributes[name];

                    if (attribute != null && int.TryParse(
                        attribute.Value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var iWidth))
                    {
                        return (int?)iWidth;
                    }

                    return (int?)null;
                })
                .Where(value => value != null)
                .FirstOrDefault() ??
                100;

            var url = IdenticonUrl.Create(ViewContext.HttpContext, 
                hash: Hash ?? HashGenerator.ComputeHash(Value, "SHA1"),
                size: size,
                format: Format,
                style: null);

            output.Attributes.SetAttribute("src", url);
        }
    }
}

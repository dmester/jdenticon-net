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

using Jdenticon.Cryptography;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jdenticon.AspNet.WebForms
{
    /// <summary>
    /// Rendered as an identicon.
    /// </summary>
    public class Icon : Image
    {
        private byte[] compressedHash;
        private byte[] uncompressedHash;
        private object value;
        private int size;
        
        /// <summary>
        /// Gets or sets the format in which the icon will be generated. Default is PNG.
        /// </summary>
        public ExportImageFormat Format
        {
            get => (ExportImageFormat?)(ViewState["Format"] as int?) ?? ExportImageFormat.Png;
            set => ViewState["Format"] = (int)value;
        }
        
        /// <summary>
        /// Gets or sets the size of the generated icon in pixels. Default is 64 pixels.
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

                return Page.Response.ApplyAppPathModifier("~/identicon.axd?" + parameters);
            }
            set
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or sets the value whose hash will be used as base for the icon. This property
        /// can be used from the aspx code editor as object properties cannot be set that way.
        /// </summary>
        public string StringValue
        {
            get => Value as string;
            set => Value = value;
        }

        /// <summary>
        /// Gets or sets the value whose hash will be used as base for the icon. 
        /// </summary>
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
        public virtual byte[] Hash
        {
            // Don't load the hash from view state as it might have been compressed. 
            // Returning another hash than what was set will probably confuse the user.
            get { return uncompressedHash; }
            set { HashCore = value; uncompressedHash = value; }
        }

        /// <summary>
        /// Gets or sets the compressed hash.
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
                    compressedHash = Identicon.FromHash(value).Hash;
                    ViewState["Hash"] = Convert.ToBase64String(compressedHash);
                }
            }
        }

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

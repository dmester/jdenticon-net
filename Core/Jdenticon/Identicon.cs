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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon
{
    /// <summary>
    /// Represents an identicon for a specific hash.
    /// </summary>
    /// <remarks>
    /// <note type="note">
    ///     <para>
    ///     The operations to save and export the icon are implemented as extension methods
    ///     located in the <see cref="Jdenticon"/> namespace.
    ///     </para>
    /// </note>
    /// </remarks>
    /// <example>
    /// <para>
    /// Before using <see cref="Identicon"/> the <c>Jdenticon-net</c> package needs to be added to 
    /// the project. Use the NuGet Package Manager to add a reference.
    /// </para>
    /// <code language="bat" title="NuGet Package Manager">
    /// PM&gt; Install-Package Jdenticon-net
    /// </code>
    /// <para>
    /// The following code illustrates how to use Jdenticon to generate an icon from a string 
    /// and save it as a PNG image. Note that all save operations are implemented as
    /// extension methods, which means a using for the <see cref="Jdenticon"/> namespace
    /// is required.
    /// </para>
    /// <code language="cs" title="Generate identicon as PNG">
    /// using Jdenticon;
    /// // ...
    /// Identicon
    ///     .FromValue("string to hash", size: 160)
    ///     .SaveAsPng("test.png");
    /// </code>
    /// </example>
    public class Identicon
    {
        private byte[] hash;
        private int size;
        private IconGenerator iconGenerator;
        private IdenticonStyle style;
        private static IdenticonStyle defaultStyle = new IdenticonStyle();

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 6 bytes.</param>
        /// <param name="size">The size of the icon in pixels (the icon is quadratic).</param>
        /// <exception cref="ArgumentException"><paramref name="hash"/> does not contain 6 bytes.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is less than 1 pixel.</exception>
        public Identicon(byte[] hash, int size)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if (hash.Length < 6) throw new ArgumentException(nameof(hash), 
                "The hash array was too short. At least 6 bytes are required.");
            if (size < 1) throw new ArgumentOutOfRangeException(nameof(size), size, 
                "The size should be 1 pixel or larger.");

            this.size = size;

            // Remove parts of hash that should not be used, as
            // some of the extensions want to keep the size of the 
            // hash down.
            if (hash.Length <= 10)
            {
                this.hash = new byte[hash.Length];
                Buffer.BlockCopy(hash, 0, this.hash, 0, hash.Length);
            }
            else
            {
                this.hash = new byte[10];
                Buffer.BlockCopy(hash, 0, this.hash, 0, 6);
                Buffer.BlockCopy(hash, hash.Length - 4, this.hash, this.hash.Length - 4, 4);
            }
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance from a specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 6 bytes.</param>
        /// <param name="size">The size of the icon in pixels (the icon is quadratic).</param>
        /// <exception cref="ArgumentException"><paramref name="hash"/> does not contain 6 bytes.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is less than 1 pixel.</exception>
        /// <returns>An <see cref="Identicon"/> instance for the specified hash.</returns>
        public static Identicon FromHash(byte[] hash, int size)
        {
            return new Identicon(hash, size);
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance from a hexadecimal hash string.
        /// </summary>
        /// <param name="hash">The hex encoded hash that will be used as base for the icon. The hash string must contain at least 12 characters.</param>
        /// <param name="size">The size of the icon in pixels (the icon is quadratic).</param>
        /// <exception cref="ArgumentException"><paramref name="hash"/> does not contain 6 bytes.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="hash"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is less than 1 pixel.</exception>
        /// <exception cref="FormatException"><paramref name="hash"/> is not a hexadecimal string.</exception>
        /// <returns>An <see cref="Identicon"/> instance for the specified hash.</returns>
        public static Identicon FromHash(string hash, int size)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            return new Identicon(HexString.ToArray(hash), size);
        }

#pragma warning disable CS1573
        /// <inheritdoc cref="HashGenerator.ComputeHash(object, string)" />
        /// <summary>
        /// Generates a hash for a specified value and creates an <see cref="Identicon"/> instance from the generated hash.
        /// </summary>
        /// <param name="size">The size of the icon in pixels (the icon is quadratic).</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="size"/> is less than 1 pixel.</exception>
        /// <returns>An <see cref="Identicon"/> instance for the hash of <paramref name="value"/>.</returns>
        public static Identicon FromValue(object value, int size, string hashAlgorithmName = "SHA1")
        {
            return new Identicon(HashGenerator.ComputeHash(value, hashAlgorithmName), size);
        }
#pragma warning restore CS1573

        /// <summary>
        /// Gets or sets the size of the icon.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value is less than 1 pixel.</exception>
        public int Size
        {
            get { return size; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(Size), 
                        value, "The size should be 1 pixel or larger.");
                }
                size = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the <see cref="Jdenticon.Rendering.IconGenerator"/> used to generate icons.
        /// </summary>
        public IconGenerator IconGenerator
        {
            get
            {
                if (iconGenerator == null) iconGenerator = new IconGenerator();
                return iconGenerator;
            }
            set { iconGenerator = value; }
        }

        /// <summary>
        /// Gets or sets the style of the icon.
        /// </summary>
        /// <remarks>
        /// This property will be initialized with a clone of the <see cref="DefaultStyle"/>.
        /// </remarks>
        public IdenticonStyle Style
        {
            get
            {
                if (style == null) style = DefaultStyle.Clone();
                return style;
            }
            set { style = value; }
        }

        /// <summary>
        /// Gets or sets the default style that will be used for all <see cref="Identicon"/> 
        /// instances where no explicit style has been specified.
        /// </summary>
        /// <remarks>
        /// When rendering a new <see cref="Identicon"/> this style will be applied, 
        /// if no explicit style has been specified on the icon. This specific <see cref="IdenticonStyle"/> 
        /// instance is never exposed directly by <see cref="Identicon"/> instances, but is always cloned
        /// before exposed.
        /// </remarks>
        public static IdenticonStyle DefaultStyle
        {
            get => defaultStyle;
            set => defaultStyle = value ?? new IdenticonStyle();
        }

        /// <summary>
        /// Draws this icon using a specified renderer.
        /// </summary>
        /// <param name="renderer">The renderer used to render this icon.</param>
        /// <param name="rect">The bounds of the rendered icon. No padding will be applied to the rectangle.</param>
        /// <remarks>
        /// This method is only intended for usage with custom renderers. A custom renderer could as an example 
        /// render an <see cref="Identicon"/> in a file format not natively supported by Jdenticon. To implement
        /// a new file format, implement the abstract <see cref="Renderer"/> class.
        /// </remarks>
        public void Draw(Renderer renderer, Rectangle rect)
        {
            IconGenerator.Generate(renderer, rect, Style, hash);
        }

        /// <summary>
        /// Gets the hash that is used as base for this icon.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <see cref="Hash"/> property always returns a copy of its internal byte array to avoid accidental 
        /// changes to the icon.
        /// </para>
        /// <para>
        /// This property exposes the internally stored compacted hash. If the hash that was used to construct the
        /// <see cref="Identicon"/> was longer than 10 byte, it has been shortened to 10 byte.
        /// </para>
        /// </remarks>
        public byte[] Hash
        {
            get
            {
                var clone = new byte[hash.Length];
                Buffer.BlockCopy(hash, 0, clone, 0, hash.Length);
                return clone;
            }
        }

        /// <summary>
        /// Gets the bounds of the icon excluding its padding.
        /// </summary>
        public Rectangle GetIconBounds()
        {
            // Round to nearest integer
            var padding = (int)(Style.Padding * size + 0.5f);
            
            return new Rectangle(
                padding, padding,
                size - padding * 2, size - padding * 2);
        }

        /// <summary>
        /// Gets a string representation of this <see cref="Identicon"/>.
        /// </summary>
        public override string ToString()
        {
            return "Identicon: " + HexString.ToString(hash);
        }
    }
}


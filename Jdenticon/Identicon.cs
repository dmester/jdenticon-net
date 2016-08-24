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

using Jdenticon.IO;
using Jdenticon.Rendering;
using Jdenticon.Rendering.GdiPlus;
using Jdenticon.Rendering.Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon
{
    /// <summary>
    /// Represents an identicon for a specific hash.
    /// </summary>
    public class Identicon
    {
        private byte[] hash;
        private float padding = 0.08f;
        private IconGenerator iconGenerator = new IconGenerator();
        private IdenticonStyle style = new IdenticonStyle();
        
        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 11 bytes.</param>
        public Identicon(byte[] hash)
        {
            if (hash == null) throw new ArgumentNullException("hash");
            if (hash.Length < 6) throw new ArgumentException("hash", "The hash array was too short. At least 6 bytes are required.");

            this.hash = hash;
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 11 bytes.</param>
        public static Identicon FromHash(byte[] hash)
        {
            return new Identicon(hash);
        }

        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with the specified hash.
        /// </summary>
        /// <param name="hash">The hash that will be used as base for this icon. The hash must contain at least 11 bytes.</param>
        public static Identicon FromHash(string hash)
        {
            return new Identicon(HexString.ToArray(hash));
        }
        
        /// <summary>
        /// Creates an <see cref="Identicon"/> instance with a hash of the specified object.
        /// </summary>
        /// <param name="value">The string representation of this object will be hashed and used as base for this icon.</param>
        /// <param name="hashAlgorithmName">The name of the hash algorithm to use for hashing.</param>
        public static Identicon FromValue(object value, string hashAlgorithmName = "SHA1")
        {
            return new Identicon(HashUtils.ComputeHash(value, hashAlgorithmName));
        }

        /// <summary>
        /// Gets or sets the padding between the edge of the image and the bounds of the rendered icon. The value is specified in percent in the range [0.0, 0.4].
        /// </summary>
        public float Padding
        {
            get { return padding; }
            set
            {
                if (padding < 0f || padding > 0.4f) throw new ArgumentOutOfRangeException("Padding", "Only padding values in the range [0.0, 0.4] are valid.");
                padding = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the <see cref="Jdenticon.Rendering.IconGenerator"/> used to generate icons.
        /// </summary>
        public IconGenerator IconGenerator
        {
            get { return iconGenerator; }
            set { iconGenerator = value ?? new IconGenerator(); }
        }
        
        /// <summary>
        /// Gets or sets the style of the generated icon.
        /// </summary>
        public IdenticonStyle Style
        {
            get { return style; }
            set { style = value ?? new IdenticonStyle(); }
        }
        
        /// <summary>
        /// Draws this icon in the specified drawing context.
        /// </summary>
        /// <param name="g">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon. No padding will be applied to the rectangle.</param>
        public void Draw(Graphics g, Rectangle rect)
        {
            var renderer = new GdiPlusRenderer(g);
            iconGenerator.Generate(renderer, rect, Style, hash);
        }

        private void GenerateSvg(int size, TextWriter writer, bool fragment)
        {
            var iconBounds = GetIconBounds(size);
            var renderer = new SvgRenderer(size, size);
            iconGenerator.Generate(renderer, iconBounds, Style, hash);
            renderer.Save(writer, fragment);
        }

        private Rectangle GetIconBounds(int size)
        {
            return new Rectangle(
                (int)(padding * size),
                (int)(padding * size),
                size - (int)(padding * size) * 2,
                size - (int)(padding * size) * 2);
        }

        /// <summary>
        /// Creates a bitmap icon.
        /// </summary>
        /// <param name="size">The size of the generated bitmap in pixels.</param>
        public Bitmap ToBitmap(int size)
        {
            if (size < 30) throw new ArgumentOutOfRangeException("size", size, "The size was too small. Only sizes greater than or equal to 30 pixels are supported.");

            var iconBounds = GetIconBounds(size);     
            var img = new Bitmap(size, size);
            try
            {
                using (var g = Graphics.FromImage(img))
                {
                    Draw(g, iconBounds);
                }

                return img;
            }
            catch
            {
                img.Dispose();
                throw;
            }
        }

        private void ToMetafile(Stream stream, int size)
        {
            var iconBounds = GetIconBounds(size);

            using (var desktopGraphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                var hdc = desktopGraphics.GetHdc();
                try
                {
                    using (var img = new Metafile(stream, hdc, 
                        new Rectangle(0, 0, size, size), MetafileFrameUnit.Pixel, 
                        EmfType.EmfPlusDual))
                    {
                        using (var graphics = Graphics.FromImage(img))
                        {
                            Draw(graphics, iconBounds);
                        }
                    }
                }
                finally
                {
                    desktopGraphics.ReleaseHdc(hdc);
                }
            }
        }

        /// <summary>
        /// Saves this icon to the specified stream.
        /// </summary>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="stream">The stream to which the icon is written.</param>
        /// <param name="format">The image format of the generated icon.</param>
        public void Save(int size, Stream stream, ExportImageFormat format)
        {
            if (size < 30) throw new ArgumentOutOfRangeException("size", size, "The size was too small. Only sizes greater than or equal to 30 pixels are supported.");
            if (stream == null) throw new ArgumentNullException("stream");

            if (format == ExportImageFormat.Png)
            {
                using (var img = ToBitmap(size))
                {
                    img.Save(stream, ImageFormat.Png);
                }
            }
            else if (format == ExportImageFormat.Emf)
            {
                ToMetafile(stream, size);
            }
            else 
            {
                // The LeaveOpenStream wrapper is needed for .NET 4.0 compatibility, since the
                // .NET 4.0 version of StreamWriter does not have an option for keeping the
                // stream open when the writer itself is disposed.
                using (var leaveOpenStream = new LeaveOpenStream(stream))
                {
                    using (var writer = new StreamWriter(leaveOpenStream, Encoding.UTF8))
                    {
                        GenerateSvg(size, writer, format == ExportImageFormat.SvgFragment);
                    }
                }
            }
        }

        /// <summary>
        /// Saves this icon to a specified file.
        /// </summary>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="path">The path to the file to which the icon will be written.</param>
        /// <param name="format">The image format of the generated icon.</param>
        public void Save(int size, string path, ExportImageFormat format)
        {
            if (size < 30) throw new ArgumentOutOfRangeException("size", size, "The size was too small. Only sizes greater than or equal to 30 pixels are supported.");
            if (path == null) throw new ArgumentNullException("path");

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                Save(size, stream, format);
            }
        }

        /// <summary>
        /// Saves this icon to a specified file. The format is automatically determined from the file name extension.
        /// </summary>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="path">The path to the file to which the icon will be written.</param>
        public void Save(int size, string path)
        {
            if (size < 30) throw new ArgumentOutOfRangeException("size", size, "The size was too small. Only sizes greater than or equal to 30 pixels are supported.");
            if (path == null) throw new ArgumentNullException("path");

            ExportImageFormat format;

            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Could not automatically determine a file format. No file name extension was specified.", "path");
            }

            switch (extension.ToLowerInvariant())
            {
                case ".svg":
                    format = ExportImageFormat.Svg;
                    break;
                case ".emf":
                    format = ExportImageFormat.Emf;
                    break;
                case ".png":
                    format = ExportImageFormat.Png;
                    break;
                default:
                    throw new ArgumentException("Could not automatically determine a file format for the extension '" + extension + "'.", "path");
            }

            using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                Save(size, stream, format);
            }
        }

        /// <summary>
        /// Creates a string containing an SVG version of this icon.
        /// </summary>
        /// <param name="size">The size of the generated icon in pixels.</param>
        /// <param name="fragment">
        /// If <c>true</c> the generated SVG will not be encapsulated in the root svg element making 
        /// it suitable to be embedded in another SVG.
        /// </param>
        public string ToSvg(int size, bool fragment)
        {
            using (var writer = new StringWriter())
            {
                GenerateSvg(size, writer, fragment);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Creates a string containing an SVG version of this icon.
        /// </summary>
        /// <param name="size">The size of the generated icon in pixels.</param>
        public string ToSvg(int size)
        {
            return ToSvg(size, false);
        }
        
        /// <summary>
        /// Gets a string representation of this <see cref="Identicon"/> .
        /// </summary>
        public override string ToString()
        {
            return "Identicon: " + HexString.ToString(hash);
        }
    }
}


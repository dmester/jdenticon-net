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
using System.Globalization;
using System.IO;
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Renders icons as SVG paths.
    /// </summary>
    public class SvgRenderer : Renderer
    {
        private Dictionary<Color, SvgPath> pathsByColor = new Dictionary<Color, SvgPath>();
        private SvgPath path;
        private int width, height;
        private Color backColor;

        /// <summary>
        /// Creates an instance of <see cref="SvgRenderer"/>.
        /// </summary>
        /// <param name="width">The width of the icon in pixels.</param>
        /// <param name="height">The height of the icon in pixels.</param>
        public SvgRenderer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <inheritdoc />
        protected override void AddCircleNoTransform(PointF location, float diameter, bool counterClockwise)
        {
            path.AddCircle(location, diameter, counterClockwise);
        }

        /// <inheritdoc />
        protected override void AddPolygonNoTransform(PointF[] points)
        {
            path.AddPolygon(points);
        }

        /// <inheritdoc />
        public override void SetBackground(Color color)
        {
            backColor = color;
        }

        /// <inheritdoc />
        public override IDisposable BeginShape(Color color)
        {
            if (!pathsByColor.TryGetValue(color, out path))
            {
                pathsByColor[color] = path = new SvgPath();
            }

            return ActionDisposable.Empty;
        }

        /// <summary>
        /// Writes the SVG to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="writer">The output writer to which the SVG will be written.</param>
        /// <param name="fragment">If <c>true</c> an SVG string without the root svg element will be rendered.</param>
        public void Save(TextWriter writer, bool fragment)
        {
            writer.Write(ToSvg(fragment));
        }

        /// <summary>
        /// Writes the SVG to the specified <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="fragment">If <c>true</c> an SVG string without the root svg element will be rendered.</param>
        public string ToSvg(bool fragment)
        {
            var svg = new List<string>();

            var invariantCulture = CultureInfo.InvariantCulture;
            var widthAsString = width.ToString(invariantCulture);
            var heightAsString = height.ToString(invariantCulture);
            
            if (!fragment)
            {
                svg.Add("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"" +
                    widthAsString + "\" height=\"" + heightAsString + "\" viewBox=\"0 0 " +
                    widthAsString + " " + heightAsString + "\" preserveAspectRatio=\"xMidYMid meet\">");
            }

            if (backColor.A > 0)
            {
                var opacity = (float)backColor.A / 255;
                svg.Add("<rect fill=\"" + ColorUtils.ToHexString(backColor) + "\" fill-opacity=\"" +
                    opacity.ToString(invariantCulture) +
                    "\" x=\"0\" y=\"0\" width=\"" + widthAsString + "\" height=\"" + heightAsString + "\"/>");
            }
            
            foreach (var pair in pathsByColor)
            {
                svg.Add("<path fill=\"" + ColorUtils.ToHexString(pair.Key) + "\" d=\"" + pair.Value + "\"/>");
            }

            if (!fragment)
            {
                svg.Add("</svg>");
            }

            return string.Concat(svg);
        }
    }
}

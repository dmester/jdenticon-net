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

using Jdenticon.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Renders icons as PNG using the internal vector rasterizer.
    /// </summary>
    public class PngRenderer : Renderer
    {
        private Bitmap bitmap;
        private Drawing.Path path;

        /// <summary>
        /// Creates an instance of the class <see cref="PngRenderer"/>.
        /// </summary>
        /// <param name="width">The width of the icon in pixels.</param>
        /// <param name="height">The height of the icon in pixels.</param>
        public PngRenderer(int width, int height)
        {
            bitmap = new Bitmap(width, height);
        }

        /// <inheritdoc />
        protected override void AddCircleNoTransform(PointF location, float diameter, bool counterClockwise)
        {
            path.AddCircle(
                new PointF(
                    location.X + diameter / 2, location.Y + diameter / 2
                    ),
                diameter / 2,
                !counterClockwise);
        }

        /// <inheritdoc />
        protected override void AddPolygonNoTransform(PointF[] points)
        {
            path.AddPolygon(points);
        }

        /// <inheritdoc />
        public override void SetBackground(Color color, Rectangle iconBounds)
        {
            bitmap.BackgroundColor = color;
        }

        /// <inheritdoc />
        public override IDisposable BeginShape(Color color)
        {
            path = new Drawing.Path();

            return new ActionDisposable(() =>
            {
                bitmap.FillPath(color, path);
            });
        }

        /// <summary>
        /// Saves the rendered icon as a PNG stream.
        /// </summary>
        /// <param name="stream">Stream to write PNG data to.</param>
        public void SavePng(Stream stream)
        {
            bitmap.SaveAsPng(stream);
        }
    }
}

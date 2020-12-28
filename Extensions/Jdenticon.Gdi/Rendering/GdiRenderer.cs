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

using Jdenticon.Gdi.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Renders icons to a GDI+ <see cref="Graphics"/> drawing context.
    /// </summary>
    public class GdiRenderer : Renderer
    {
        private readonly Graphics graphics;
        private readonly Dictionary<Color, GraphicsPath> pathsByColor = new Dictionary<Color, GraphicsPath>();
        private GraphicsPath path;

        /// <summary>
        /// Creates an instance of the class <see cref="GdiRenderer"/>.
        /// </summary>
        /// <param name="graphics">GDI+ drawing context to which the icon will be rendered.</param>
        public GdiRenderer(Graphics graphics)
        {
            this.graphics = graphics;
        }

        /// <inheritdoc />
        public override void Flush()
        {
            var state = graphics.Save();
            try
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                foreach (var path in pathsByColor)
                {
                    using (var brush = new SolidBrush(path.Key.ToGdi()))
                    {
                        graphics.FillPath(brush, path.Value);
                    }

                    path.Value.Dispose();
                }

                pathsByColor.Clear();
            }
            finally
            {
                graphics.Restore(state);
            }
        }

        /// <inheritdoc />
        protected override void AddCircleNoTransform(PointF location, float diameter, bool counterClockwise)
        {
            var rect = new RectangleF(location.ToGdi(), new SizeF(diameter, diameter));
            var angle = counterClockwise ? -360f : 360f;
            path.AddArc(rect, 0, angle);
            path.CloseFigure();
        }

        /// <inheritdoc />
        protected override void AddPolygonNoTransform(PointF[] points)
        {
            path.AddPolygon(points.ToGdi());
        }

        /// <inheritdoc />
        public override void SetBackground(Color color)
        {
            graphics.Clear(color.ToGdi());
        }

        /// <inheritdoc />
        public override IDisposable BeginShape(Color color)
        {
            if (!pathsByColor.TryGetValue(color, out path))
            {
                pathsByColor[color] = path = new GraphicsPath(FillMode.Alternate);
            }

            return ActionDisposable.Empty;
        }
    }
}

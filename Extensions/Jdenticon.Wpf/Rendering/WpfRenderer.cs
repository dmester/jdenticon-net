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

using Jdenticon.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using JdenticonColor = Jdenticon.Rendering.Color;
using WpfColor = System.Windows.Media.Color;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Renders icons to a WPF <see cref="DrawingContext"/>.
    /// </summary>
    public class WpfRenderer : Renderer
    {
        private int width, height;
        private DrawingContext context;
        private PathGeometry currentPath;

        /// <summary>
        /// Creates an instance of the class <see cref="WpfRenderer"/>.
        /// </summary>
        /// <param name="context">The drawing context to which the icon will be rendered.</param>
        /// <param name="width">Width of the drawing surface in pixels.</param>
        /// <param name="height">Height of the drawing surface in pixels.</param>
        public WpfRenderer(DrawingContext context, int width, int height)
        {
            this.context = context;
            this.width = width;
            this.height = height;
        }

        /// <inheritdoc />
        public override IDisposable BeginShape(JdenticonColor color)
        {
            currentPath = new PathGeometry
            {
                FillRule = FillRule.Nonzero
            };
            
            return new ActionDisposable(() =>
            {
                context.DrawGeometry(
                    brush: new SolidColorBrush(color.ToWpfColor()),
                    pen: null,
                    geometry: currentPath);
                currentPath = null;
            });
        }

        /// <inheritdoc />
        public override void SetBackground(JdenticonColor color)
        {
            if (color.A > 0)
            {
                context.DrawRectangle(
                    brush: new SolidColorBrush(color.ToWpfColor()),
                    pen: null,
                    rectangle: new Rect(0, 0, width, height));
            }
        }

        /// <inheritdoc />
        protected override void AddCircleNoTransform(PointF location, float diameter, bool counterClockwise)
        {
            // If the start and end positions are the same, there won't be any circle rendered. 
            // Create an offset of 0.001 pixels between the positions to force a full circle.
            var dx = counterClockwise ? 0.001 : -0.001;

            var startPoint = new Point(location.X + diameter / 2 - dx, location.Y);
            var endPoint = new Point(location.X + diameter / 2 + dx, location.Y);
            var size = new Size(diameter / 2, diameter / 2);

            var arc = new ArcSegment(
                point: endPoint, 
                size: size,
                rotationAngle: 360, 
                isLargeArc: true, 
                sweepDirection: counterClockwise ? SweepDirection.Counterclockwise : SweepDirection.Clockwise, 
                isStroked: true);

            var figure = new PathFigure(
                start: startPoint, 
                segments: new[] { arc }, 
                closed: true);
            
            currentPath.Figures.Add(figure);
        }

        /// <inheritdoc />
        protected override void AddPolygonNoTransform(PointF[] points)
        {
            var startPoint = new Point(points[0].X, points[0].Y);
            var segments = points
                .Skip(1)
                .Select(p =>
                    new LineSegment(new Point(p.X, p.Y), true));
            
            var figure = new PathFigure(
                start: startPoint,
                segments: segments, 
                closed: true);

            currentPath.Figures.Add(figure);
        }
    }
}

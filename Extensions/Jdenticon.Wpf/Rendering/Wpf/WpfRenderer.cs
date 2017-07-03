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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using JdenticonColor = Jdenticon.Rendering.Color;
using WpfColor = System.Windows.Media.Color;

namespace Jdenticon.Rendering.Wpf
{
    /// <summary>
    /// Renders icons to a Wpf <see cref="DrawingContext"/>.
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

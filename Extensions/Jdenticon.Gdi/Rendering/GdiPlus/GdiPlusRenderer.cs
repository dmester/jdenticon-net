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

using Jdenticon.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;

namespace Jdenticon.Rendering.GdiPlus
{
    /// <summary>
    /// Renders icons to a GDI+ <see cref="Graphics"/> drawing context.
    /// </summary>
    public class GdiPlusRenderer : Renderer
    {
        private GraphicsPath path;
        private Graphics graphics;

        /// <summary>
        /// Creates an instance of the class <see cref="GdiPlusRenderer"/>.
        /// </summary>
        /// <param name="graphics">GDI+ drawing context to which the icon will be rendered.</param>
        public GdiPlusRenderer(Graphics graphics)
        {
            this.graphics = graphics;
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
            var localPath = new GraphicsPath(FillMode.Alternate);
            this.path = localPath;

            return new ActionDisposable(() =>
            {
                Interlocked.CompareExchange(ref this.path, null, localPath);

                var state = graphics.Save();
                try
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                    using (var brush = new SolidBrush(color.ToGdi()))
                    {
                        graphics.FillPath(brush, localPath);
                    }
                }
                finally
                {
                    graphics.Restore(state);
                    localPath.Dispose();
                }
            });
        }
    }
}

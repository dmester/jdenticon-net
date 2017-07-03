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

using Jdenticon.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon.Rendering.Png
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
        public override void SetBackground(Color color)
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

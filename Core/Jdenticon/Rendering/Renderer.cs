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
using System.Text;

namespace Jdenticon.Rendering
{
    /// <summary>
    /// Base class for rendering shapes in an identicon.
    /// </summary>
    /// <remarks>
    /// Implement this class to e.g. support a new file format that is not natively supported by
    /// Jdenticon. To invoke the new <see cref="Renderer"/>, pass the renderer as an argument to the 
    /// <see cref="Identicon.Draw(Renderer, Rectangle)"/> method.
    /// </remarks>
    public abstract class Renderer
    {
        private Transform transform = Transform.Empty;
        
        /// <summary>
        /// Gets or sets the current transform that will be applied on all coordinates before being rendered
        /// in the target image.
        /// </summary>
        internal Transform Transform
        {
            get { return transform; }
            set { transform = value ?? Transform.Empty; }
        }

        /// <summary>
        /// Adds a polygon without translating or changing direction of the points.
        /// </summary>
        /// <param name="points">The points that the polygon consists of.</param>
        protected abstract void AddPolygonNoTransform(PointF[] points);

        /// <summary>
        /// Adds a circle without translating of its border.
        /// </summary>
        /// <param name="location">The upper-left position of the bounding rectangle.</param>
        /// <param name="diameter">The diameter of the circle.</param>
        /// <param name="counterClockwise">If <c>true</c> the cirlce will be drawn counter clockwise.</param>
        protected abstract void AddCircleNoTransform(PointF location, float diameter, bool counterClockwise);

        /// <summary>
        /// Sets the background color of the image.
        /// </summary>
        /// <param name="color">New background color.</param>
        /// <param name="iconBounds">The icon bounds.</param>
        public abstract void SetBackground(Color color, Rectangle iconBounds);

        /// <summary>
        /// Begins a new shape. The shape should always be ended by disposing the 
        /// returned <see cref="IDisposable"/>.
        /// </summary>
        /// <param name="color">The fill color of the shape.</param>
        public abstract IDisposable BeginShape(Color color);

        private void AddPolygonCore(PointF[] points, bool invert)
        {
            if (invert)
            {
                Array.Reverse(points);
            }

            for (var i = 0; i < points.Length; i++)
            {
                points[i] = transform.TransformPoint(points[i].X, points[i].Y);
            }

            AddPolygonNoTransform(points);
        }

        /// <summary>
        /// Adds a rectangle to the image.
        /// </summary>
        /// <param name="x">X coordinate of the rectangle.</param>
        /// <param name="y">Y coordinate of the rectangle.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="invert">If <c>true</c> the area of the rectangle will be removed from the filled area.</param>
        public void AddRectangle(float x, float y, float width, float height, bool invert = false)
        {
            AddPolygonCore(new[]
            {
                new PointF(x, y),
                new PointF(x + width, y),
                new PointF(x + width, y + height),
                new PointF(x, y + height),
            }, invert);
        }

        /// <summary>
        /// Adds a circle to the image.
        /// </summary>
        /// <param name="x">X coordinate of the bounding rectangle.</param>
        /// <param name="y">Y coordinate of the bounding rectangle.</param>
        /// <param name="size">Width and height of the bounding rectangle.</param>
        /// <param name="invert">If <c>true</c> the area of the circle will be removed from the filled area.</param>
        public void AddCircle(float x, float y, float size, bool invert = false)
        {
            var northWest = transform.TransformPoint(x, y, size, size);
            AddCircleNoTransform(northWest, size, invert);
        }

        /// <summary>
        /// Adds a polygon to the image.
        /// </summary>
        /// <param name="points">List of points that the polygon consists of.</param>
        /// <param name="invert">If <c>true</c> the area of the polygon will be removed from the filled area.</param>
        public void AddPolygon(PointF[] points, bool invert = false)
        {
            AddPolygonCore((PointF[])points.Clone(), invert);
        }

        /// <summary>
        /// Adds a triangle to the image.
        /// </summary>
        /// <param name="x">X coordinate of the bounding rectangle.</param>
        /// <param name="y">Y coordinate of the bounding rectangle.</param>
        /// <param name="width">Width of the bounding rectangle.</param>
        /// <param name="height">Height of the bounding rectangle.</param>
        /// <param name="direction">The direction of the triangle.</param>
        /// <param name="invert">If <c>true</c> the area of the triangle will be removed from the filled area.</param>
        public void AddTriangle(float x, float y, float width, float height, TriangleDirection direction, bool invert = false)
        {
            var points = new List<PointF>(4)
            {
                new PointF(x + width, y),
                new PointF(x + width, y + height),
                new PointF(x, y + height),
                new PointF(x, y)
            };

            points.RemoveAt((int)direction);

            AddPolygonCore(points.ToArray(), invert);
        }

        /// <summary>
        /// Adds a rhombus to the image.
        /// </summary>
        /// <param name="x">X coordinate of the bounding rectangle.</param>
        /// <param name="y">Y coordinate of the bounding rectangle.</param>
        /// <param name="width">Width of the bounding rectangle.</param>
        /// <param name="height">Height of the bounding rectangle.</param>
        /// <param name="invert">If <c>true</c> the area of the rhombus will be removed from the filled area.</param>
        public void AddRhombus(float x, float y, float width, float height, bool invert = false)
        {
            AddPolygonCore(new[]
            {
                new PointF(x + width / 2, y),
                new PointF(x + width, y + height / 2),
                new PointF(x + width / 2, y + height),
                new PointF(x, y + height / 2),
            }, invert);
        }

        /// <summary>
        /// Flushes all pending draw operations to the target.
        /// </summary>
        public virtual void Flush()
        {
        }
    }
}

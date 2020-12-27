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

using Jdenticon.Drawing.Png;
using Jdenticon.Drawing.Rasterization;
using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Jdenticon.Drawing
{
    /// <summary>
    /// A canvas where vector graphics can be rasterized.
    /// </summary>
    internal class Bitmap
    {
        private EdgeTable edges;
        private int width;
        private int height;
        private int nextID = 1;
        
        /// <summary>
        /// Creates a new instance of <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="width">Width of the drawing canvas in pixels.</param>
        /// <param name="height"></param>
        public Bitmap(int width, int height)
        {
            this.edges = new EdgeTable(width, height);
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Gets or sets the background color that will be drawn behind all 
        /// polygons. Set to <see cref="Color.Transparent"/> to disable the
        /// background.
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets the width of the canvas in pixels.
        /// </summary>
        public int Width => width;

        /// <summary>
        /// Gets the height of the canvas in pixels.
        /// </summary>
        public int Height => height;

        private void FillPolygonCore(int id, Color color, PointF[] points)
        {
            for (var i = 1; i < points.Length; i++)
            {
                edges.Add(new Edge(id, points[i - 1], points[i], color));
            }

            // Close polygon
            if (points[0].X != points[points.Length - 1].X ||
                points[0].Y != points[points.Length - 1].Y)
            {
                edges.Add(new Edge(id, points[points.Length - 1], points[0], color));
            }
        }

        /// <summary>
        /// Fills the specified path with a color.
        /// </summary>
        /// <param name="color">The fill color.</param>
        /// <param name="path">The path to be filled.</param>
        public void FillPath(Color color, Path path)
        {
            var id = nextID++;

            foreach (var polygon in path.Polygons)
            {
                FillPolygonCore(id, color, polygon);
            }
        }

        /// <summary>
        /// Fills the specified polygon with a color.
        /// </summary>
        /// <param name="color">The fill color.</param>
        /// <param name="points">The location of all corners of the polygon. The polygon is rendered using the non-zero winding rule.</param>
        public void FillPolygon(Color color, PointF[] points)
        {
            FillPolygonCore(nextID++, color, points);
        }

        /// <summary>
        /// Fills the specified circle with a color.
        /// </summary>
        /// <param name="color">The fill color.</param>
        /// <param name="center">The location of the center of the circle.</param>
        /// <param name="radius">The radius of the circle in pixels.</param>
        /// <param name="clockwise">Specifies if the circle will be drawn clockwise. The direction might affect the result since the shapes are rendered using the non-zero winding rule.</param>
        public void FillCircle(Color color, PointF center, float radius, bool clockwise)
        {
            FillPolygonCore(nextID++, color, PolygonFactory.FromCircle(center, radius, clockwise));
        }
        
        private ColorRange[] Render()
        {
            this.edges.Sort();
            
            var writer = new BitmapWriter(BackgroundColor, width, height);
            var superSampleBuffer = new SuperSampleBuffer(width);

            var layers = new LayerManager[SuperSampling.SamplesPerPixelY];
            var superSampleRanges = new SuperSampleRangeList();

            // Keeps track of how many of the subpixellayers that are used for 
            // the currently rendered scanline. Until a range requiring supersampling
            // is encountered only a single layer is needed.
            var usedLayers = 0;

            var color = default(Color);
            var intersections = new IntersectionList();
            
            // Create a layer manager for every subpixel scanline
            for (var i = 0; i < layers.Length; i++)
            {
                layers[i] = new LayerManager();
            }

            for (var ey = 0; ey < height; ey++)
            {
                var ranges = this.edges[ey];
                if (ranges.Count == 0)
                {
                    writer.Skip(width);
                    continue;
                }
                
                for (var i = 0; i < usedLayers; i++)
                {
                    layers[i].Clear();
                }
                usedLayers = 1;

                superSampleRanges.Populate(ranges);
                
                writer.Skip(superSampleRanges[0].FromX);

                for (var rangeIndex = 0; rangeIndex < superSampleRanges.Count; rangeIndex++)
                {
                    ref var superSampleRange = ref superSampleRanges[rangeIndex];

                    // If there is exactly one edge in the supersample range, and it is crossing
                    // the entire scanline, we can perform the antialiasing by integrating the
                    // edge function.
                    if (superSampleRange.Count == 1 && (
                        superSampleRange[0].From.Y <= ey && superSampleRange[0].To.Y >= ey + 1 ||
                        superSampleRange[0].From.Y >= ey + 1 && superSampleRange[0].To.Y <= ey
                        ))
                    {
                        var edge = superSampleRange[0];

                        // Determine the lower and upper x value where the edge 
                        // intersects the scanline.
                        var xey = edge.Intersection(ey);
                        var xey1 = edge.Intersection(ey + 1);
                        var x0 = Math.Min(xey, xey1);
                        var x1 = Math.Max(xey, xey1);
                        var width = x1 - x0;

                        // Compute the average color of all subpixel layers before
                        // and after the edge intersection.
                        var fromColorAverage = new AverageColor();
                        var toColorAverage = new AverageColor();

                        for (var sy = 0; sy < usedLayers; sy++)
                        {
                            var subScanlineLayers = layers[sy];
                            fromColorAverage.Add(subScanlineLayers.CurrentColor);
                            toColorAverage.Add(subScanlineLayers.Add(edge));
                        }

                        var fromColor = fromColorAverage.Color;
                        color = toColorAverage.Color;

                        // Render pixels
                        for (var x = superSampleRange.FromX; x < superSampleRange.ToXExcl; x++)
                        {
                            if (x0 >= x + 1)
                            {
                                // Pixel not covered
                                writer.Write(fromColor);
                                continue;
                            }

                            if (x1 <= x)
                            {
                                // Pixel fully covered
                                writer.Write(color);
                                continue;
                            }
                            
                            // toColor coverage in the range [0.0, 1.0]
                            // Initialize to the fully covered range of the pixel.
                            var coverage = x1 < x + 1 ? x + 1 - x1 : 0;

                            // Compute integral for non-vertical edges
                            if (width > 0.001f)
                            {
                                // Range to integrate
                                var integralFrom = Math.Max(x0, x) - x0;
                                var integralTo = Math.Min(x1, x + 1) - x0;

                                coverage += (integralTo * integralTo - integralFrom * integralFrom) / (2 * width);
                            }

                            writer.Write(Color.Mix(fromColor, color, coverage));
                        }

                    } // /simplified antialiasing
                    else
                    {
                        // There are more than a single intersecting edge in this range.
                        // Use super sampling to render the pixels.
                        intersections.Initialize(ranges.Count);

                        var y = ey + SuperSampling.SampleHeight / 2;

                        // Ensure all subpixel layers are initialized
                        while (usedLayers < SuperSampling.SamplesPerPixelY)
                        {
                            layers[0].CopyTo(layers[usedLayers]);
                            usedLayers++;
                        }

                        // Average color of the pixels following the current supersample range.
                        var forwardColorAverage = new AverageColor();

                        for (var sy = 0; sy < SuperSampling.SamplesPerPixelY; sy++, y += SuperSampling.SampleHeight)
                        {
                            var subScanlineLayers = layers[sy];
                            color = subScanlineLayers.CurrentColor;

                            superSampleRange.GetIntersections(ref intersections, y);

                            for (var i = 0; i < intersections.Count; i++)
                            {
                                ref var intersection = ref intersections[i];
                                superSampleBuffer.Add(color, intersection.X - superSampleRange.FromX);
                                color = subScanlineLayers.Add(intersection.Edge);
                            }

                            // Write an extra pixel that will contain the color that
                            // will be forwarded until the next supersample range.
                            superSampleBuffer.Add(color, superSampleRange.Width);
                            superSampleBuffer.Rewind();

                            forwardColorAverage.Add(color);
                        } // /subpixel

                        // Get color to be forwarded
                        color = forwardColorAverage.Color;

                        // Blend subpixels
                        superSampleBuffer.WriteTo(ref writer, superSampleRange.Width);
                        superSampleBuffer.Clear();
                    } // /supersampling

                    // Forward last color
                    if (rangeIndex + 1 < superSampleRanges.Count)
                    {
                        var nextRangeX = superSampleRanges[rangeIndex + 1].FromX;
                        writer.Write(color, nextRangeX - superSampleRange.ToXExcl);
                    }
                    else
                    {
                        writer.Write(color, width - superSampleRange.ToXExcl);
                    }
                } // /range
            }
            
            return writer.ToArray();
        }

        /// <summary>
        /// Encodes this bitmap as a PNG stream.
        /// </summary>
        /// <param name="stream">The stream to write the PNG encoded data to.</param>
        public void SaveAsPng(Stream stream)
        {
            var canvas = Render();
            var encoder = new PngEncoder
            {
                Canvas = canvas,
                Height = height,
                Width = width
            };
            encoder.Save(stream);
        }
    }
}

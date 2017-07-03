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
    public class Bitmap
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
            this.edges = new EdgeTable(height);
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

            var i = 0;
            var color = default(Color);
            var intersections = new IntersectionList();
            
            for (i = 0; i < layers.Length; i++)
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

                for (i = 0; i < layers.Length; i++)
                {
                    layers[i].Clear();
                }

                superSampleRanges.Populate(ranges);
                
                writer.Skip(superSampleRanges[0].FromX);

                for (var rangeIndex = 0; rangeIndex < superSampleRanges.Count; rangeIndex++)
                {
                    ref var superSampleRange = ref superSampleRanges[rangeIndex];
                    
                    intersections.Initialize(ranges.Count);

                    var y = ey + SuperSampling.SampleHeight / 2;

                    for (var sy = 0; sy < SuperSampling.SamplesPerPixelY; sy++, y += SuperSampling.SampleHeight)
                    {
                        var subScanlineLayers = layers[sy];
                        color = subScanlineLayers.CurrentColor;

                        superSampleRange.GetIntersections(ref intersections, y);
                        
                        for (i = 0; i < intersections.Count; i++)
                        {
                            ref var intersection = ref intersections[i];
                            superSampleBuffer.Add(color, intersection.X - superSampleRange.FromX);
                            color = subScanlineLayers.Add(intersection.Edge);
                        }
                        
                        superSampleBuffer.Add(color, superSampleRange.Width);
                        superSampleBuffer.Rewind();
                    } // /subpixel
                    
                    // Blend subpixels
                    superSampleBuffer.WriteTo(ref writer, superSampleRange.Width);
                    superSampleBuffer.Clear();

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

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

using Jdenticon.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Jdenticon.Drawing.Rasterization
{
    internal class LayerManager
    {
        [DebuggerDisplay("{PolygonID} {Color} {Winding}")]
        struct Layer
        {
            public int PolygonID;
            public Color Color;
            public int Winding;
        }

        private Buffer<Layer> layers = new Buffer<Layer>(10);
        private Color color;

        public void CopyTo(LayerManager other)
        {
            for (var i = 0; i < layers.Count; i++)
            {
                other.layers.Add(layers[i]);
            }

            other.color = color;
        }
        
        public Color Add(Edge intersection)
        {
            var color = default(Color);

            var i = layers.Count - 1;

            for ( ; i >= 0; i--)
            {
                if (layers[i].PolygonID == intersection.PolygonID)
                {
                    layers[i].Winding += intersection.From.Y < intersection.To.Y ? 1 : -1;

                    if (layers[i].Winding == 0)
                    {
                        layers.RemoveAt(i);
                    }
                    else if (color.A != 255)
                    {
                        color = color.Over(layers[i].Color);
                    }

                    goto CompleteColor;
                }
                else if (layers[i].PolygonID < intersection.PolygonID)
                {
                    // Insert here
                    layers.Insert(i + 1, new Layer
                    {
                        PolygonID = intersection.PolygonID,
                        Color = intersection.Color,
                        Winding = intersection.From.Y < intersection.To.Y ? 1 : -1
                    });

                    if (color.A != 255)
                    {
                        color = color.Over(intersection.Color);

                        if (color.A != 255)
                        {
                            color = color.Over(layers[i].Color);
                        }
                    }

                    goto CompleteColor;
                }
                else
                {
                    color = color.Over(layers[i].Color);
                }
            }

            layers.Insert(0, new Layer
            {
                PolygonID = intersection.PolygonID,
                Color = intersection.Color,
                Winding = intersection.From.Y < intersection.To.Y ? 1 : -1
            });
            color = color.Over(intersection.Color);

            CompleteColor:

            for (i--; i >= 0 && color.A != 255; i--)
            {
                color = color.Over(layers[i].Color);
            }

            this.color = color;
            return color;
        }

        public void Clear()
        {
            layers.Clear();
            color = default(Color);
        }

        public Color CurrentColor => this.color;
    }
}

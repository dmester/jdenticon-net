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
        }

        public Color CurrentColor => this.color;
    }
}

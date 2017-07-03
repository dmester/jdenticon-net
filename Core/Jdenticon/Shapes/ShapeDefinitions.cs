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
using System.Text;

namespace Jdenticon.Shapes
{
    /// <summary>
    /// Provides definitions for the default shapes used in identicons.
    /// </summary>
    public static class ShapeDefinitions
    {
        static ShapeDefinitions()
        {
            OuterShapes = new ShapeDefinition[]
            {
                (renderer, cell, index) =>
                {
                    renderer.AddTriangle(0, 0, cell, cell, 0);
                },
                (renderer, cell, index) =>
                {
                    renderer.AddTriangle(0, cell / 2f, cell, cell / 2f, 0);
                },
                (renderer, cell, index) =>
                {
                    renderer.AddRhombus(0, 0, cell, cell);
                },
                (renderer, cell, index) =>
                {
                    var m = cell / 6;
                    renderer.AddCircle(m, m, cell - 2 * m);
                },
            };

            CenterShapes = new ShapeDefinition[]
            {
                (renderer, cell, index) =>
                {
                    var k = cell * 0.42f;
                    renderer.AddPolygon(new []
                    {
                        new PointF(0,0),
                        new PointF(cell,0),
                        new PointF(cell,cell - k*2),
                        new PointF(cell - k, cell),
                        new PointF(0, cell)
                    });
                },
                (renderer, cell, index) =>
                {
                    var w = (int)(cell * 0.5);
                    var h = (int)(cell * 0.8);
                    renderer.AddTriangle(cell - w, 0, w, h, TriangleDirection.NorthEast);
                },
                (renderer, cell, index) =>
                {
                    var s = cell / 3;
                    renderer.AddRectangle(s, s, cell - s, cell - s);
                },
                (renderer, cell, index) =>
                {
                    var tmp = cell * 0.1f;

                    var inner =
                        tmp > 1 ? (int)tmp : // large icon => truncate decimals
                        tmp > 0.5 ? 1 :      // medium size icon => fixed width
                        tmp;                 // small icon => anti-aliased border

                    // Use fixed outer border widths in small icons to ensure the border is drawn
                    var outer =
                        cell < 6 ? 1 :
                        cell < 8 ? 2 :
                        cell / 4;

                    renderer.AddRectangle(outer, outer, cell - inner - outer, cell - inner - outer);
                },
                (renderer, cell, index) =>
                {
                    var m = (int)(cell * 0.15);
                    var s = (int)(cell * 0.5);
                    renderer.AddCircle(cell - s - m, cell - s - m, s);
                },
                (renderer, cell, index) =>
                {
                    var inner = cell * 0.1f;
                    var outer = inner * 4;

                    renderer.AddRectangle(0, 0, cell, cell);
                    renderer.AddPolygon(new []
                    {
                        new PointF(outer, outer),
                        new PointF(cell - inner, outer),
                        new PointF(outer + (cell - outer - inner) / 2, cell - inner)
                    }, true);
                },
                (renderer, cell, index) =>
                {
                    renderer.AddPolygon(new []
                    {
                        new PointF(0, 0),
                        new PointF(cell, 0),
                        new PointF(cell, cell * 0.7f),
                        new PointF(cell * 0.4f, cell * 0.4f),
                        new PointF(cell * 0.7f, cell),
                        new PointF(0, cell)
                    });
                },
                (renderer, cell, index) =>
                {
                    renderer.AddTriangle(cell / 2f, cell / 2f, cell / 2f, cell / 2f, TriangleDirection.SouthEast);
                },
                (renderer, cell, index) =>
                {
                    renderer.AddPolygon(new []
                    {
                        new PointF(0, 0),
                        new PointF(cell, 0),
                        new PointF(cell, cell / 2),
                        new PointF(cell / 2, cell),
                        new PointF(0, cell)
                    });
                },
                (renderer, cell, index) =>
                {
                    var tmp = cell * 0.14f;
                    var inner =
                         cell < 8 ? tmp : // small icon => anti-aliased border
                         (int)tmp;        // large icon => truncate decimals
 
                     // Use fixed outer border widths in small icons to ensure the border is drawn
                     var outer =
                         cell < 4 ? 1 :
                         cell < 6 ? 2 :
                         (int)(cell * 0.35f);

                    renderer.AddRectangle(0, 0, cell, cell);
                    renderer.AddRectangle(outer, outer, cell - outer - inner, cell - outer - inner, true);
                },
                (renderer, cell, index) =>
                {
                    var inner = cell * 0.12f;
                    var outer = inner * 3;

                    renderer.AddRectangle(0, 0, cell, cell);
                    renderer.AddCircle(outer, outer, cell - inner - outer, true);
                },
                (renderer, cell, index) =>
                {
                    renderer.AddTriangle(cell / 2f, cell / 2f, cell / 2f, cell / 2f, TriangleDirection.SouthEast);
                },
                (renderer, cell, index) =>
                {
                    var m = cell * 0.25f;

                    renderer.AddRectangle(0, 0, cell, cell);
                    renderer.AddRhombus(m, m, cell - m, cell - m, true);
                },
                (renderer, cell, index) =>
                {
                    var m = cell * 0.4f;
                    var s = cell * 1.2f;

                    if (index != 0)
                    {
                        renderer.AddCircle(m, m, s);
                    }
                }
            };
        }

        /// <summary>
        /// Gets the definition of the shapes that are placed in the center of the icon.
        /// </summary>
        public static IList<ShapeDefinition> CenterShapes { get; private set; }

        /// <summary>
        /// Gets the definition of the shapes that are placed around the center of the icon.
        /// </summary>
        public static IList<ShapeDefinition> OuterShapes { get; private set; }
    }
}

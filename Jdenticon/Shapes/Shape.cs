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
    /// Represents a shape to be rendered in an icon. These instances are hash specific.
    /// </summary>
    public class Shape
    {
        /// <summary>
        /// The shape definition to be used to render the shape.
        /// </summary>
        public ShapeDefinition Definition { get; set; }

        /// <summary>
        /// The fill color of the shape.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The positions in which the shape will be rendered.
        /// </summary>
        public ShapePositionCollection Positions { get; set; }

        /// <summary>
        /// The rotation index of the icon in the first position.
        /// </summary>
        public int StartRotationIndex { get; set; }
    }
}

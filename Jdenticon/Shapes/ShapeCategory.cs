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
using System.Text;

namespace Jdenticon.Shapes
{
    /// <summary>
    /// Represents a category of shapes that can be rendered in an icon. These instances are
    /// not hash specific.
    /// </summary>
    public class ShapeCategory
    {
        /// <summary>
        /// The index of the hash octet determining the color of shapes in this category.
        /// </summary>
        public int ColorIndex { get; set; }

        /// <summary>
        /// A list of possible shape definitions in this category.
        /// </summary>
        public IList<ShapeDefinition> Shapes { get; set; }

        /// <summary>
        /// The index of the hash octet determining which of the shape definitions that will be used 
        /// for a particular hash.
        /// </summary>
        public int ShapeIndex { get; set; }

        /// <summary>
        /// The index of the hash octet determining the rotation index of the shape in the first position.
        /// </summary>
        public int? RotationIndex { get; set; }

        /// <summary>
        /// The positions in which the shapes of this category will be rendered.
        /// </summary>
        public ShapePositionCollection Positions { get; set; }
    }
}

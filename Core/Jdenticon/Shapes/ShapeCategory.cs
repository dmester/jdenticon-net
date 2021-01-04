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

#nullable enable

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
        public IList<ShapeDefinition> Shapes { get; set; } = new ShapeDefinition[0];

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
        public ShapePositionCollection Positions { get; set; } = new ShapePositionCollection();
    }
}

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
using Jdenticon.Wpf.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Jdenticon
{
    /// <summary>
    /// Extends <see cref="Identicon"/> with WPF specific methods.
    /// </summary>
    public static class WpfExtensions
    {
        /// <summary>
        /// Renders an <see cref="Identicon"/> as a WPF <see cref="Visual"/>.
        /// </summary>
        /// <param name="icon">The identicon to convert to a <see cref="Visual"/>.</param>
        public static Visual ToVisual(this Identicon icon)
        {
            var visual = new DrawingVisual();
            
            using (var context = visual.RenderOpen())
            {
                icon.Draw(context);
            }

            return visual;
        }

        /// <summary>
        /// Draws an <see cref="Identicon"/> in a specified WPF drawing context.
        /// </summary>
        /// <param name="icon">The identicon to draw.</param>
        /// <param name="drawingContext">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon, including padding.</param>
        public static void Draw(this Identicon icon, DrawingContext drawingContext, Rendering.Rectangle rect)
        {
            var renderer = new WpfRenderer(drawingContext, rect.Width, rect.Height);
            icon.Draw(renderer, rect);
        }

        /// <summary>
        /// Draws an <see cref="Identicon"/> in a specified WPF drawing context.
        /// </summary>
        /// <param name="icon">The identicon to draw.</param>
        /// <param name="drawingContext">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon, including padding.</param>
        public static void Draw(this Identicon icon, DrawingContext drawingContext, System.Windows.Rect rect)
        {
            icon.Draw(drawingContext, rect.ToJdenticon());
        }

        /// <summary>
        /// Draws an <see cref="Identicon"/> in a specified WPF drawing context at position (0, 0).
        /// </summary>
        /// <param name="icon">The identicon to draw.</param>
        /// <param name="drawingContext">Drawing context in which the icon will be rendered.</param>
        public static void Draw(this Identicon icon, DrawingContext drawingContext)
        {
            var renderer = new WpfRenderer(drawingContext, icon.Size, icon.Size);
            icon.Draw(renderer);
        }
    }
}

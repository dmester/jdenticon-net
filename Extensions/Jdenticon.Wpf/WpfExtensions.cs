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
        /// Renders the icon as a WPF <see cref="Visual"/>.
        /// </summary>
        /// <param name="size">The size of the generated bitmap in pixels.</param>
        public static Visual ToVisual(this Identicon icon)
        {
            var visual = new DrawingVisual();
            
            using (var context = visual.RenderOpen())
            {
                var iconBounds = icon.GetIconBounds();
                icon.Draw(context, iconBounds);
            }

            return visual;
        }
        
        /// <summary>
        /// Draws this icon in the specified drawing context.
        /// </summary>
        /// <param name="drawingContext">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon. No padding will be applied to the rectangle.</param>
        public static void Draw(this Identicon icon, DrawingContext drawingContext, Rendering.Rectangle rect)
        {
            var renderer = new WpfRenderer(drawingContext, rect.Width, rect.Height);
            icon.Draw(renderer, rect);
        }

        /// <summary>
        /// Draws this icon in the specified drawing context.
        /// </summary>
        /// <param name="drawingContext">Drawing context in which the icon will be rendered.</param>
        /// <param name="rect">The bounds of the rendered icon. No padding will be applied to the rectangle.</param>
        public static void Draw(this Identicon icon, DrawingContext drawingContext, System.Windows.Rect rect)
        {
            icon.Draw(drawingContext, rect.ToJdenticon());
        }
    }
}

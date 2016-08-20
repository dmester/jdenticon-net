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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jdenticon.Rendering.Svg
{
    /// <summary>
    /// Renders icons as SVG paths.
    /// </summary>
    public class SvgRenderer : Renderer
    {
        private Dictionary<Color, SvgPath> pathsByColor = new Dictionary<Color, SvgPath>();
        private SvgPath path;
        private int width, height;
        private Color backColor;

        public SvgRenderer(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        protected override void AddCircleNoTransform(PointF location, float diameter, bool counterClockwise)
        {
            path.AddCircle(location, diameter, counterClockwise);
        }

        protected override void AddPolygonNoTransform(PointF[] points)
        {
            path.AddPolygon(points);
        }

        public override void SetBackground(Color color)
        {
            backColor = color;
        }

        public override IDisposable BeginShape(Color color)
        {
            if (!pathsByColor.TryGetValue(color, out path))
            {
                pathsByColor[color] = path = new SvgPath();
            }

            return ActionDisposable.Empty;
        }
        
        public void Save(TextWriter writer, bool fragment)
        {
            var invariantCulture = CultureInfo.InvariantCulture;
            var widthAsString = width.ToString(invariantCulture);
            var heightAsString = height.ToString(invariantCulture);

            if (!fragment)
            {
                writer.Write("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"" +
                    widthAsString + "\" height=\"" + heightAsString + "\" viewBox=\"0 0 " +
                    widthAsString + " " + heightAsString + "\" preserveAspectRatio=\"xMidYMid meet\">");
            }

            if (backColor.A > 0)
            {
                var opacity = (float)backColor.A / 255;
                writer.Write("<rect fill=\"" + ColorUtils.ToHexString(backColor) + "\" fill-opacity=\"" +
                    opacity.ToString(invariantCulture) +
                    "\" x=\"0\" y=\"0\" width=\"" + widthAsString + "\" height=\"" + heightAsString + "\"/>");
            }
            
            foreach (var pair in pathsByColor)
            {
                writer.Write("<path fill=\"" + ColorUtils.ToHexString(pair.Key) + "\" d=\"" + pair.Value + "\"/>");
            }

            if (!fragment)
            {
                writer.Write("</svg>");
            }
        }
    }
}

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

namespace Jdenticon
{
    /// <summary>
    /// Specifies the color style of an identicon.
    /// </summary>
    public class IdenticonStyle : IEquatable<IdenticonStyle>
    {
        private Color backColor = DefaultBackColor;
        private float padding = DefaultPadding;
        private float saturation = DefaultSaturation;
        private Range<float> colorLightness = DefaultColorLightness;
        private Range<float> grayscaleLightness = DefaultGrayscaleLightness;

        /// <summary>
        /// Gets the default value of the <see cref="Padding"/> property. Resolves to 0.08f.
        /// </summary>
        public static float DefaultPadding => 0.08f;

        /// <summary>
        /// Gets the default value of the <see cref="ColorLightness"/> property. Resolves to [0.4f, 0.8f].
        /// </summary>
        public static Range<float> DefaultColorLightness => Range.Create(0.4f, 0.8f);

        /// <summary>
        /// Gets the default value of the <see cref="GrayscaleLightness"/> property. Resolves to [0.3f, 0.9f].
        /// </summary>
        public static Range<float> DefaultGrayscaleLightness => Range.Create(0.3f, 0.9f);

        /// <summary>
        /// Gets the default value of the <see cref="Saturation"/> property. Resolves to 0.5f.
        /// </summary>
        public static float DefaultSaturation => 0.5f;

        /// <summary>
        /// Gets the default value of the <see cref="BackColor"/> property. Resolves to <see cref="Color.White"/>.
        /// </summary>
        public static Color DefaultBackColor => Color.White;
        
        /// <summary>
        /// The background color of the icon. Set to <see cref="Color.Transparent"/> to remove the background.
        /// </summary>
        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }
        
        /// <summary>
        /// Gets or sets the padding between the edge of the image and the bounds of the rendered icon. The value is specified in percent in the range [0.0, 0.4].
        /// </summary>
        public float Padding
        {
            get { return padding; }
            set
            {
                if (padding < 0f || padding > 0.4f)
                {
                    throw new ArgumentOutOfRangeException(nameof(Padding),
                        value, "Only padding values in the range [0.0, 0.4] are valid.");
                }
                padding = value;
            }
        }
        
        /// <summary>
        /// The saturation of the icon in the range [0.0f, 1.0f].
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was less than 0.0f or greater than 1.0f.</exception>
        public float Saturation
        {
            get { return saturation; }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(Saturation), value,
                        "Only saturation values in the range [0.0, 1.0] are allowed.");
                }

                saturation = value;
            }
        }

        /// <summary>
        /// The lightness range of the colored shapes in the icon.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The lightness of the shapes can be inverted by specifying a range where <see cref="Range{TValue}.From"/> 
        /// is greater than <see cref="Range{TValue}.To"/>. The normally darker shapes will then be lighter than 
        /// the normally lighter ones.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or both range bounds were less than 0.0f or greater than 1.0f.
        /// </exception>
        public Range<float> ColorLightness
        {
            get { return colorLightness; }
            set
            {
                if (value.From < 0 || value.From > 1)
                {
                    throw new ArgumentOutOfRangeException("ColorLightness.From", value.From, 
                        "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                if (value.To < 0 || value.To > 1)
                {
                    throw new ArgumentOutOfRangeException("ColorLightness.To", value.To, 
                        "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                colorLightness = value;
            }
        }

        /// <summary>
        /// The lightness range of the grayscale shapes in the icon. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// The lightness of the shapes can be inverted by specifying a range where <see cref="Range{TValue}.From"/> 
        /// is greater than <see cref="Range{TValue}.To"/>. The normally darker shapes will then be lighter than 
        /// the normally lighter ones.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or both range bounds were less than 0.0f or greater than 1.0f.
        /// </exception>
        public Range<float> GrayscaleLightness
        {
            get { return grayscaleLightness; }
            set
            {
                if (value.From < 0 || value.From > 1)
                {
                    throw new ArgumentOutOfRangeException("GrayscaleLightness.From", value.From, "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                if (value.To < 0 || value.To > 1)
                {
                    throw new ArgumentOutOfRangeException("GrayscaleLightness.To", value.To, "Only lightness values in the range [0.0, 1.0] are allowed.");
                }
                grayscaleLightness = value;
            }
        }

        /// <summary>
        /// Gets a hash code for this <see cref="IdenticonStyle"/>.
        /// </summary>
        public override int GetHashCode()
        {
            return
                padding.GetHashCode() ^
                saturation.GetHashCode() ^
                backColor.GetHashCode() ^
                colorLightness.GetHashCode() ^
                grayscaleLightness.GetHashCode();
        }

        /// <summary>
        /// Checks if this style is identical to another style.
        /// </summary>
        /// <param name="obj">The <see cref="IdenticonStyle"/> to compare.</param>
        public override bool Equals(object obj)
        {
            return Equals(obj as IdenticonStyle);
        }

        /// <summary>
        /// Checks if this style is identical to another style.
        /// </summary>
        /// <param name="other">The <see cref="IdenticonStyle"/> to compare.</param>
        public bool Equals(IdenticonStyle other)
        {
            return
                other != null &&
                other.padding == padding &&
                other.backColor == backColor &&
                other.colorLightness == colorLightness &&
                other.grayscaleLightness == grayscaleLightness &
                other.saturation == saturation;
        }
    }
}

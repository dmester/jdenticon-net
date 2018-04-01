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
using System.ComponentModel;
using System.Text;

namespace Jdenticon
{
    /// <summary>
    /// Specifies the color style of an identicon.
    /// </summary>
    public class IdenticonStyle : IEquatable<IdenticonStyle>
    {
        private HueCollection hues = new HueCollection();
        private Color backColor = DefaultBackColor;
        private float padding = DefaultPadding;
        private float colorSaturation = DefaultColorSaturation;
        private float grayscaleSaturation = DefaultGrayscaleSaturation;
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

        /// <exclude/>
        [Obsolete("Use DefaultColorSaturation instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static float DefaultSaturation => DefaultColorSaturation;

        /// <summary>
        /// Gets the default value of the <see cref="ColorSaturation"/> property. Resolves to 0.5f.
        /// </summary>
        public static float DefaultColorSaturation => 0.5f;

        /// <summary>
        /// Gets the default value of the <see cref="GrayscaleSaturation"/> property. Resolves to 0f.
        /// </summary>
        public static float DefaultGrayscaleSaturation => 0f;

        /// <summary>
        /// Gets the default value of the <see cref="BackColor"/> property. Resolves to <see cref="Color.White"/>.
        /// </summary>
        public static Color DefaultBackColor => Color.White;

        /// <summary>
        /// Gets or sets a collection of the allowed hues in the generated icons. If the collection is empty
        /// all hues are allowed.
        /// </summary>
        /// <remarks>
        /// This property always returns a collection, even after setting the property to <c>Nothing</c>.
        /// </remarks>
        public HueCollection Hues
        {
            get => hues;
            set => hues = value ?? new HueCollection();
        }

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

        /// <exclude/>
        [Obsolete("Use ColorSaturation instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public float Saturation
        {
            get { return ColorSaturation; }
            set { ColorSaturation = value; }
        }

        /// <summary>
        /// The saturation of the colored shapes in the icon. Valid values are [0.0f, 1.0f].
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was less than 0.0f or greater than 1.0f.</exception>
        /// <remarks>
        /// This property was previously called <see cref="Saturation"/> but was renamed in version 2.1.0.
        /// </remarks>
        public float ColorSaturation
        {
            get { return colorSaturation; }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(ColorSaturation), value,
                        "Only saturation values in the range [0.0, 1.0] are allowed.");
                }

                colorSaturation = value;
            }
        }

        /// <summary>
        /// The saturation of the by default grayscale shapes in the icon. The same hue is used for colored and grayscale shapes. Valid values are [0.0f, 1.0f].
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">The value was less than 0.0f or greater than 1.0f.</exception>
        /// <remarks>
        /// This property is available since version 2.1.0.
        /// </remarks>
        public float GrayscaleSaturation
        {
            get { return grayscaleSaturation; }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(GrayscaleSaturation), value,
                        "Only saturation values in the range [0.0, 1.0] are allowed.");
                }

                grayscaleSaturation = value;
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
                colorSaturation.GetHashCode() ^
                grayscaleSaturation.GetHashCode() ^
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
                other.colorSaturation == colorSaturation &&
                other.grayscaleSaturation == grayscaleSaturation &&
                other.hues.Equals(hues);
        }
    }
}
